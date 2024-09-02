namespace AppStarter
{
    partial class MainForm
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
            this.pickAppButton = new System.Windows.Forms.Button();
            this.appPathTextBox = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.appArgsTextBox = new System.Windows.Forms.TextBox();
            this.argumentsLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pickAppButton
            // 
            this.pickAppButton.Location = new System.Drawing.Point(392, 33);
            this.pickAppButton.Name = "pickAppButton";
            this.pickAppButton.Size = new System.Drawing.Size(56, 23);
            this.pickAppButton.TabIndex = 0;
            this.pickAppButton.Text = "Browse";
            this.pickAppButton.UseVisualStyleBackColor = true;
            this.pickAppButton.Click += new System.EventHandler(this.pickAppButton_Click);
            // 
            // appPathTextBox
            // 
            this.appPathTextBox.Location = new System.Drawing.Point(13, 33);
            this.appPathTextBox.Name = "appPathTextBox";
            this.appPathTextBox.Size = new System.Drawing.Size(359, 20);
            this.appPathTextBox.TabIndex = 1;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(156, 112);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 2;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // appArgsTextBox
            // 
            this.appArgsTextBox.Location = new System.Drawing.Point(12, 71);
            this.appArgsTextBox.Name = "appArgsTextBox";
            this.appArgsTextBox.Size = new System.Drawing.Size(359, 20);
            this.appArgsTextBox.TabIndex = 3;
            // 
            // argumentsLabel
            // 
            this.argumentsLabel.AutoSize = true;
            this.argumentsLabel.Location = new System.Drawing.Point(391, 74);
            this.argumentsLabel.Name = "argumentsLabel";
            this.argumentsLabel.Size = new System.Drawing.Size(57, 13);
            this.argumentsLabel.TabIndex = 4;
            this.argumentsLabel.Text = "Arguments";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 152);
            this.Controls.Add(this.argumentsLabel);
            this.Controls.Add(this.appArgsTextBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.appPathTextBox);
            this.Controls.Add(this.pickAppButton);
            this.Name = "MainForm";
            this.Text = "App Starter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button pickAppButton;
        private System.Windows.Forms.TextBox appPathTextBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.TextBox appArgsTextBox;
        private System.Windows.Forms.Label argumentsLabel;
    }
}

