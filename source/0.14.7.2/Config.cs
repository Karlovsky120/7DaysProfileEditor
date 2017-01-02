using System.Collections.Generic;
using System.IO;

namespace SevenDaysProfileEditor
{
    internal class Config
    {
        public static Dictionary<string, string> config = new Dictionary<string, string>();

        public static void Load()
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

        private static void Save()
        {
            StreamWriter writer = new StreamWriter("7DaysProfileEditor.config");

            foreach (KeyValuePair<string, string> setting in config)
            {
                writer.WriteLine(setting.Key + "=" + setting.Value);
            }

            writer.Close();
        }

        public static string GetSetting(string key)
        {
            string value = null;

            config.TryGetValue(key, out value);

            return value;
        }

        public static void SetSetting(string key, string value)
        {
            config.Remove(key);
            config.Add(key, value);
            Save();
        }
    }
}