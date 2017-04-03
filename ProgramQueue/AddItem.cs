using System;
using System.IO;
using System.Windows.Forms;

using QueueRunner;

namespace ProgramQueue
{
    public partial class AddItem : Form
    {
        public AddItem()
        {
            InitializeComponent();
            // Default to cancel in case the user x's out.
            DialogResult = DialogResult.Cancel;
        }

        public AddItem(ProgramQueueItem item)
        {
            InitializeComponent();
            // Default to cancel in case the user x's out.
            DialogResult = DialogResult.Cancel;

            executableTextbox.Text = item.Executable;
            argumentsTextbox.Text = item.Arguments;
            timeoutTextbox.Value = (decimal)item.Timeout.TotalSeconds;
            addButton.Text = "Modify";
        }

        private void openExecutableButton_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            dialog.Filter = "Executables (*.exe)|*.exe|Batch files (*.bat;*.cmd)|*.bat;*.cmd|All files (*.*)|*.*";
            dialog.DefaultExt = ".exe";
            dialog.Multiselect = false;
            dialog.Title = "Select Executable to run";
            dialog.RestoreDirectory = true;
            var result = dialog.ShowDialog();

            if(result != DialogResult.OK)
            {
                return;
            }

            executableTextbox.Text = dialog.FileName;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (!ValidateAddItemDialog())
            {
                MessageBox.Show("There are some boxes that don't have valid values.\nPlease correct them before continuing.", "Can't add item");
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool ValidateAddItemDialog()
        {
            if (string.IsNullOrWhiteSpace(executableTextbox.Text) ||
                !File.Exists(executableTextbox.Text))
            {
                return false;
            }

            return true;
        }
    }
}
