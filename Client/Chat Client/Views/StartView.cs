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
    public partial class StartView : UserControl
    {
        private UserControl _activeScreen;

        public StartView()
        {
            InitializeComponent();

            NavigationView.onCurrentChatBtnClick += LoadCurrentChat;
            NavigationView.onUsersListBtnClick += LoadUsersList;
        }

        private void navigationView1_Load(object sender, EventArgs e)
        {
        
        }

        private void LoadUsersList()
        {

        }

        private void LoadCurrentChat()
        {

        }
    }
}
