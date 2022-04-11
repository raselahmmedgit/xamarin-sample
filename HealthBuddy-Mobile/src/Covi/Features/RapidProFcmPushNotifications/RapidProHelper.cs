using System;
using System.Text;

namespace Covi.Features.RapidProFcmPushNotifications
{
    public static class RapidProHelper
    {
        public static string GetUrnFromGuid()
        {
            try
            {
                return Guid.NewGuid().ToString();
            }
            catch
            {
                return null;
            }
        }

        public static string GetUrnFromGuid(int length = 15)
        {
            try
            {
                var builder = new StringBuilder();
                while (builder.Length < length)
                {
                    builder.Append(Guid.NewGuid().ToString());
                }
                return builder.ToString(0, length);
            }
            catch
            {
                return null;
            }
        }

        public static string GetUrnFromRandom(int length = 15)
        {
            try
            {
                Random random = new Random();
                string s = "";
                for (int i = 0; i < length; i++)
                {
                    int a = random.Next(5);
                    int chr;
                    switch (a)
                    {
                        case 0:
                            chr = random.Next(0, 9);
                            s = s + chr.ToString();
                            break;
                        case 1:
                            chr = random.Next(65, 90);
                            s = s + Convert.ToChar(chr).ToString();
                            break;
                        case 2:
                            chr = random.Next(97, 122);
                            s = s + Convert.ToChar(chr).ToString();
                            break;
                        case 3:
                            chr = random.Next(125, 198);
                            s = s + Convert.ToChar(chr).ToString();
                            break;
                        case 4:
                            chr = random.Next(201, 289);
                            s = s + Convert.ToChar(chr).ToString();
                            break;
                    }
                }
                return s;
            }
            catch
            {
                return null;
            }
        }
    }
}
