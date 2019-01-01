using SevenDaysProfileEditor.Data;
using SevenDaysSaveManipulator;
using SevenDaysSaveManipulator.SaveData;
using System;
using System.IO;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI {

    internal class CentralController {

        private MainMenuStrip mainMenu;
        private PlayerTabControl tabControl;
        private BottomStatusBar statusBar;

        internal CentralController() {}

        internal void Initialize(MainMenuStrip mainMenu, PlayerTabControl tabControl, BottomStatusBar statusBar) {
            this.mainMenu = mainMenu;
            this.tabControl = tabControl;
            this.statusBar = statusBar;
        }

        internal void UpdateMenus(int tabCount) {
            mainMenu.UpdateMenus(tabCount);
        }

        internal void MenuOpen(object sender, EventArgs e) {
            OpenFileDialog openFile = new OpenFileDialog() {
                Filter = "7 Days to Die save file(*.ttp)|*.ttp|All Files(*.*)|*.*",
                Multiselect = true,
                CheckFileExists = true
            };

            if (openFile.ShowDialog() == DialogResult.OK) {
                foreach (string filePath in openFile.FileNames) {
                    OpenSingleFile(filePath, 0);
                }
            }
        }

        internal void MenuClose(object sender, EventArgs e) {
            Close(tabControl.GetSelectedTab());
        }

        internal void MenuCloseAll(object sender, EventArgs e) {
            CloseAll();
        }

        internal void MenuReload(object sender, EventArgs e) {
            PlayerTab tab = tabControl.GetSelectedTab();
            statusBar.SetText("Reloading " + tab.fileName + " ...");

            string path = tab.path;
            int selectedIndex = tab.ttpFileTabControl.SelectedIndex;

            tabControl.TabPages.Remove(tab);
            OpenSingleFile(path, selectedIndex);
        }

        internal void MenuSaveAs(object sender, EventArgs e) {
            SaveAs(tabControl.GetSelectedTab());
        }

        internal void MenuSave(object sender, EventArgs e) {
            PlayerTab tab = tabControl.GetSelectedTab();
            Save(tab, tab.path);
        }

        internal void MenuAbout(object sender, EventArgs e) {
            new AboutWindow();
        }

        internal void MenuForumPage(object sender, EventArgs e) {
            System.Diagnostics.Process.Start("https://7daystodie.com/forums/showthread.php?39919-7DaysProfileEditor");
        }

        internal void MenuGithubPage(object sender, EventArgs e) {
            System.Diagnostics.Process.Start("https://github.com/Karlovsky120/7DaysProfileEditor");
        }

        internal void MenuExit(object sender, EventArgs e) {
            if (CloseAll()) {
                Application.Exit();
            }
        }

        /// <summary>
        /// Opens a ttp file and adds it to the main tab control.
        /// </summary>
        /// <param name="filePath">Path to the file</param>
        /// <param name="selectedIndex">Subtab of the file to open after adding it to the main tab control</param>
        /// <returns>True if the tab was successfully loaded, false otherwise</returns>
        private bool OpenSingleFile(string filePath, int selectedIndex) {
            string worldRoot = Path.GetFullPath(Path.Combine(filePath, @"..\..\").ToString());
            string itemMappingsPath = worldRoot + @"\itemmappings.nim";
            string blockMappingsPath = worldRoot + @"\blockmappings.nim";

            if (!File.Exists(itemMappingsPath) || !File.Exists(blockMappingsPath)) {
                MessageBox.Show("itemmappings.nim or blockmappings.nim could not be found in the world folder for file " + filePath + ". Make sure that the file you're trying to open is in the Player folder of the world it was created in!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            statusBar.SetText("Opening " + filePath + "...");
            try {
                tabControl.AddTab(new PlayerTab(new PlayerDataFile(filePath, blockMappingsPath, itemMappingsPath, XmlData.blocks, XmlData.items, XmlData.itemModifiers, XmlData.quests, XmlData.traders), filePath, selectedIndex));
                statusBar.Reset();
                return true;
            } catch (Exception e) when (e is MismatchedSaveVersionException || e is KnownBugException) {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusBar.Reset();
                return false;
            } /*catch (IOException) {
                MessageBox.Show("Failed to open file " + filePath + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusBar.Reset();
                return false;
            }*/

        }

        /// <summary>
        /// Opens a save file dialog and saves selected tab to choosen location
        /// </summary>
        /// <param name="tab">Tab to save</param>
        /// <returns>True if the tab was successfully saved, false otherwise</returns>
        private bool SaveAs(PlayerTab tab) {
            SaveFileDialog saveFile = new SaveFileDialog {
                Filter = "7 Days to Die save file(*.ttp)|*.ttp"
            };

            if (saveFile.ShowDialog() == DialogResult.OK) {
                return Save(tab, saveFile.FileName);
            }

            return false;
        }

        /// <summary>
        /// Saves selected tab to choosen location
        /// </summary>
        /// <param name="tab">Tab to save</param>
        /// <param name="path">Location to save the file to</param>
        /// <returns>True if the tab was successfully saved, false otherwise</returns>
        private bool Save(PlayerTab tab, string path) {
            statusBar.SetText("Saving " + tab.fileName + "...");

            if (!path.EndsWith(".ttp")) {
                path += ".ttp";
            }

            try {
                BackupFile(path);

                tab.playerDataFile.Save(path);
                tab.path = path;
                tab.fileName = path.Substring(path.LastIndexOf('\\') + 1);
                MessageBox.Show("Successfully saved file " + path + ".", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } catch (IOException) {
                statusBar.Reset();
                MessageBox.Show("Failed to save file " + path + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            statusBar.Reset();
            return true;
        }

        /// <summary>
        /// Asks to save the tab, and closes it if successful
        /// </summary>
        /// <param name="tab">Tab to close</param>
        /// <returns>True if the tabs saved and closed successfully, false otherwise</returns>
        private bool Close(PlayerTab tab) {
            DialogResult result = MessageBox.Show(string.Format("Do you wish to save file {0} before closing?", tab.fileName), "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            switch (result) {
                case DialogResult.Yes:
                    if (SaveAs(tab)) {
                        tabControl.TabPages.Remove(tab);
                        return true;
                    }
                    return false;

                case DialogResult.No:
                    tabControl.TabPages.Remove(tab);
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Calls Close(PlayerTab) on all the tabs
        /// </summary>
        /// <returns>True if all the tabs were saved and closed successfully, flase otherwise</returns>
        internal bool CloseAll() {
            bool successfullyClosed = true;
            foreach (PlayerTab tab in tabControl.TabPages) {
                if (!Close(tab)) {
                    successfullyClosed = false;
                }              
            }

            return successfullyClosed;
        }

        /// <summary>
        /// Creates a backup of a file.
        /// </summary>
        /// <param name="path">Path to file</param>
        private void BackupFile(string path) {
            DateTime now = DateTime.Now;
            string timeStamp = string.Format("{0}-{1}-{2}-{3}-{4}-{5}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

            if (File.Exists(path)) {
                File.Copy(path, string.Format("{0}_{1}.bkp", path.Substring(0, path.LastIndexOf('.')), timeStamp));
            }
        }
        /*
        /// <summary>
        /// Process the inventory for saving.
        /// </summary>
        /// <param name="itemStacks">Inventory to process</param>
        private void CleanUpInventory(ItemStack[] itemStacks) {
            /*for (int i = 0; i < itemStacks.Length; i++) {
                ItemValue itemValue = itemStacks[i].itemValue;

                // Remove empty attachments.
                List<ItemValue> attachments = itemValue.attachments;
                if (attachments.Count > 0) {
                    for (int j = 0; j < attachments.Count; j++) {
                        if (attachments[j].type.Get() == 0) {
                            attachments.RemoveAt(j);
                            j--;
                        }
                    }
                }

                // Set single part/attachment weapons to that item.
                if (itemValue.parts[0] != null) {
                    ItemValue[] partsAndAttachments = new ItemValue[4 + itemValue.attachments.Count];
                    for (int j = 0; j < 4; j++) {
                        partsAndAttachments[j] = itemValue.parts[j];
                    }

                    for (int j = 0; j < itemValue.attachments.Count; j++) {
                        partsAndAttachments[j + 4] = itemValue.attachments[j];
                    }

                    int count = 0;
                    int index = 0;

                    for (int j = 0; j < partsAndAttachments.Length; j++) {
                        if (partsAndAttachments[j].type.Get() != 0) {
                            count++;
                            index = j;
                        }
                    }

                    if (count <= 1) {
                        itemStacks[i].itemValue = partsAndAttachments[index];
                    }
                }
            }
        }

        /// <summary>
        /// Opens a file.
        /// </summary>
        /// <param name="fileName">File to open</param>
        /// <param name="playerDataFile">Holds all the ttp info of a file</param>
        /// <param name="selectedIndex">Tab index to be selected after opening</param>
        private void OpenFile(string fileName, PlayerDataFile playerDataFile, int selectedIndex) {
            /*ItemStack[] inventory = playerDataFile.inventory;
            ItemStack[] bag = playerDataFile.bag;

            List<string> devItems = new List<string>();

            for (int i = 0; i < bag.Length; i++) {
                ItemData devItem = ItemData.GetDevItemById(bag[i].itemValue.type.Get());
                if (devItem != null) {
                    devItems.Add(string.Format("{0} at position: row {1}, column {2}\n", devItem.name, (i / 8) + 1, (i % 8) + 1));
                }
            }

            for (int i = 0; i < inventory.Length; i++) {
                ItemData devItem = ItemData.GetDevItemById(inventory[i].itemValue.type.Get());
                if (devItem != null) {
                    devItems.Add(string.Format("{0} at position: row 5, column {1}\n", devItem.name, i + 1));
                }
            }

            if (devItems.Count == 0) {*/
            /*try {
                PlayerTab tab = new PlayerTab(playerDataFile, fileName, selectedIndex);

                playerTabControl.AddTab(tab);
            }
            catch (Exception e) {
                //ErrorHandler.HandleError(string.Format("Failed to open file {0}. {1}", fileName, e.Message), e, true, fileName);
            }
        /*}
        else {

            // Split the full path into an array where the last element will be the short filename
            string[] fullPathFileNameArray = fileName.Split('\\');

            // Capture the short filename from the last element of the array
            string shortFileName = fullPathFileNameArray[fullPathFileNameArray.Length - 1];
            string text = string.Format("Player file {0} has the following developer items in their inventory:\n\n", shortFileName);

            for (int i = 0; i < devItems.Count; i++) {
                text += devItems[i] + "\n";
            }

            text += "\nPlease remove these items before continuing.";

            MessageBox.Show(text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        }

        /// <summary>
        /// Processes the file for saving.
        /// </summary>
        /// <param name="playerDataFile"></param>
        private void PostProcess(PlayerDataFile playerDataFile) {
            /*PlayerDataFile copy = playerDataFile.Clone();

            // Remove skills with default values.
            Dictionary<int, Skill> newSkillDictionary = new Dictionary<int, Skill>();

            foreach (int key in copy.skills.skillDictionary.Keys) {
                Skill skill = null;
                copy.skills.skillDictionary.TryGetValue(key, out skill);

                if (!SkillBinder.IsDefaultValues(skill)) {
                    newSkillDictionary.Add(key, skill);
                }
            }

            playerDataFile.skills.skillDictionary = newSkillDictionary;

            CleanUpInventory(playerDataFile.bag);
            CleanUpInventory(playerDataFile.inventory);
        }*/
    }
}
