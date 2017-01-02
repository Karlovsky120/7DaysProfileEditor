using SevenDaysProfileEditor.GUI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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

            WindowMain window = new WindowMain();
            Config.load();

            string gameRoot = Config.getSetting("gameRoot");

            if (gameRoot == null)
            {
                if (File.Exists(Environment.CurrentDirectory + "7DaysToDie.exe"))
                {
                    Xml.initialize(Environment.CurrentDirectory);
                    window.Show();
                }

                else
                {
                    OpenFileDialog gameRootDialog = new OpenFileDialog();
                    gameRootDialog.Title = "Tool needs to find the 7DaysToDie.exe!";

                    gameRootDialog.FileOk += (sender1, e1) =>
                    {
                        gameRoot = gameRootDialog.FileName.Substring(0, gameRootDialog.FileName.LastIndexOf('\\'));
                        Config.setSetting("gameRoot", gameRoot);
                        Xml.initialize(gameRoot);
                        window.Show();
                    };

                    gameRootDialog.ShowDialog();
                }
            }

            else
            {
                Xml.initialize(gameRoot);
                window.Show();
            }

            Application.Run(window);
        }
    }
}
