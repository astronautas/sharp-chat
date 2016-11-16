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
    public partial class CurrentChatView : UserControl
    {
        public ChatClient Client { get; set; }

        private RichTextBox chatLog;
        private TextBox chatInput;
        private Button submitBtn;

        public CurrentChatView()
        {
            InitializeComponent();
        }

        public void Render()
        {
            if (Client.CurrentChatPal != null)
            {
                chatLog   = new RichTextBox();
                chatLog.MinimumSize = new Size(300, 200);
                chatInput = new TextBox();
                submitBtn = new Button();
                submitBtn.Text = "Send message";

                var flow = new FlowLayoutPanel();
                flow.FlowDirection = FlowDirection.TopDown;
                flow.WrapContents = false;
                flow.AutoSize = true;

                Controls.Add(flow);

                flow.Controls.Add(chatLog);
                flow.Controls.Add(chatInput);
                flow.Controls.Add(submitBtn);

                Client.OnReceiveMessageFromUser += (() => {
                    Invoke(new MethodInvoker(UpdateLog));
                });

                submitBtn.Click += delegate (System.Object o, System.EventArgs e)
                {
                    new Task(() =>
                    {
                        var msg = chatInput.Text;
                        Client.SendMsgToUser(Client.CurrentChatPal, msg);
                    }).Start();
                };

                UpdateLog();
            } else
            {
                var textBox = new TextBox();
                textBox.Width = 300;
                textBox.Text = "You are not chatting with anyone now";

                Controls.Add(textBox);
            }
        }

        // Render callbacks
        private void UpdateLog()
        {
            chatLog.Text = "";

            var lastMessages = Client.CurrChatMsgHistory.Skip(
                Math.Max(0, Client.CurrChatMsgHistory.Count() - 10));

            foreach (var msg in lastMessages)
            {
                chatLog.Text += msg + Environment.NewLine;
            }
        }
    }
}
