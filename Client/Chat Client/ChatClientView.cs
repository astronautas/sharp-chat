using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chat_Client
{
    public partial class ChatClientGui : Form
    {
        private ChatClient _chatClient;

        public ChatClientGui(ChatClient chatClient)
        {
            InitializeComponent();
            _chatClient = chatClient;

            if (IsHandleCreated)
            {
                // Initialize events
                _chatClient.LogChanged += new ChangeEventHandler(RenderLog);
                RenderLog();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    var text = textBox1.Text;
        //    _chatClient.SendMessage(text);
        //}

        public void RenderLog()
        {
            Invoke(new Action(
                () =>
                {
                    // Log already has formatting
                    richTextBox1.Text = _chatClient.Log;
                }
            )
            );
        }
    }
}
