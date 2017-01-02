using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI
{
    class Tabs : TabControl
    {
        public void addTab(TabPlayer tab)
        {
            Controls.Add(tab);
        }

        public TabPlayer getSelectedTab()
        {
            return (TabPlayer)SelectedTab;
        }

        public int getTabCount()
        {
            return TabPages.Count;
        }

        public Tabs(WindowMain windowMain) : base()
        {
            Dock = DockStyle.Fill;
            DrawMode = TabDrawMode.OwnerDrawFixed;

            DrawItem += (sender, e) =>
            {
                e.Graphics.DrawString("x", e.Font, Brushes.Black, e.Bounds.Right - 13, e.Bounds.Top + 1);
                e.Graphics.DrawString(TabPages[e.Index].Text, e.Font, Brushes.Black, e.Bounds.Left + 12, e.Bounds.Top + 4);
                e.DrawFocusRectangle();
            };

            MouseDown += (sender, e) =>
            {
                for (int i = 0; i < TabPages.Count; i++)
                {
                    Rectangle r = GetTabRect(i);
                    Rectangle closeButton = new Rectangle(r.Right - 13, r.Top + 1, 11, 10);
                    if (closeButton.Contains(e.Location))
                    {
                        foreach (TabPlayer tab in TabPages)
                        {
                            if (TabPages.IndexOf(tab) == i)
                            {
                                windowMain.mainMenu.close(tab);
                            }
                        }
                    }
                }
            };

            ControlAdded += (sender, e) =>
            {
                windowMain.mainMenu.updateMenus(getTabCount());
            };

            ControlRemoved += (sender, e) =>
            {
                //-1 since tab count isn't updated right away
                windowMain.mainMenu.updateMenus(getTabCount()-1);
            };
        }
    }
}
