using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI {

    /// <summary>
    /// Holds all the tabs that represent a ttp each.
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    internal class PlayerTabControl : TabControl {

        private CentralController centralController;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="mainMenuStrip">Menu strip of the program</param>
        internal PlayerTabControl(CentralController centralController) {
            this.centralController = centralController;

            Dock = DockStyle.Fill;
            DrawMode = TabDrawMode.OwnerDrawFixed;

            //Add a little x for closing the tab
            DrawItem += (sender, e) => {
                e.Graphics.DrawString("x", e.Font, Brushes.Black, e.Bounds.Right - 13, e.Bounds.Top + 1);
                e.Graphics.DrawString(TabPages[e.Index].Text, e.Font, Brushes.Black, e.Bounds.Left + 12, e.Bounds.Top + 4);
                e.DrawFocusRectangle();
            };

            MouseDown += (sender, e) => {
                foreach (PlayerTab tab in TabPages) {
                    Rectangle r = GetTabRect(TabPages.IndexOf(tab));
                    Rectangle closeButton = new Rectangle(r.Right - 13, r.Top + 1, 11, 10);
                    if (closeButton.Contains(e.Location)) {
                        //mainWindow.mainMenu.mainMenuActions.Close(tab);
                    }
                }
            };

            ControlAdded += (sender, e) => {
                centralController.UpdateMenus(GetTabCount());
            };

            ControlRemoved += (sender, e) => {
                // GetTabCount() - 1 since tab count isn't updated right away for some reason.
                centralController.UpdateMenus(GetTabCount() - 1);
            };
        }

        /// <summary>
        /// Adds a tab.
        /// </summary>
        /// <param name="tab">Tab to add</param>
        public void AddTab(PlayerTab tab) {
            Controls.Add(tab);
        }

        /// <summary>
        /// Gets the currently selected tab
        /// </summary>
        /// <returns>Currently selected tab</returns>
        public PlayerTab GetSelectedTab() {
            return (PlayerTab)SelectedTab;
        }

        /// <summary>
        /// Gets the tab count
        /// </summary>
        /// <returns>Tab count</returns>
        public int GetTabCount() {
            return TabPages.Count;
        }
    }
}