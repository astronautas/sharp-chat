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
    public partial class UsersListView : UserControl
    {
        public ChatClient Client { get; set; }

        public UsersListView(ChatClient client)
        {
            InitializeComponent();

            Client = client;

            Client.OnClientListChange += (() => {
                Invoke(new MethodInvoker(LoadUsersList));
            });

            Client.UpdateClientsList();
        }

        private void LoadUsersList()
        {

            foreach (var client in Client.ClientList)
            {
                Button clientBtn = new Button();
                clientBtn.Width = 300;
                clientBtn.Text = client;
                clientsList.Controls.Add(clientBtn);
                clientsList.SetFlowBreak(clientBtn, true);

                clientBtn.Click += delegate (object sender, System.EventArgs eventObject)
                {
                    Client.SendChatRequest(client);
                };
            }
        }
    }
}
