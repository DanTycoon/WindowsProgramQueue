using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace QueueRunner
{
    public class QueueRunner
    {
        Queue<ProgramQueueItem> ItemsToRun { get; }
        int ConcurrentProcesses { get; }

        public bool Running { get; private set; }
        public int WorkItemCount { get; }
        public int CurrentWorkItem { get; private set; }

        Thread[] workerThreads;
        ManualResetEvent stopEvent;
        ManualResetEvent stopImmediatelyEvent;
        int runningThreads;
        readonly object _lock = new object();

        public QueueRunner(IEnumerable<ProgramQueueItem> ItemsToRun, int ConcurrentProcesses)
        {
            this.ItemsToRun = new Queue<ProgramQueueItem>(ItemsToRun);
            this.ConcurrentProcesses = ConcurrentProcesses;
            Running = false;
            stopEvent = new ManualResetEvent(false);
            stopImmediatelyEvent = new ManualResetEvent(false);
            WorkItemCount = this.ItemsToRun.Count;

            foreach(var item in this.ItemsToRun)
            {
                item.Reset();
            }
        }

        public bool Start()
        {
            if(Running)
            {
                return false;
            }

            stopEvent.Reset();
            stopImmediatelyEvent.Reset();
            workerThreads = new Thread[ConcurrentProcesses];
            runningThreads = ConcurrentProcesses;
            CurrentWorkItem = 0;

            // Create and start thread
            for(int i = 0; i < ConcurrentProcesses; i++)
            {
                (workerThreads[i] = new Thread(RunProgram)).Start();
            }

            Running = true;

            return true;
        }

        public void StopImmediately()
        {
            stopEvent.Set();
            stopImmediatelyEvent.Set();
        }

        public void StopOnExit()
        {
            stopEvent.Set();
        }



        public void RunProgram()
        {
            // Main loop
            while (true)
            {
                ProgramQueueItem item;
                
                // Check for stop request
                if(stopEvent.WaitOne(0))
                {
                    break;
                }

                // Get next item
                lock (ItemsToRun)
                {
                    // Nothing left
                    if(ItemsToRun.Count == 0)
                    {
                        break;
                    }

                    item = ItemsToRun.Dequeue();
                }

                // Run the program
                var startInfo = new ProcessStartInfo();
                startInfo.Arguments = item.Arguments;
                startInfo.FileName = item.Executable;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;

                // Wait for immediate exit flag, or wait for process to exit
                using (Process p = Process.Start(startInfo))
                {
                    bool normalFinish = true;
                    item.Finished = ProgramQueueItem.FinishType.InProgress;

                    // Check for process exit every quarter second
                    while (p.WaitForExit(250) == false)
                    {
                        // Update runtime
                        item.Runtime = (DateTime.UtcNow - p.StartTime.ToUniversalTime());

                        // Don't wait here, since we're already waiting on the process every 250 ms
                        if (stopImmediatelyEvent.WaitOne(0))
                        {
                            normalFinish = false;
                            item.Finished = ProgramQueueItem.FinishType.Terminated;
                            p.Kill();
                            break;
                        }

                        // Look for timeout condition
                        if(item.Timeout.Seconds > 0 && (DateTime.UtcNow - p.StartTime.ToUniversalTime()) > item.Timeout)
                        {
                            normalFinish = false;
                            item.Finished = ProgramQueueItem.FinishType.Timeout;
                            p.Kill();
                            break;
                        }
                    }

                    if(normalFinish)
                    {
                        item.ExitCode = p.ExitCode;
                        if (p.ExitCode == 0)
                        {
                            item.Finished = ProgramQueueItem.FinishType.Complete;
                        }
                        else
                        {
                            item.Finished = ProgramQueueItem.FinishType.NonZeroExitCode;
                        }
                    }

                    item.Runtime = (DateTime.UtcNow - p.StartTime.ToUniversalTime());
                }

                // Update progress
                lock (_lock)
                {
                    ++CurrentWorkItem;
                }
            }

            // Cleanup
            lock (_lock)
            {
                --runningThreads;

                if(runningThreads == 0)
                {
                    Running = false;
                }
            }
        }
    }
}
