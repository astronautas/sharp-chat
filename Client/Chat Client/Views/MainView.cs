using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chat_Client.Views
{
    public partial class MainView : Form
    {
        public ChatClient _client;

        public MainView()
        {
            InitializeComponent();
        }

        public MainView(ChatClient client) : this()
        {
            _client = client;

            authenticationScreen1.Client = client;
            AuthenticationView.onAuthSuccess += loadStartScreen;
        }

        private void authenticationScreen1_Load(object sender, EventArgs e)
        {
        }

        private void loadStartScreen()
        {
            Controls.Add(new StartView());
        }
    }
}
