using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace QueueRunner
{
    public class ProgramQueueItem
    {
        public string Executable { get; set; }
        public string Arguments { get; set; }
        public TimeSpan Timeout { get; set; }

        [XmlIgnore]
        public FinishType Finished { get; internal set; }

        [XmlIgnore]
        public int? ExitCode { get; internal set; }

        [XmlIgnore]
        public TimeSpan? Runtime { get; internal set; }

        public enum FinishType
        {
            NotStarted,
            InProgress,
            Complete,
            Timeout,
            Terminated,
            NonZeroExitCode
        }
        
        public ProgramQueueItem()
            : this("", "", 0)
        {

        }

        public ProgramQueueItem(ProgramQueueItem other)
            : this(other.Executable, other.Arguments, other.Timeout)
        {

        }

        public ProgramQueueItem(string Executable, string Arguments, decimal Timeout)
        {
            Set(Executable, Arguments, Timeout);
            Finished = FinishType.NotStarted;
            ExitCode = null;
        }

        public ProgramQueueItem(string Executable, string Arguments, TimeSpan Timeout)
        {
            Set(Executable, Arguments, Timeout);
        }

        public void Set(string Executable, string Arguments, decimal Timeout)
        {
            this.Executable = Executable;
            this.Arguments = Arguments;
            this.Timeout = TimeSpan.FromSeconds((double)Timeout);
        }

        public void Set(string Executable, string Arguments, TimeSpan Timeout)
        {
            this.Executable = Executable;
            this.Arguments = Arguments;
            this.Timeout = Timeout;
        }

        public void Reset()
        {
            Finished = FinishType.NotStarted;
            ExitCode = null;
            Runtime = null;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Timeout.Ticks > 0)
            {
                sb.Append($"({Timeout.ToString("g")}) ");
            }
            sb.Append(Path.GetFileName(Executable));

            if (!string.IsNullOrWhiteSpace(Arguments))
            {
                sb.Append(" ");
                sb.Append(Arguments);
            }

            if(ExitCode.HasValue)
            {
                sb.Append($" (Exited: {ExitCode.Value})");
            }

            switch(Finished)
            {
                case FinishType.Terminated:
                    sb.Append(" (Terminated)");
                    break;

                case FinishType.Timeout:
                    sb.Append(" (Timeout)");
                    break;
            }

            if(Runtime.HasValue)
            {
                sb.Append(" - ");
                sb.Append(Runtime.Value.ToString("g"));
            }

            return sb.ToString();
        }
    }
}
