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

            Config.load();
            
            string gameRoot = Config.getSetting("gameRoot");

            if (gameRoot == null)
            {
                if (File.Exists(Environment.CurrentDirectory + "7DaysToDie.exe"))
                {
                    gameRoot = Environment.CurrentDirectory;
                }

                else
                {
                    OpenFileDialog gameRootDialog = new OpenFileDialog();
                    gameRootDialog.Title = "Tool needs to find the game exe!";
                    
                    gameRootDialog.FileOk += (sender1, e1) =>
                    {
                        gameRoot = gameRootDialog.FileName.Substring(0, gameRootDialog.FileName.LastIndexOf('\\'));
                        Config.setSetting("gameRoot", gameRoot);
                    };

                    gameRootDialog.ShowDialog();
                }              
            }          

            Xml.initialize(gameRoot);
            
            WindowMain window = new WindowMain();

            Application.Run(window);
        }
    }
}
