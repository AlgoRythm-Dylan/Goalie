using System;

namespace Goalie.Lib.Data
{
    public class ServiceBase
    {
        const string ID_CHARS = "abcdefghijklmnopqrstuvqxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public static string NewID(int length = 12)
        {
            Random random = new Random();
            string ID = "";
            for (int i = 0; i < length; i++)
                ID += ID_CHARS[random.Next(0, ID_CHARS.Length)];
            return ID;
        }
    }
}
