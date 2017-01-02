using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysProfileEditor
{
    class Config
    {
        public static Dictionary<string, string> config = new Dictionary<string, string>();

        public static void load()
        {
            if (File.Exists("7DaysProfileEditor.config"))
            {
                StreamReader reader = new StreamReader("7DaysProfileEditor.config");

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    string key = line.Substring(0, line.LastIndexOf('='));
                    string value = line.Substring(line.LastIndexOf('=') + 1);

                    config.Add(key, value);
                }

                reader.Close();
            }
        }

        private static void save()
        {
            StreamWriter writer = new StreamWriter("7DaysProfileEditor.config");

            foreach (KeyValuePair<string, string> setting in config)
            {
                writer.WriteLine(setting.Key + "=" + setting.Value);
            }

            writer.Close();
        }

        public static string getSetting(string key)
        {
            string value = null;

            config.TryGetValue(key, out value);

            return value;
        }

        public static void setSetting(string key, string value)
        {
            config.Remove(key);
            config.Add(key, value);
            save();
        }
    }
}
