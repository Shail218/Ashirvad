using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Common
{
    public static class Common
    {
        public static string GetStringConfigKey(string keyName)
        {
            return Convert.ToString(ConfigurationManager.AppSettings[keyName]);
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public static int GetIntConfigKey(string keyName)
        {
            string configVal = Convert.ToString(ConfigurationManager.AppSettings[keyName]);
            long val = 0;
            if (long.TryParse(configVal, out val))
            {
                return (int)val;
            }

            return 0;
        }

        public static void SaveFile(byte[] content, string fileName, string path)
        {
            //string directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            //File.WriteAllBytes(Common.GetStringConfigKey("DocDirectory") + path + fileName, content);

            string directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (!Directory.Exists(Common.GetStringConfigKey("DocDirectory") + path))
            {
                Directory.CreateDirectory(Common.GetStringConfigKey("DocDirectory") + path);
            }
            File.WriteAllBytes(Common.GetStringConfigKey("DocDirectory") + path + fileName, content);
        }

        public static Dictionary<string, int> GetRoles()
        {
            List<Enums.Roles> roles = Enum.GetValues(typeof(Enums.Roles)).Cast<Enums.Roles>().ToList();
            Dictionary<string, int> data = new Dictionary<string, int>();
            foreach (var item in roles)
            {
                var enumData = (Enums.Roles)item;
                data.Add(enumData.ToString(), (int)item);
            }

            return data;
        }

        public static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
