namespace ProgramQueue
{
    partial class AddItem
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.executableTextbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.addButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.openExecutableButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.argumentsTextbox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.timeoutTextbox = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutTextbox)).BeginInit();
            this.SuspendLayout();
            // 
            // executableTextbox
            // 
            this.executableTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.executableTextbox.Location = new System.Drawing.Point(81, 12);
            this.executableTextbox.Name = "executableTextbox";
            this.executableTextbox.Size = new System.Drawing.Size(436, 20);
            this.executableTextbox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Executable:";
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Location = new System.Drawing.Point(395, 71);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 4;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(476, 71);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // openExecutableButton
            // 
            this.openExecutableButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.openExecutableButton.Location = new System.Drawing.Point(523, 11);
            this.openExecutableButton.Name = "openExecutableButton";
            this.openExecutableButton.Size = new System.Drawing.Size(28, 22);
            this.openExecutableButton.TabIndex = 1;
            this.openExecutableButton.Text = "...";
            this.openExecutableButton.UseVisualStyleBackColor = true;
            this.openExecutableButton.Click += new System.EventHandler(this.openExecutableButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Arguments:";
            // 
            // argumentsTextbox
            // 
            this.argumentsTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.argumentsTextbox.Location = new System.Drawing.Point(81, 38);
            this.argumentsTextbox.Name = "argumentsTextbox";
            this.argumentsTextbox.Size = new System.Drawing.Size(470, 20);
            this.argumentsTextbox.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Timeout:";
            // 
            // timeoutTextbox
            // 
            this.timeoutTextbox.DecimalPlaces = 2;
            this.timeoutTextbox.Location = new System.Drawing.Point(81, 64);
            this.timeoutTextbox.Maximum = new decimal(new int[] {
            200000,
            0,
            0,
            0});
            this.timeoutTextbox.Name = "timeoutTextbox";
            this.timeoutTextbox.Size = new System.Drawing.Size(120, 20);
            this.timeoutTextbox.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(207, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "seconds";
            // 
            // AddItem
            // 
            this.AcceptButton = this.addButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(563, 106);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.timeoutTextbox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.argumentsTextbox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.openExecutableButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.executableTextbox);
            this.MinimumSize = new System.Drawing.Size(348, 145);
            this.Name = "AddItem";
            this.Text = "Add Task";
            ((System.ComponentModel.ISupportInitialize)(this.timeoutTextbox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button openExecutableButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        internal System.Windows.Forms.TextBox executableTextbox;
        internal System.Windows.Forms.TextBox argumentsTextbox;
        internal System.Windows.Forms.NumericUpDown timeoutTextbox;
    }
}