using SevenDaysProfileEditor.GUI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SevenDaysProfileEditor
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainWindow window = new MainWindow();
            Config.Load();

            string gameRoot = Config.GetSetting("gameRoot");

            if (gameRoot == null)
            {
                if (File.Exists(Environment.CurrentDirectory + "7DaysToDie.exe"))
                {
                    Initialize(Environment.CurrentDirectory);
                    window.Show();
                }

                else
                {
                    OpenFileDialog gameRootDialog = new OpenFileDialog();
                    gameRootDialog.Title = "Tool needs to find the 7DaysToDie.exe!";

                    gameRootDialog.FileOk += (sender1, e1) =>
                    {
                        gameRoot = gameRootDialog.FileName.Substring(0, gameRootDialog.FileName.LastIndexOf('\\'));
                        Config.SetSetting("gameRoot", gameRoot);
                        Initialize(gameRoot);
                        window.Show();
                    };

                    gameRootDialog.ShowDialog();
                }
            }

            else
            {
                Initialize(gameRoot);
                window.Show();
            }

            Application.Run(window);
        }

        private static void Initialize(string path)
        {
            try
            {
                AssetInfo.GenerateAssetDictionary();
                IconData.CreateIconDictionarys();
            }

            catch (Exception e)
            {
                Log.WriteError(e);
                MessageBox.Show("No icons will be loaded. Failed to load asset files." + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                XmlData.GetBlocks();
                XmlData.GetItems();
                XmlData.ArrangeItemList();
                XmlData.GetSkills();
                XmlData.GetQuests();
            }

            catch (Exception e)
            {
                Log.WriteError(e);
                MessageBox.Show("Failed to load XML files. " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

/*        private static void isSame()
        {
            System.Console.WriteLine(Unreadable(true, true, true, true, true) == Readable(true, true, true, true, true));
            System.Console.WriteLine(Unreadable(true, true, true, true, false) == Readable(true, true, true, true, false));
            System.Console.WriteLine(Unreadable(true, true, true, false, true) == Readable(true, true, true, false, true));
            System.Console.WriteLine(Unreadable(true, true, true, false, false) == Readable(true, true, true, false, false));

            System.Console.WriteLine(Unreadable(true, true, false, true, true) == Readable(true, true, false, true, true));
            System.Console.WriteLine(Unreadable(true, true, false, true, false) == Readable(true, true, false, true, false));
            System.Console.WriteLine(Unreadable(true, true, false, false, true) == Readable(true, true, false, false, true));
            System.Console.WriteLine(Unreadable(true, true, false, false, false) == Readable(true, true, false, false, false));

            System.Console.WriteLine(Unreadable(true, false, true, true, true) == Readable(true, false, true, true, true));
            System.Console.WriteLine(Unreadable(true, false, true, true, false) == Readable(true, false, true, true, false));
            System.Console.WriteLine(Unreadable(true, false, true, false, true) == Readable(true, false, true, false, true));
            System.Console.WriteLine(Unreadable(true, false, true, false, false) == Readable(true, false, true, false, false));

            System.Console.WriteLine(Unreadable(true, false, false, true, true) == Readable(true, false, false, true, true));
            System.Console.WriteLine(Unreadable(true, false, false, true, false) == Readable(true, false, false, true, false));
            System.Console.WriteLine(Unreadable(true, false, false, false, true) == Readable(true, false, false, false, true));
            System.Console.WriteLine(Unreadable(true, false, false, false, false) == Readable(true, false, false, false, false));

            System.Console.WriteLine(Unreadable(false, true, true, true, true) == Readable(false, true, true, true, true));
            System.Console.WriteLine(Unreadable(false, true, true, true, false) == Readable(false, true, true, true, false));
            System.Console.WriteLine(Unreadable(false, true, true, false, true) == Readable(false, true, true, false, true));
            System.Console.WriteLine(Unreadable(false, true, true, false, false) == Readable(false, true, true, false, false));

            System.Console.WriteLine(Unreadable(false, true, false, true, true) == Readable(false, true, false, true, true));
            System.Console.WriteLine(Unreadable(false, true, false, true, false) == Readable(false, true, false, true, false));
            System.Console.WriteLine(Unreadable(false, true, false, false, true) == Readable(false, true, false, false, true));
            System.Console.WriteLine(Unreadable(false, true, false, false, false) == Readable(false, true, false, false, false));

            System.Console.WriteLine(Unreadable(false, false, true, true, true) == Readable(false, false, true, true, true));
            System.Console.WriteLine(Unreadable(false, false, true, true, false) == Readable(false, false, true, true, false));
            System.Console.WriteLine(Unreadable(false, false, true, false, true) == Readable(false, false, true, false, true));
            System.Console.WriteLine(Unreadable(false, false, true, false, false) == Readable(false, false, true, false, false));

            System.Console.WriteLine(Unreadable(false, false, false, true, true) == Readable(false, false, false, true, true));
            System.Console.WriteLine(Unreadable(false, false, false, true, false) == Readable(false, false, false, true, false));
            System.Console.WriteLine(Unreadable(false, false, false, false, true) == Readable(false, false, false, false, true));
            System.Console.WriteLine(Unreadable(false, false, false, false, false) == Readable(false, false, false, false, false));
        }

        private static bool Readable(bool one, bool two, bool three, bool four, bool five)
        {
            bool flag;
            if (one)
            {
                flag = (two);
            }

            else
            {
                flag = false;
            }

            bool flag2;
            if (three)
            {
                flag2 = !(four);
            }

            else
            {
                flag2 = false;
            }

            if (!flag)
            {
                if (!one)
                {
                    if (flag2)
                    {
                        return five;
                    }
                }

                return false;
            }

            else
            {
                return true;
            }
        }

        private static bool Unreadable(bool one, bool two, bool three, bool four, bool five)
        {

            bool flag;
            if (one)
            {
                flag = (two);
            }

            else
            {
                flag = false;
            }

            bool flag2;
            if (three)
            {
                flag2 = !(four);
            }

            else
            {
                flag2 = false;
            }

            if (!flag)
            {
                if (!one)
                {
                    if (flag2)
                    {
                        return five;
                    }
                }

                return false;
            }

            else
            {
                return true;
            }
        }*/
    }
}