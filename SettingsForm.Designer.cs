namespace qConnectionSitter
{
    partial class SettingsForm
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
            this.chkStartMinimized = new System.Windows.Forms.CheckBox();
            this.chkRunOnStartup = new System.Windows.Forms.CheckBox();
            this.chkEnableOnStartup = new System.Windows.Forms.CheckBox();
            this.BtnOK = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chkStartMinimized
            // 
            this.chkStartMinimized.AutoSize = true;
            this.chkStartMinimized.Location = new System.Drawing.Point(90, 67);
            this.chkStartMinimized.Name = "chkStartMinimized";
            this.chkStartMinimized.Size = new System.Drawing.Size(97, 17);
            this.chkStartMinimized.TabIndex = 0;
            this.chkStartMinimized.Text = "Start Minimized";
            this.chkStartMinimized.UseVisualStyleBackColor = true;
            // 
            // chkRunOnStartup
            // 
            this.chkRunOnStartup.AutoSize = true;
            this.chkRunOnStartup.Location = new System.Drawing.Point(90, 91);
            this.chkRunOnStartup.Name = "chkRunOnStartup";
            this.chkRunOnStartup.Size = new System.Drawing.Size(145, 17);
            this.chkRunOnStartup.TabIndex = 1;
            this.chkRunOnStartup.Text = "Run on Windows Startup";
            this.chkRunOnStartup.UseVisualStyleBackColor = true;
            // 
            // chkEnableOnStartup
            // 
            this.chkEnableOnStartup.AutoSize = true;
            this.chkEnableOnStartup.Location = new System.Drawing.Point(90, 115);
            this.chkEnableOnStartup.Name = "chkEnableOnStartup";
            this.chkEnableOnStartup.Size = new System.Drawing.Size(176, 17);
            this.chkEnableOnStartup.TabIndex = 2;
            this.chkEnableOnStartup.Text = "Start Monitoring on App Launch";
            this.chkEnableOnStartup.UseVisualStyleBackColor = true;
            // 
            // BtnOK
            // 
            this.BtnOK.Location = new System.Drawing.Point(235, 186);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(75, 23);
            this.BtnOK.TabIndex = 3;
            this.BtnOK.Text = "OK";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(90, 186);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 4;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 271);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOK);
            this.Controls.Add(this.chkEnableOnStartup);
            this.Controls.Add(this.chkRunOnStartup);
            this.Controls.Add(this.chkStartMinimized);
            this.Name = "SettingsForm";
            this.Text = "SettingsForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkStartMinimized;
        private System.Windows.Forms.CheckBox chkRunOnStartup;
        private System.Windows.Forms.CheckBox chkEnableOnStartup;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnOK;
    }
}