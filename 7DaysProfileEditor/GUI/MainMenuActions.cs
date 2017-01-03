using SevenDaysProfileEditor.Inventory;
using SevenDaysProfileEditor.Skills;
using SevenDaysSaveManipulator.GameData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI {

    /// <summary>
    /// Stores all the actions to be taken when any menu item is clicked.
    /// </summary>
    internal class MainMenuActions {
        private BottomStatusBar bottomStatusBar;
        private MainWindow mainWindow;
        private PlayerTabControl playerTabControl;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="mainWindow">The program main window</param>
        /// <param name="playerTabControl">Tab control that holds all ttp files</param>
        /// <param name="statusBar">Status bar of the main window</param>
        public MainMenuActions(MainWindow mainWindow, PlayerTabControl playerTabControl, BottomStatusBar statusBar) {
            this.mainWindow = mainWindow;
            this.playerTabControl = playerTabControl;
            this.bottomStatusBar = statusBar;
        }

        /// <summary>
        /// Closes a specific tab.
        /// </summary>
        /// <param name="tab">Tab to close</param>
        public void Close(PlayerTab tab) {
            DialogResult result = MessageBox.Show(string.Format("Do you wish to save file {0} before closing?", tab.fileName), "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            switch (result) {
                case DialogResult.Yes:
                    Save(tab);
                    playerTabControl.TabPages.Remove(tab);
                    break;

                case DialogResult.No:
                    playerTabControl.TabPages.Remove(tab);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Initiates an open file dialog.
        /// </summary>
        public void Open() {
            OpenFileDialog openFile = new OpenFileDialog() {
                Filter = "7 Days to Die save file(*.ttp)|*.ttp|All Files(*.*)|*.*",
                Multiselect = true,
                CheckFileExists = true
            };

            openFile.FileOk += (sender, e) => {
                bottomStatusBar.SetText("Opening...");

                foreach (string fileName in openFile.FileNames) {
                    OpenFile(fileName, new PlayerDataFile(fileName), 0);
                }

                bottomStatusBar.Reset();
            };

            openFile.ShowDialog();
        }

        /// <summary>
        /// Reloads a tab. Useful for testing.
        /// </summary>
        /// <param name="tab">Tab to reload</param>
        public void Reload(PlayerTab tab) {
            bottomStatusBar.SetText("Reloading...");

            string path = tab.path;
            int selectedIndex = tab.ttpFileTabControl.SelectedIndex;

            playerTabControl.TabPages.Remove(tab);
            OpenFile(path, new PlayerDataFile(path), selectedIndex);

            bottomStatusBar.Reset();
        }

        /// <summary>
        /// Saves a specific tab.
        /// </summary>
        /// <param name="tab">Tab to save</param>
        public void Save(PlayerTab tab) {
            Save(tab, tab.path);
        }

        /// <summary>
        /// Saves a specific tab to specified location.
        /// </summary>
        /// <param name="tab">Tab to save</param>
        /// <param name="path">Path of the location to save the tab to</param>
        public void Save(PlayerTab tab, string path) {
            mainWindow.focusDummy.Focus();

            bottomStatusBar.SetText("Saving...");

            try {
                BackupFile(path);

                if (!path.EndsWith(".ttp")) {
                    path += ".ttp";
                }

                PlayerDataFile playerDataFile = tab.playerDataFile.Clone();

                PostProcess(playerDataFile);

                playerDataFile.Write(path);

                tab.path = path;
                tab.fileName = path.Substring(path.LastIndexOf('\\') + 1);

                MessageBox.Show(string.Format("File {0} saved!", path.Substring(path.LastIndexOf('\\') + 1)), "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception e) {
                ErrorHandler.HandleError(string.Format("Failed to save file {0}. {1}", path.Substring(path.LastIndexOf('\\') + 1), e.Message), e, true, tab.path);
            }

            bottomStatusBar.Reset();
        }

        /// <summary>
        /// Creates a backup of a file.
        /// </summary>
        /// <param name="path">Path to file</param>
        private void BackupFile(string path) {
            DateTime now = DateTime.Now;
            string timeStamp = string.Format("{0}{1}{2}{3}{4}{5}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

            if (File.Exists(path)) {
                File.Copy(path, string.Format("{0}_{1}.bkp", path.Substring(0, path.LastIndexOf('.')), timeStamp));
            }
        }

        /// <summary>
        /// Process the inventory for saving.
        /// </summary>
        /// <param name="itemStacks">Inventory to process</param>
        private void CleanUpInventory(ItemStack[] itemStacks) {
            for (int i = 0; i < itemStacks.Length; i++) {
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
            ItemStack[] inventory = playerDataFile.inventory;
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

            if (devItems.Count == 0) {
                try {
                    PlayerTab tab = new PlayerTab(playerDataFile, fileName, selectedIndex);
                    playerTabControl.AddTab(tab);
                }
                catch (Exception e) {
                    ErrorHandler.HandleError(string.Format("Failed to open file {0}. {1}", fileName, e.Message), e, true, fileName);
                }
            }
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
            PlayerDataFile copy = playerDataFile.Clone();

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
        }
    }
}