using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chat_Client.Views
{
    public partial class AuthenticationView : UserControl
    {
        public static event Action onAuthSuccess;

        public ChatClient Client { get; set; }

        public AuthenticationView()
        {
            InitializeComponent();
        }

        private void AuthenticationScreen_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var username = usernameField.Text;
            Client.NickName = username;

            if (Client.ConnectToServer())
            {
                var status = Client.Authenticate();
                var test = status;

                errors.Visible = false;

                Parent.Controls.Remove(this);
                onAuthSuccess.Invoke();
            }
            else
            {
                errors.Text = "Could not connect to chat server. Try again.";
                errors.Visible = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
