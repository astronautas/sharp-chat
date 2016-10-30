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
            this.navigationView1 = new Chat_Client.Views.NavigationView();
            this.p2PChatView1 = new Chat_Client.Views.P2PChatView();
            this.SuspendLayout();
            // 
            // navigationView1
            // 
            this.navigationView1.Location = new System.Drawing.Point(88, 3);
            this.navigationView1.Name = "navigationView1";
            this.navigationView1.Size = new System.Drawing.Size(335, 69);
            this.navigationView1.TabIndex = 0;
            this.navigationView1.Load += new System.EventHandler(this.navigationView1_Load);
            // 
            // p2PChatView1
            // 
            this.p2PChatView1.Location = new System.Drawing.Point(112, 48);
            this.p2PChatView1.Name = "p2PChatView1";
            this.p2PChatView1.Size = new System.Drawing.Size(8, 8);
            this.p2PChatView1.TabIndex = 1;
            // 
            // StartView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.p2PChatView1);
            this.Controls.Add(this.navigationView1);
            this.Name = "StartView";
            this.Size = new System.Drawing.Size(514, 530);
            this.ResumeLayout(false);

        }

        #endregion

        private NavigationView navigationView1;
        private P2PChatView p2PChatView1;
    }
}
