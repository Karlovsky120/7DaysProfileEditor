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
        private static void Initialize() {
            try {
                AssetInfo.GenerateAssetInfoList();
                IconData.itemIconDictionary = new Dictionary<string, byte[]>();
                IconData.uiIconDictionary = new Dictionary<string, UIIconData>();
                IconData.PopulateIconDictionaries();
            }
            catch (Exception e) {
                Log.WriteException(e);
                MessageBox.Show("No icons will be loaded. Failed to load asset files." + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            try {
                XmlData.GetBlocks();
                XmlData.GetItems();
                XmlData.ArrangeItemList();
                XmlData.GetSkills();
            }
            catch (Exception e) {
                Log.WriteException(e);

                MessageBox.Show(string.Format("Failed to load XML files! {0} Program will now terminate!", e.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

        [STAThread]
        private static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainWindow window = new MainWindow();
            Config.Load();

            string gameRoot = Config.GetSetting("gameRoot");

            // If no game root is specified in the config, we check if we are inside the game root. If not, we ask the user.
            if (gameRoot == null) {
                if (File.Exists(Environment.CurrentDirectory + "7DaysToDie.exe")) {
                    Initialize();
                    window.Show();
                }
                else {
                    OpenFileDialog gameRootDialog = new OpenFileDialog() {
                        Title = "Tool needs to find the 7DaysToDie.exe!",
                        Filter = "7DaysToDie.exe|7DaysToDie.exe"
                    };

                    gameRootDialog.FileOk += (sender1, e1) => {
                        gameRoot = gameRootDialog.FileName.Substring(0, gameRootDialog.FileName.LastIndexOf('\\'));
                        Config.SetSetting("gameRoot", gameRoot);
                        Initialize();
                        window.Show();
                    };

                    if (gameRootDialog.ShowDialog() != DialogResult.OK) {
                        Application.Exit();
                        return;
                    }
                }
            }
            else {
                Initialize();
                window.Show();
            }

            Application.Run(window);
        }
    }
}