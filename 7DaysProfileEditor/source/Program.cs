using SevenDaysProfileEditor.Data;
using SevenDaysProfileEditor.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SevenDaysProfileEditor {

    internal static class Program {

        /// <summary>
        /// Initializes the static data of the program
        /// </summary>
        private static bool Initialize() {
            /*try {
                AssetInfo.GenerateAssetInfoList();
                IconData.itemIconDictionary = new Dictionary<string, byte[]>();
                IconData.uiIconDictionary = new Dictionary<string, UIIconData>();
            }
            catch (Exception e) {
                ErrorHandler.HandleError("Error while processing icons. Failed to load asset files." + e.Message, e, true);
            }
            try {
                //This step normally fails. Lets suppressed the error message.
                //We get inventory icons
                IconData.PopulateIconDictionaries();
            }
            catch (Exception) {
                // ErrorHandler.HandleError("Error while processing icons. Failed to load asset files." + e.Message, e, true);
            }

            try {
                XmlData.GetBlocks();
                XmlData.GetItems();
                XmlData.ArrangeItemList();
                XmlData.GetSkills();
                AssetInfo.ClearAssetInfoList();
            }
            catch (Exception e) {
                ErrorHandler.HandleError(string.Format("Failed to load XML files!\n\n{0}\n\nProgram will now terminate!", e.Message), e, true);
                return false;
            }*/
            
            return true;
        }

        private static void Start(MainWindow window) {
            if (Initialize()) {
                window.Show();
                Application.Run(window);
            }

            else {
                Application.Exit();
            }
        }

        private static bool LocateRootDirectory() {
            string gameRootDir = Config.GetSetting("gameRootDir");
            if (gameRootDir == null || !Directory.Exists(gameRootDir)) {

                string defaultGameRootDir = @"C:\Program Files\Steam\steamapps\common\7 Days To Die";
                if (File.Exists(defaultGameRootDir)) {
                    gameRootDir = defaultGameRootDir;
                } else {
                    MessageBox.Show("In order to work, the program need to access your root game directory. Please press Ok and locate 7DaysToDie.exe in the game's root folder.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    OpenFileDialog gameRootDialog = new OpenFileDialog() {
                        Title = "Tool needs to find the 7DaysToDie.exe!",
                        Filter = "7DaysToDie.exe|7DaysToDie.exe",
                        CheckFileExists = true
                    };

                    gameRootDialog.FileOk += (sender, e) => {
                        gameRootDir = gameRootDialog.FileName.Substring(0, gameRootDialog.FileName.LastIndexOf('\\'));
                    };

                    if (gameRootDialog.ShowDialog() != DialogResult.OK) {
                        return false;
                    }
                }

                Config.SetSetting("gameRootDir", gameRootDir);
            }

            return true;
        }

        [STAThread]
        private static void Main() {
            Log.startLog();
            Config.Load();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (LocateRootDirectory()) {
                string xmlPath = Config.GetSetting("gameRootDir") + @"\Data\Config\";
                if (XmlData.Initialize(xmlPath)) {
                    MainWindow window = new MainWindow();
                    window.Show();
                    Application.Run(window);
                } else {
                    MessageBox.Show("Failed to load XML configurations, the program will now exit. Check your " + xmlPath + " folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            } else {
                MessageBox.Show("Failed to locate game root directory, the program will now exit. If this error persists, delete 7DaysProfileEditor.config file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
    }
}