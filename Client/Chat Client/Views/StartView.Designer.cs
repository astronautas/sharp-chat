namespace Chat_Client.Views
{
    partial class StartView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.welcomeText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // welcomeText
            // 
            this.welcomeText.Location = new System.Drawing.Point(3, 482);
            this.welcomeText.Name = "welcomeText";
            this.welcomeText.ReadOnly = true;
            this.welcomeText.Size = new System.Drawing.Size(508, 26);
            this.welcomeText.TabIndex = 0;
            this.welcomeText.Text = "Welcome to the chat!";
            this.welcomeText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // StartView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.welcomeText);
            this.Name = "StartView";
            this.Size = new System.Drawing.Size(514, 530);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox welcomeText;
    }
}
