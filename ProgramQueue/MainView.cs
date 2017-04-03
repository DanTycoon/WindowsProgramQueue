using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using QueueRunner;

namespace ProgramQueue
{
    public partial class MainView : Form
    {
        private List<ProgramQueueItem> queueItems;
        private bool running;
        private bool stopping;
        private Timer taskTimer;
        private QueueRunner.QueueRunner runner;

        public MainView()
        {
            InitializeComponent();
            queueItems = new List<ProgramQueueItem>();
            running = false;
        }

        private void MainView_Load(object sender, EventArgs e)
        {
            itemList.DataSource = queueItems;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (running)
            {
                return;
            }

            var window = new AddItem();
            var result = window.ShowDialog();

            if (result == DialogResult.OK)
            {
                queueItems.Add(new ProgramQueueItem(window.executableTextbox.Text, window.argumentsTextbox.Text, window.timeoutTextbox.Value));
                UpdateItemBox();
            }
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (running)
            {
                return;
            }

            var selectedItems = itemList.SelectedIndices;

            if(selectedItems.Count == 0)
            {
                return;
            }

            var confirmResult = MessageBox.Show($"Are you sure you want to remove these {selectedItems.Count} item(s)?",
                "Confirm removal",
                MessageBoxButtons.YesNo);

            if (confirmResult == DialogResult.Yes)
            {
                var toRemove = SelectedIndices();

                foreach (int removeIndex in toRemove.OrderByDescending(x => x))
                {
                    queueItems.RemoveAt(removeIndex);
                }
            }

            UpdateItemBox();
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (running)
            {
                return;
            }

            if (itemList.SelectedIndices.Count == 0)
            {
                return;
            }

            if (itemList.SelectedIndices.Count > 1)
            {
                MessageBox.Show("You can only edit items one at a time.\nPlease select only one item.", "Too many items selected.");
                return;
            }

            var item = (ProgramQueueItem)itemList.Items[itemList.SelectedIndex];
            var window = new AddItem(item);

            var result = window.ShowDialog();

            if(result == DialogResult.OK)
            {
                item.Set(window.executableTextbox.Text, window.argumentsTextbox.Text, window.timeoutTextbox.Value);
                UpdateItemBox();
            }
        }

        private void UpdateItemBox()
        {
            itemList.DataSource = null;
            itemList.DataSource = queueItems;
        }

        private void upButton_Click(object sender, EventArgs e)
        {
            if (running)
            {
                return;
            }

            var selectedIndices = SelectedIndices();
            var newSelectedIndices = new List<int>();
            int skipIndex = -1;

            foreach(int i in selectedIndices.OrderBy(x => x))
            {
                if (i == 0 || i == skipIndex)
                {
                    newSelectedIndices.Add(i);
                    skipIndex = i + 1;
                    continue;
                }

                var tmp = queueItems[i];
                queueItems[i] = queueItems[i - 1];
                queueItems[i - 1] = tmp;
                newSelectedIndices.Add(i - 1);
            }

            UpdateItemBox();
            itemList.SelectedIndices.Clear();
            foreach (int i in newSelectedIndices)
            {
                itemList.SelectedIndices.Add(i);
            }
        }

        private List<int> SelectedIndices()
        {
            var selectedIndices = new List<int>();
            foreach (int i in itemList.SelectedIndices)
            {
                selectedIndices.Add(i);
            }

            return selectedIndices;
        }

