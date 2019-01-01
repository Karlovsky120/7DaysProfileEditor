using System;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI {

    /// <summary>
    /// The top menu strip
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    internal class MainMenuStrip : MenuStrip {
        private CentralController centralController;

        private ToolStripMenuItem itemAbout;
        private ToolStripMenuItem itemClose;
        private ToolStripMenuItem itemCloseAll;
        private ToolStripMenuItem itemExit;
        private ToolStripMenuItem itemOpen;
        private ToolStripMenuItem itemReload;
        private ToolStripMenuItem itemSave;
        private ToolStripMenuItem itemSaveAs;
        //private ToolStripMenuItem itemSendReport;
        private ToolStripMenuItem itemForumPage;
        private ToolStripMenuItem itemGithubPage;

        /// <summary>
        /// Default contructor.
        /// </summary>
        /// <param name="mainWindow">Main window of the program</param>
        /// <param name="tabsControlPlayer">Tab control that holds all the ttps</param>
        /// <param name="statusBar">Bottom status bar</param>
        internal MainMenuStrip(CentralController centralController) {
            this.centralController = centralController;

            ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");

            itemOpen = new ToolStripMenuItem("Open...");
            itemOpen.Click += new EventHandler(centralController.MenuOpen);
            itemOpen.ShortcutKeys = Keys.Control | Keys.O;
            fileMenu.DropDownItems.Add(itemOpen);

            itemReload = new ToolStripMenuItem("Reload");
            itemReload.Click += new EventHandler(centralController.MenuReload);
            itemReload.ShortcutKeys = Keys.Control | Keys.R;
            itemReload.Enabled = false;
            fileMenu.DropDownItems.Add(itemReload);

            itemSave = new ToolStripMenuItem("Save");
            itemSave.Click += new EventHandler(centralController.MenuSave);
            itemSave.ShortcutKeys = Keys.Control | Keys.S;
            itemSave.Enabled = false;
            fileMenu.DropDownItems.Add(itemSave);

            itemSaveAs = new ToolStripMenuItem("Save As...");
            itemSaveAs.Click += new EventHandler(centralController.MenuSaveAs);
            itemSaveAs.ShortcutKeys = Keys.Control | Keys.Alt | Keys.S;
            itemSaveAs.Enabled = false;
            fileMenu.DropDownItems.Add(itemSaveAs);

            fileMenu.DropDownItems.Add(new ToolStripSeparator());

            itemClose = new ToolStripMenuItem("Close");
            itemClose.Click += new EventHandler(centralController.MenuClose);
            itemClose.ShortcutKeys = Keys.Control | Keys.W;
            itemClose.Enabled = false;
            fileMenu.DropDownItems.Add(itemClose);

            itemCloseAll = new ToolStripMenuItem("Close all");
            itemCloseAll.Click += new EventHandler(centralController.MenuCloseAll);
            itemCloseAll.ShortcutKeys = Keys.Control | Keys.Alt | Keys.W;
            itemCloseAll.Enabled = false;
            fileMenu.DropDownItems.Add(itemCloseAll);

            fileMenu.DropDownItems.Add(new ToolStripSeparator());

            itemExit = new ToolStripMenuItem("Exit");
            itemExit.Click += new EventHandler(centralController.MenuExit);
            itemExit.ShortcutKeys = Keys.Alt | Keys.F4;
            fileMenu.DropDownItems.Add(itemExit);

            Items.Add(fileMenu);

            ToolStripMenuItem helpMenu = new ToolStripMenuItem("Help");

            /*itemSendReport = new ToolStripMenuItem("Send error report");
            itemSendReport.Click += new EventHandler(SendReportClick);
            helpMenu.DropDownItems.Add(itemSendReport);*/

            itemForumPage = new ToolStripMenuItem("Forum page");
            itemForumPage.Click += new EventHandler(centralController.MenuForumPage);
            helpMenu.DropDownItems.Add(itemForumPage);

            itemGithubPage = new ToolStripMenuItem("Github page");
            itemGithubPage.Click += new EventHandler(centralController.MenuGithubPage);
            helpMenu.DropDownItems.Add(itemGithubPage);

            helpMenu.DropDownItems.Add(new ToolStripSeparator());

            itemAbout = new ToolStripMenuItem("About");
            itemAbout.Click += new EventHandler(centralController.MenuAbout);
            helpMenu.DropDownItems.Add(itemAbout);

            Items.Add(helpMenu);
        }

        /// <summary>
        /// Triggered when file is opened/closed. Used to enable/disable aproppriate menus.
        /// </summary>
        /// <param name="tabCount">Number of opened tabs</param>
        internal void UpdateMenus(int tabCount) {
            itemReload.Enabled = tabCount > 0;
            itemSave.Enabled = tabCount > 0;
            itemSaveAs.Enabled = tabCount > 0;
            itemClose.Enabled = tabCount > 0;
            itemCloseAll.Enabled = tabCount > 0;
        }

        /// <summary>
        /// Event handler for about button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutClick(object sender, EventArgs e) {
            //new AboutWindow();
        }

        /// <summary>
        /// Event handler for save error report button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendReportClick(object sender, EventArgs e) {
            /*string path = "";

            if (playerTabs.GetTabCount() > 0) {
                path = playerTabs.GetSelectedTab().path;
            }

            ErrorHandler.SaveReport(path);*/
        }
    }
}