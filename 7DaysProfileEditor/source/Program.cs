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


            try {
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
            catch (Exception e) {
                ErrorHandler.HandleError("Error while processing icons. Failed to load asset files." + e.Message, e, false);
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
            }

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

        [STAThread]
        private static void Main() {
            Log.startLog();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Updater.DoUpdate();

            MainWindow window = new MainWindow();
            Config.Load();

            string gameRoot = Config.GetSetting("gameRoot");

            // If they gave a gameroot, launch app.
            if (gameRoot != null) {
                Start(window);
                return;
            }

            //use default location?
            string defaultlocation = @"C:\Program Files (x86)\Steam\steamapps\common\7 Days To Die\7DaysToDie.exe";
            if (File.Exists(defaultlocation)) {
                gameRoot = defaultlocation.Substring(0, defaultlocation.LastIndexOf('\\'));
                Config.SetSetting("gameRoot", gameRoot);
                Start(window);
                return;
            }

            //Ask user were is game
            OpenFileDialog gameRootDialog = new OpenFileDialog() {
                Title = "Tool needs to find the 7DaysToDie.exe!",
                Filter = "7DaysToDie.exe|7DaysToDie.exe",
                CheckFileExists = true
            };

            gameRootDialog.FileOk += (sender1, e1) => {
                gameRoot = gameRootDialog.FileName.Substring(0, gameRootDialog.FileName.LastIndexOf('\\'));
                Config.SetSetting("gameRoot", gameRoot);
            };

            if (gameRootDialog.ShowDialog() != DialogResult.OK) {
                Application.Exit();
                return;
            }

            Start(window);
        }

    }
}