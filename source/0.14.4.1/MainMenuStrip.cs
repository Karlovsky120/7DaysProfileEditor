using SevenDaysProfileEditor.Skills;
using SevenDaysSaveManipulator.GameData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI
{
    class MainMenuStrip : MenuStrip
    {
        private WindowMain mainWindow;
        private Tabs tabs;
        private BottomStatusBar statusBar;

        private ToolStripMenuItem itemOpen;
        private ToolStripMenuItem itemSave;
        private ToolStripMenuItem itemSaveAs;
        private ToolStripMenuItem itemClose;
        private ToolStripMenuItem itemCloseAll;
        private ToolStripMenuItem itemExit;
        private ToolStripMenuItem itemAbout;

        public MainMenuStrip(WindowMain mainWindow, Tabs tabs, BottomStatusBar statusBar) : base()
        {
            this.mainWindow = mainWindow;
            this.tabs = tabs;
            this.statusBar = statusBar;

            ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");

            itemOpen = new ToolStripMenuItem("Open...");
            itemOpen.ShortcutKeys = Keys.Control | Keys.O;
            itemOpen.Click += new EventHandler(openClick);
            fileMenu.DropDownItems.Add(itemOpen);

            itemSave = new ToolStripMenuItem("Save");
            itemSave.Click += new EventHandler(saveClick);
            itemSave.ShortcutKeys = Keys.Control | Keys.S;
            itemSave.Enabled = false;
            fileMenu.DropDownItems.Add(itemSave);

            itemSaveAs = new ToolStripMenuItem("Save As...");
            itemSaveAs.Click += new EventHandler(saveAsClick);
            itemSaveAs.ShortcutKeys = Keys.Control | Keys.Alt | Keys.S;
            itemSaveAs.Enabled = false;
            fileMenu.DropDownItems.Add(itemSaveAs);

            fileMenu.DropDownItems.Add(new ToolStripSeparator());

            itemClose = new ToolStripMenuItem("Close");
            itemClose.Click += new EventHandler(closeClick);
            itemClose.ShortcutKeys = Keys.Control | Keys.W;
            itemClose.Enabled = false;
            fileMenu.DropDownItems.Add(itemClose);

            itemCloseAll = new ToolStripMenuItem("Close all");
            itemCloseAll.Click += new EventHandler(closeAllClick);
            itemCloseAll.ShortcutKeys = Keys.Control | Keys.Alt | Keys.W;
            itemCloseAll.Enabled = false;
            fileMenu.DropDownItems.Add(itemCloseAll);

            fileMenu.DropDownItems.Add(new ToolStripSeparator());

            itemExit = new ToolStripMenuItem("Exit");
            itemExit.Click += new EventHandler(exitClick);
            itemExit.ShortcutKeys = Keys.Alt | Keys.F4;
            fileMenu.DropDownItems.Add(itemExit);

            Items.Add(fileMenu);

            ToolStripMenuItem helpMenu = new ToolStripMenuItem("Help");

            itemAbout = new ToolStripMenuItem("About");
            itemAbout.Click += new EventHandler(aboutClick);
            helpMenu.DropDownItems.Add(itemAbout);

            Items.Add(helpMenu);
        }

        public void updateMenus(int tabs)
        {
            if (tabs < 0)
            {
                itemSave.Enabled = false;
                itemSaveAs.Enabled = false;
                itemClose.Enabled = false;
                itemCloseAll.Enabled = false;
            }

            else
            {
                itemSave.Enabled = true;
                itemSaveAs.Enabled = true;
                itemClose.Enabled = true;
                itemCloseAll.Enabled = true;
            }
        }

        private void openClick(object sender, System.EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            openFile.Filter = "7 Days to Die save file(*.ttp)|*.ttp|All Files(*.*)|*.*";
            openFile.Multiselect = true;
            openFile.CheckFileExists = true;

            openFile.FileOk += (sender1, e1) =>
            {
                statusBar.setText("Opening...");

                foreach (string fileName in openFile.FileNames)
                {
                    try
                    {
                        tabs.addTab(new TabPlayer(fileName));
                    }

                    catch (Exception e2)
                    {
                        Log.writeError(e2);
                        MessageBox.Show("Failed to open file " + fileName + ". " + e2.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                statusBar.reset();
            };

            openFile.ShowDialog();
        }

        private void saveClick(object sender, EventArgs e)
        {
            if (tabs.getTabCount() > 0)
            {               
                save(tabs.getSelectedTab());
            }
        }

        private void saveAsClick(object sender, EventArgs e)
        {
            if (tabs.getTabCount() > 0)
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = "7 Days to Die save file(*.ttp)|*.ttp";

                saveFile.FileOk += (sender1, e1) =>
                {
                    save(tabs.getSelectedTab());
                };

                saveFile.ShowDialog();
            }
        }

        private void closeClick(object sender, EventArgs e)
        {
            if (tabs.getTabCount() > 0)
            {
                close(tabs.getSelectedTab());
            }
        }
        private void closeAllClick(object sender, EventArgs e)
        {
            foreach (TabPlayer tab in tabs.TabPages)
            {
                close(tab);
            }
        }

        private void exitClick(object sender, EventArgs e)
        {
            closeAllClick(sender, e);

            if (tabs.TabCount == 0)
            {
                Application.Exit();
            }
        }

        private void aboutClick(object sender, EventArgs e)
        {
            new WindowAbout();
        }

        private void save(TabPlayer tab)
        {
            statusBar.setText("Saving...");

            bool success = false;
            string name = tab.fileName;
            string path = tab.path;

            try
            {    
                backupFile(path);

                if (!name.EndsWith(".ttp"))
                {
                    name += ".ttp";
                }

                //NEED TO CHECK SKILLS TO REMOVE DEFAULTS!!!
                PlayerDataFile copy = tab.playerDataFile.clone();
                Dictionary<int, Skill> newSkillDictionary = new Dictionary<int, Skill>();

                foreach (int key in copy.skills.skillDictionary.Keys)
                {
                    Skill skill = null;
                    copy.skills.skillDictionary.TryGetValue(key, out skill);

                    if (!BinderSkill.isDefaultValues(skill))
                    {
                        newSkillDictionary.Add(key, skill);
                    }
                }

                copy.skills.skillDictionary = newSkillDictionary;

                copy.Write(path);

                success = true;
            }

            catch (Exception e)
            {
                Log.writeError(e);
                MessageBox.Show("Failed to save file " + name + ". " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            statusBar.reset();

            if (success)
            {
                MessageBox.Show("File " + tab.fileName + " saved!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void backupFile(string path)
        {
            DateTime now = DateTime.Now;
            string backupCode = "" + now.Year + now.Month + now.Day + now.Hour + now.Minute + now.Second;

            if (File.Exists(path))
            {
                //COPY TO MOVE!!
                File.Copy(path, path.Substring(0, path.LastIndexOf('.')) + "_" + backupCode + ".bkp");
            }
        }

        public void close(TabPlayer tab)
        {
            DialogResult result = MessageBox.Show("Do you wish to save file " + tab.fileName + " before closing?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            switch (result)
            {
                case DialogResult.Yes:
                    save(tab);
                    tabs.TabPages.Remove(tab);
                    break;

                case DialogResult.No:
                    tabs.TabPages.Remove(tab);
                    break;

                default:
                    break;
            }

        }
    }
}
