using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace Chat_Client.Views
{
    public partial class AuthenticationView : UserControl
    {
        public static event System.Action onAuthSuccess;

        public ChatClient Client { get; set; }

        public AuthenticationView()
        {
            InitializeComponent();

            if (ConfigurationManager.AppSettings["username"] != null)
            {
                usernameField.Text = ConfigurationManager.AppSettings["username"];
            }
        }

        private void AuthenticationScreen_Load(object sender, EventArgs e)
        {
            Client.onAuthFailure += (() => {
                Invoke(new MethodInvoker(() => {
                    errors.Text = "Auth failure";
                    errors.Visible = true;
                }));
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var username = usernameField.Text;
            Client.NickName = username;
            Client.Password = passwordField.Text;

            ConfigurationManager.AppSettings["username"] = username;

            if (Client.Connection.Connected || Client.ConnectToServer())
            {
                var status = Client.Authenticate();
                var test = status;

                errors.Visible = false;
            }
            else
            {
                errors.Text = "Try again";
                errors.Visible = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void username_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
