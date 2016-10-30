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
    public partial class NavigationView : UserControl
    {
        public static event Action onUsersListBtnClick;
        public static event Action onCurrentChatBtnClick;

        public NavigationView()
        {
            InitializeComponent();
        }

        private void UsersListBtn_Click(object sender, EventArgs e)
        {
            onUsersListBtnClick.Invoke();
        }

        private void CurrentChatBtn_Click(object sender, EventArgs e)
        {
            onCurrentChatBtnClick.Invoke();
        }
    }
}
