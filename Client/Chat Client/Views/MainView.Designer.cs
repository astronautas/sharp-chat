namespace Chat_Client.Views
{
    partial class MainView
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
            this.mainFlow = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // mainFlow
            // 
            this.mainFlow.AutoSize = true;
            this.mainFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mainFlow.Location = new System.Drawing.Point(13, 13);
            this.mainFlow.Name = "mainFlow";
            this.mainFlow.Size = new System.Drawing.Size(0, 0);
            this.mainFlow.TabIndex = 0;
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(603, 560);
            this.Controls.Add(this.mainFlow);
            this.Name = "MainView";
            this.Text = "Client Chat App";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel mainFlow;
    }
}