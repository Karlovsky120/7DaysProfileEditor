using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI
{
    class WindowMain : Form
    {
        public MainMenuStrip mainMenu;
        public Tabs tabs;
        public BottomStatusBar statusBar;

        public WindowMain() : base()
        {   
            Text = "7DaysProfileEditor";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;

            tabs = new Tabs(this);
            statusBar = new BottomStatusBar();
            mainMenu = new MainMenuStrip(this, tabs, statusBar);

            Controls.Add(tabs);
            Controls.Add(mainMenu);
            Controls.Add(statusBar);

            //Show();
        }
    }
}
