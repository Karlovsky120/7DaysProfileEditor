using SevenDaysProfileEditor.Inventory;
using SevenDaysProfileEditor.Skills;
using SevenDaysSaveManipulator.GameData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI
{
    class MainMenuActions
    {
        MainWindow mainWindow;
        PlayerTabControl playerTabControl;
        BottomStatusBar bottomStatusBar;

        public MainMenuActions(MainWindow mainWindow, PlayerTabControl playerTabControl, BottomStatusBar statusBar)
        {
            this.mainWindow = mainWindow;
            this.playerTabControl = playerTabControl;
            this.bottomStatusBar = statusBar;
        }

        public void Open()
        {
            OpenFileDialog openFile = new OpenFileDialog();

            openFile.Filter = "7 Days to Die save file(*.ttp)|*.ttp|All Files(*.*)|*.*";
            openFile.Multiselect = true;
            openFile.CheckFileExists = true;

            openFile.FileOk += (sender, e) =>
            {
                bottomStatusBar.SetText("Opening...");

                foreach (string fileName in openFile.FileNames)
                {
                    PlayerDataFile playerDataFile = new PlayerDataFile(fileName);

                    ItemStack[] inventory = playerDataFile.inventory;
                    ItemStack[] bag = playerDataFile.bag;

                    List<string> devItems = new List<string>();

                    for (int i = 0; i < bag.Length; i++)
                    {
                        ItemData devItem = ItemData.GetDevItemById(bag[i].itemValue.type.Get());
                        if (devItem != null)
                        {
                            devItems.Add(devItem.name + " at position: row " + ((i / 8) + 1) + ", column " + ((i % 8) + 1) + "\n");
                        }
                    }

                    for (int i = 0; i < inventory.Length; i++)
                    {
                        ItemData devItem = ItemData.GetDevItemById(inventory[i].itemValue.type.Get());
                        if (devItem != null)
                        {
                            devItems.Add(devItem.name + " at position: row 5, column " + (i + 1) + "\n");
                        }
                    }

                    if (devItems.Count == 0)
                    {
                        try
                        {
                            playerTabControl.AddTab(new PlayerTab(playerDataFile, fileName));
                        }

                        catch (Exception e2)
                        {
                            Log.WriteError(e2);
                            MessageBox.Show("Failed to open file " + fileName + ". " + e2.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    else
                    {
                        string text = "You have following developer items in your inventory:\n";

                        for (int i = 0; i < devItems.Count; i++)
                        {
                            text += devItems[i] + "\n";
                        }

                        text += "\nPlease remove these items before continuing.";

                        MessageBox.Show(text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                bottomStatusBar.Reset();
            };

            openFile.ShowDialog();
        }

        public void Save(PlayerTab tab)
        {
            Save(tab, tab.path);
        }

        public void Save(PlayerTab tab, string path)
        {
            bottomStatusBar.SetText("Saving...");

            bool success = false;

            //try
            {
                BackupFile(path);

                if (!path.EndsWith(".ttp"))
                {
                    path += ".ttp";
                }

                PlayerDataFile playerDataFile = tab.playerDataFile.Clone();

                PostProcess(playerDataFile);

                playerDataFile.Write(path);

                success = true;
            }

            /*catch (Exception e)
            {
                Log.WriteError(e);
                MessageBox.Show("Failed to save file " + path.Substring(path.LastIndexOf('\\')+1) + ". " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/

            bottomStatusBar.Reset();

            if (success)
            {
                MessageBox.Show("File " + path.Substring(path.LastIndexOf('\\') + 1) + " saved!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void Close(PlayerTab tab)
        {
            DialogResult result = MessageBox.Show("Do you wish to save file " + tab.fileName + " before closing?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            switch (result)
            {
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
        

        private void PostProcess(PlayerDataFile playerDataFile)
        {
            PlayerDataFile copy = playerDataFile.Clone();

            // Remove skills with default values.
            Dictionary<int, Skill> newSkillDictionary = new Dictionary<int, Skill>();

            foreach (int key in copy.skills.skillDictionary.Keys)
            {
                Skill skill = null;
                copy.skills.skillDictionary.TryGetValue(key, out skill);

                if (!SkillBinder.IsDefaultValues(skill))
                {
                    newSkillDictionary.Add(key, skill);
                }
            }

            playerDataFile.skills.skillDictionary = newSkillDictionary;

            CleanUpInventory(playerDataFile.bag);
            CleanUpInventory(playerDataFile.inventory);
        }

        private void CleanUpInventory(ItemStack[] itemStacks)
        {
            for (int i = 0; i < itemStacks.Length; i++)
            {
                ItemValue itemValue = itemStacks[i].itemValue;

                // Remove empty attachments.
                List<ItemValue> attachments = itemValue.attachments;
                if (attachments.Count > 0)
                {
                    for (int j = 0; j < attachments.Count; j++)
                    {
                        if (attachments[j].type.Get() == 0)
                        {
                            attachments.RemoveAt(j);
                            j--;
                        }
                    }
                }

                // Set single part/attachment weapons to that item.
                if (itemValue.parts[0] != null)
                {
                    ItemValue[] partsAndAttachments = new ItemValue[4 + itemValue.attachments.Count];
                    for (int j = 0; j < 4; j++)
                    {
                        partsAndAttachments[j] = itemValue.parts[j];
                    }

                    for (int j = 0; j < itemValue.attachments.Count; j++)
                    {
                        partsAndAttachments[j + 4] = itemValue.attachments[j];
                    }

                    int count = 0;
                    int index = 0;

                    for (int j = 0; j < partsAndAttachments.Length; j++)
                    {
                        if (partsAndAttachments[j].type.Get() != 0)
                        {
                            count++;
                            index = j;
                        }
                    }

                    if (count <= 1)
                    {
                        itemStacks[i].itemValue = partsAndAttachments[index];
                    }
                }
            }
        }

        private void BackupFile(string path)
        {
            DateTime now = DateTime.Now;
            string timeStamp = "" + now.Year + now.Month + now.Day + now.Hour + now.Minute + now.Second;

            if (File.Exists(path))
            {
                File.Copy(path, path.Substring(0, path.LastIndexOf('.')) + "_" + timeStamp + ".bkp");
            }
        }
    }
}
