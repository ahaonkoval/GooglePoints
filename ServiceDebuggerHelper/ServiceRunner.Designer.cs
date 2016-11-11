namespace ServiceDebuggerHelper
{
    partial class ServiceRunner
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
            this.startButton = new System.Windows.Forms.Button();
            this.pauseButton = new System.Windows.Forms.Button();
            this.continueButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtConsole = new System.Windows.Forms.TextBox();
            this.lblConsole = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(14, 13);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(87, 25);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "&Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // pauseButton
            // 
            this.pauseButton.Location = new System.Drawing.Point(108, 13);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(87, 25);
            this.pauseButton.TabIndex = 0;
            this.pauseButton.Text = "&Pause";
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // continueButton
            // 
            this.continueButton.Location = new System.Drawing.Point(203, 13);
            this.continueButton.Name = "continueButton";
            this.continueButton.Size = new System.Drawing.Size(87, 25);
            this.continueButton.TabIndex = 0;
            this.continueButton.Text = "&Continue";
            this.continueButton.UseVisualStyleBackColor = true;
            this.continueButton.Click += new System.EventHandler(this.continueButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(297, 13);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(87, 25);
            this.stopButton.TabIndex = 0;
            this.stopButton.Text = "S&top";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 386);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(399, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(51, 17);
            this.toolStripStatusLabel1.Text = "Stopped";
            // 
            // txtConsole
            // 
            this.txtConsole.Location = new System.Drawing.Point(14, 62);
            this.txtConsole.Multiline = true;
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.Size = new System.Drawing.Size(370, 319);
            this.txtConsole.TabIndex = 2;
            // 
            // lblConsole
            // 
            this.lblConsole.AutoSize = true;
            this.lblConsole.Location = new System.Drawing.Point(15, 45);
            this.lblConsole.Name = "lblConsole";
            this.lblConsole.Size = new System.Drawing.Size(44, 14);
            this.lblConsole.TabIndex = 3;
            this.lblConsole.Text = "Вывод";
            // 
            // ServiceRunner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 408);
            this.Controls.Add(this.lblConsole);
            this.Controls.Add(this.txtConsole);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.continueButton);
            this.Controls.Add(this.pauseButton);
            this.Controls.Add(this.startButton);
            this.Font = new System.Drawing.Font("Liberation Sans", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "ServiceRunner";
            this.Text = "ServiceControler";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.Button continueButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Label lblConsole;
        public System.Windows.Forms.TextBox txtConsole;
    }
}