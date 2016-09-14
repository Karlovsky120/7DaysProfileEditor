using SevenDaysProfileEditor.Inventory;
using SevenDaysProfileEditor.Skills;
using SevenDaysSaveManipulator.GameData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI
{
    class TopMainMenu : MenuStrip
    {
        public MainMenuActions mainMenuActions;

        private PlayerTabControl playerTabs;

        private ToolStripMenuItem itemOpen;
        private ToolStripMenuItem itemSave;
        private ToolStripMenuItem itemSaveAs;
        private ToolStripMenuItem itemClose;
        private ToolStripMenuItem itemCloseAll;
        private ToolStripMenuItem itemExit;
        private ToolStripMenuItem itemAbout;

        public TopMainMenu(MainWindow mainWindow, PlayerTabControl tabsControlPlayer, BottomStatusBar statusBar)
        {            
            this.playerTabs = tabsControlPlayer;

            mainMenuActions = new MainMenuActions(mainWindow, tabsControlPlayer, statusBar);

            ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");

            itemOpen = new ToolStripMenuItem("Open...");
            itemOpen.ShortcutKeys = Keys.Control | Keys.O;
            itemOpen.Click += new EventHandler(OpenClick);
            fileMenu.DropDownItems.Add(itemOpen);

            itemSave = new ToolStripMenuItem("Save");
            itemSave.Click += new EventHandler(SaveClick);
            itemSave.ShortcutKeys = Keys.Control | Keys.S;
            itemSave.Enabled = false;
            fileMenu.DropDownItems.Add(itemSave);

            itemSaveAs = new ToolStripMenuItem("Save As...");
            itemSaveAs.Click += new EventHandler(SaveAsClick);
            itemSaveAs.ShortcutKeys = Keys.Control | Keys.Alt | Keys.S;
            itemSaveAs.Enabled = false;
            fileMenu.DropDownItems.Add(itemSaveAs);

            fileMenu.DropDownItems.Add(new ToolStripSeparator());

            itemClose = new ToolStripMenuItem("Close");
            itemClose.Click += new EventHandler(CloseClick);
            itemClose.ShortcutKeys = Keys.Control | Keys.W;
            itemClose.Enabled = false;
            fileMenu.DropDownItems.Add(itemClose);

            itemCloseAll = new ToolStripMenuItem("Close all");
            itemCloseAll.Click += new EventHandler(CloseAllClick);
            itemCloseAll.ShortcutKeys = Keys.Control | Keys.Alt | Keys.W;
            itemCloseAll.Enabled = false;
            fileMenu.DropDownItems.Add(itemCloseAll);

            fileMenu.DropDownItems.Add(new ToolStripSeparator());

            itemExit = new ToolStripMenuItem("Exit");
            itemExit.Click += new EventHandler(ExitClick);
            itemExit.ShortcutKeys = Keys.Alt | Keys.F4;
            fileMenu.DropDownItems.Add(itemExit);

            Items.Add(fileMenu);

            ToolStripMenuItem helpMenu = new ToolStripMenuItem("Help");

            itemAbout = new ToolStripMenuItem("About");
            itemAbout.Click += new EventHandler(AboutClick);
            helpMenu.DropDownItems.Add(itemAbout);

            Items.Add(helpMenu);
        }

        public void UpdateMenus(int tabs)
        {
            if (tabs > 0)
            {
                itemSave.Enabled = true;
                itemSaveAs.Enabled = true;
                itemClose.Enabled = true;
                itemCloseAll.Enabled = true;
            }

            else
            {
                itemSave.Enabled = false;
                itemSaveAs.Enabled = false;
                itemClose.Enabled = false;
                itemCloseAll.Enabled = false;             
            }
        }

        private void OpenClick(object sender, System.EventArgs e)
        {
            mainMenuActions.Open();
        }

        private void SaveClick(object sender, EventArgs e)
        {
            if (playerTabs.GetTabCount() > 0)
            {               
                mainMenuActions.Save(playerTabs.GetSelectedTab());
            }
        }

        private void SaveAsClick(object sender, EventArgs e)
        {
            if (playerTabs.GetTabCount() > 0)
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = "7 Days to Die save file(*.ttp)|*.ttp";

                saveFile.FileOk += (sender1, e1) =>
                {
                    mainMenuActions.Save(playerTabs.GetSelectedTab(), saveFile.FileName);
                };

                saveFile.ShowDialog();
            }
        }

        private void CloseClick(object sender, EventArgs e)
        {
            if (playerTabs.GetTabCount() > 0)
            {
                mainMenuActions.Close(playerTabs.GetSelectedTab());
            }
        }
        private void CloseAllClick(object sender, EventArgs e)
        {
            foreach (PlayerTab tab in playerTabs.TabPages)
            {
                mainMenuActions.Close(tab);
            }
        }

        private void ExitClick(object sender, EventArgs e)
        {
            CloseAllClick(sender, e);

            if (playerTabs.TabCount == 0)
            {
                Application.Exit();
            }
        }

        private void AboutClick(object sender, EventArgs e)
        {
            new AboutWindow();
        }
    }
}
