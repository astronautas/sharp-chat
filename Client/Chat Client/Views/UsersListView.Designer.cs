namespace Chat_Client.Views
{
    partial class UsersListView
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
            this.clientsList = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // clientsList
            // 
            this.clientsList.AutoSize = true;
            this.clientsList.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.clientsList.Location = new System.Drawing.Point(4, 4);
            this.clientsList.Name = "clientsList";
            this.clientsList.Size = new System.Drawing.Size(0, 0);
            this.clientsList.TabIndex = 0;
            // 
            // UsersListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.clientsList);
            this.Name = "UsersListView";
            this.Size = new System.Drawing.Size(7, 7);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel clientsList;
    }
}
