using System;
using System.Configuration;
using System.Windows.Forms;

namespace Chat_Client.Views
{
    public partial class MainView : Form
    {
        public ChatClient _client;

        private UserControl _currentScreen;

        public MainView()
        {
            InitializeComponent();
        }

        public MainView(ChatClient client) : this()
        {
            _client = client;

            LoadAuthScreen();
        }

        private void authenticationScreen1_Load(object sender, EventArgs e)
        {
        }

        private void LoadAuthScreen()
        {
            this.Controls.Remove(_currentScreen);

            var authScreen = new AuthenticationView();
            mainFlow.Controls.Add(authScreen);
            _currentScreen = authScreen;

            authScreen.Client = _client;

            _client.onAuthSuccess += (() => {
                Invoke(new MethodInvoker(AfterAuthLoad));
            });
        }

        private void AfterAuthLoad()
        {
            mainFlow.Controls.Remove(_currentScreen);

            var navView = new NavigationView();
            mainFlow.Controls.Add(navView);

            NavigationView.onCurrentChatBtnClick += LoadCurrentChatScreen;
            NavigationView.onUsersListBtnClick += LoadUsersListScreen;

            _client.OnInitiateChatWithUser += (() => {
                Invoke(new MethodInvoker(LoadCurrentChatScreen));
            });

            LoadStartScreen();
        }

        private void LoadStartScreen()
        {
            mainFlow.Controls.Remove(_currentScreen);

            var startScreen = new StartView();
            mainFlow.Controls.Add(startScreen);
            mainFlow.SetFlowBreak(startScreen, true);
            _currentScreen = startScreen;
        }

        private void LoadCurrentChatScreen()
        {
            mainFlow.Controls.Remove(_currentScreen);

            var currChatScreen = new CurrentChatView();
            currChatScreen.Client = _client;
            currChatScreen.Render();
            mainFlow.Controls.Add(currChatScreen);
            _currentScreen = currChatScreen;
        }

        private void LoadUsersListScreen()
        {
            mainFlow.Controls.Remove(_currentScreen);

            var usersListScreen = new UsersListView(client: _client);
            mainFlow.Controls.Add(usersListScreen);
            _currentScreen = usersListScreen;
        }
    }
}