        private void downButton_Click(object sender, EventArgs e)
        {
            if(running)
            {
                return;
            }

            var selectedIndices = SelectedIndices();
            var newSelectedIndices = new List<int>();
            int skipIndex = -1;

            foreach (int i in selectedIndices.OrderByDescending(x => x))
            {
                if (i == queueItems.Count - 1 || i == skipIndex)
                {
                    newSelectedIndices.Add(i);
                    skipIndex = i - 1;
                    continue;
                }

                var tmp = queueItems[i];
                queueItems[i] = queueItems[i + 1];
                queueItems[i + 1] = tmp;
                newSelectedIndices.Add(i + 1);
            }

            UpdateItemBox();
            itemList.SelectedIndices.Clear();
            foreach (int i in newSelectedIndices)
            {
                itemList.SelectedIndices.Add(i);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var d = new SaveFileDialog();
            d.AddExtension = true;
            d.DefaultExt = "xml";
            d.Filter = "XML File (*.xml)|*.xml";
            d.OverwritePrompt = true;
            d.RestoreDirectory = true;
            var result = d.ShowDialog();

            if(result == DialogResult.OK)
            {
                SaveLoadFile.SaveQueue(d.FileName, queueItems);
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(running)
            {
                return;
            }

            var d = new OpenFileDialog();
            d.AddExtension = true;
            d.CheckFileExists = true;
            d.CheckPathExists = true;
            d.DefaultExt = "xml";
            d.Filter = "XML File (*.xml)|*.xml";
            d.RestoreDirectory = true;
            var result = d.ShowDialog();

            if (result == DialogResult.OK)
            {
                queueItems = SaveLoadFile.LoadQueue(d.FileName);
            }

            UpdateItemBox();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            if (itemList.SelectedIndices.Count == 0)
            {
                return;
            }

            if (itemList.SelectedIndices.Count > 1)
            {
                MessageBox.Show("You can only duplicate items one at a time.\nPlease select only one item.", "Too many items selected.");
                return;
            }

            int selectedIndex = itemList.SelectedIndex;
            queueItems.Insert(selectedIndex, new ProgramQueueItem(queueItems[selectedIndex]));
            UpdateItemBox();
            itemList.SelectedIndex = selectedIndex;
        }

        private void startStopButton_Click(object sender, EventArgs e)
        {
            if(running)
            {
                StopRunning();
            }
            else
            {
                StartRunning();
            }
        }

        private void StopRunning()
        {
            if(stopping)
            {
                return;
            }

            var result = MessageBox.Show("Do you want to terminate the programs immediately?\nSelecting 'No' will wait for the programs to end on their own.", "Force Exit", MessageBoxButtons.YesNoCancel);
            if(result == DialogResult.Cancel)
            {
                return;
            }
            if(result == DialogResult.Yes)
            {
                runner.StopImmediately();
            }
            else
            {
                // No
                runner.StopOnExit();
            }

            taskTimer.Stop();
            taskTimer.Tick -= UpdateProgress;
            taskTimer.Tick += WaitForExit;
            taskTimer.Start();
            statusText.Text = "Waiting for exit...";
            stopping = true;
        }

        private void FinishRunning()
        {
            running = false;
            stopping = false;
            startStopButton.Image = Resources.play;
            startStopButton.Update();
            taskTimer.Stop();
            statusText.Text = "Complete";
            addButton.Enabled = true;
            removeButton.Enabled = true;
            editButton.Enabled = true;
            copyButton.Enabled = true;
            upButton.Enabled = true;
            downButton.Enabled = true;
            loadToolStripMenuItem.Enabled = true;
        }

        private void StartRunning()
        {
            if(queueItems.Count == 0)
            {
                statusText.Text = "No items in queue!";
                return;
            }

            runner = new QueueRunner.QueueRunner(queueItems, (int)multProcCount.Value);
            if(!runner.Start())
            {
                MessageBox.Show("Runner couldn't start for an unknown reason.", "Process runner failed to start");
                return;
            }

            statusBar.Minimum = 0;
            statusBar.Step = 1;
            statusBar.Maximum = runner.WorkItemCount;

            taskTimer = new Timer();
            taskTimer.Interval = 1000;
            taskTimer.Tick += UpdateProgress;
            taskTimer.Start();

            running = true;
            startStopButton.Image = Resources.stop;
            startStopButton.Update();

            addButton.Enabled = false;
            removeButton.Enabled = false;
            editButton.Enabled = false;
            copyButton.Enabled = false;
            upButton.Enabled = false;
            downButton.Enabled = false;
            loadToolStripMenuItem.Enabled = false;
        }

        private void UpdateProgress(object sender, EventArgs e)
        {
            statusText.Text = $"Running... {runner.CurrentWorkItem}/{runner.WorkItemCount}";
            statusBar.Value = runner.CurrentWorkItem;
            itemList.Refresh();

            if (!runner.Running)
            {
                FinishRunning();
            }
        }

        private void WaitForExit(object sender, EventArgs e)
        {
            if(!runner.Running)
            {
                FinishRunning();
            }
        }

        private void itemList_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            var selected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);
            var index = e.Index;

            if(index >= 0 && index < queueItems.Count)
            {
                var item = queueItems[index];
                var text = item.ToString();
                var graphics = e.Graphics;

                SolidBrush backgroundBrush;
                switch(item.Finished)
                {
                    case ProgramQueueItem.FinishType.Complete:
                        backgroundBrush = new SolidBrush(Color.Lime);
                        break;

                    case ProgramQueueItem.FinishType.InProgress:
                        backgroundBrush = new SolidBrush(Color.Yellow);
                        break;

                    case ProgramQueueItem.FinishType.NonZeroExitCode:
                        backgroundBrush = new SolidBrush(Color.Orange);
                        break;

                    case ProgramQueueItem.FinishType.NotStarted:
                        backgroundBrush = new SolidBrush(Color.White);
                        break;

                    case ProgramQueueItem.FinishType.Terminated:
                        backgroundBrush = new SolidBrush(Color.Red);
                        break;

                    case ProgramQueueItem.FinishType.Timeout:
                        backgroundBrush = new SolidBrush(Color.Pink);
                        break;

                    default:
                        backgroundBrush = new SolidBrush(Color.Gray);
                        break;
                }

                if(selected)
                {
                    backgroundBrush = new SolidBrush(Color.FromKnownColor(KnownColor.Highlight));
                }
                graphics.FillRectangle(backgroundBrush, e.Bounds);

                var foregroundBrush = selected ? new SolidBrush(Color.White) : new SolidBrush(Color.Black);
                graphics.DrawString(text, e.Font, foregroundBrush, itemList.GetItemRectangle(index).Location);
            }

            e.DrawFocusRectangle();
        }
        
    }
}
