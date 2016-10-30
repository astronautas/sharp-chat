namespace Chat_Client.Views
{
    partial class AuthenticationView
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
            this.usernameField = new System.Windows.Forms.TextBox();
            this.username = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.errors = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // usernameField
            // 
            this.usernameField.Location = new System.Drawing.Point(29, 380);
            this.usernameField.Name = "usernameField";
            this.usernameField.Size = new System.Drawing.Size(366, 26);
            this.usernameField.TabIndex = 0;
            // 
            // username
            // 
            this.username.AutoSize = true;
            this.username.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.username.Location = new System.Drawing.Point(25, 357);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(137, 20);
            this.username.TabIndex = 1;
            this.username.Text = "Enter a nickname:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(29, 430);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(366, 46);
            this.button1.TabIndex = 2;
            this.button1.Text = "Connect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // errors
            // 
            this.errors.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.errors.Location = new System.Drawing.Point(20, 15);
            this.errors.Name = "errors";
            this.errors.ReadOnly = true;
            this.errors.Size = new System.Drawing.Size(393, 19);
            this.errors.TabIndex = 3;
            this.errors.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.errors.Visible = false;
            this.errors.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // AuthenticationScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.errors);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.username);
            this.Controls.Add(this.usernameField);
            this.Name = "AuthenticationScreen";
            this.Size = new System.Drawing.Size(430, 494);
            this.Load += new System.EventHandler(this.AuthenticationScreen_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox usernameField;
        private System.Windows.Forms.Label username;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox errors;
    }
}
