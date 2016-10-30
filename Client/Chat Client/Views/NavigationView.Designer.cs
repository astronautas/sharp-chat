namespace Chat_Client.Views
{
    partial class NavigationView
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
            this.CurrentChatBtn = new System.Windows.Forms.Button();
            this.UsersListBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CurrentChatBtn
            // 
            this.CurrentChatBtn.Location = new System.Drawing.Point(4, 4);
            this.CurrentChatBtn.Name = "CurrentChatBtn";
            this.CurrentChatBtn.Size = new System.Drawing.Size(159, 57);
            this.CurrentChatBtn.TabIndex = 0;
            this.CurrentChatBtn.Text = "Current Chat";
            this.CurrentChatBtn.UseVisualStyleBackColor = true;
            this.CurrentChatBtn.Click += new System.EventHandler(this.UsersListBtn_Click);
            // 
            // UsersListBtn
            // 
            this.UsersListBtn.Location = new System.Drawing.Point(169, 4);
            this.UsersListBtn.Name = "UsersListBtn";
            this.UsersListBtn.Size = new System.Drawing.Size(159, 57);
            this.UsersListBtn.TabIndex = 1;
            this.UsersListBtn.Text = "Users";
            this.UsersListBtn.UseVisualStyleBackColor = true;
            this.UsersListBtn.Click += new System.EventHandler(this.CurrentChatBtn_Click);
            // 
            // NavigationView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.UsersListBtn);
            this.Controls.Add(this.CurrentChatBtn);
            this.Name = "NavigationView";
            this.Size = new System.Drawing.Size(335, 69);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CurrentChatBtn;
        private System.Windows.Forms.Button UsersListBtn;
    }
}
