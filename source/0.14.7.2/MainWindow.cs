using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI
{
    class MainWindow : Form
    {
        public TopMainMenu mainMenu;
        public PlayerTabControl tabs;
        public BottomStatusBar statusBar;

        public MainWindow() : base()
        {
            //Size = new Size(1920, 1080);
            Size = new Size(1000, 850);

            Text = "7 Days Profile Editor";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            tabs = new PlayerTabControl(this);
            statusBar = new BottomStatusBar();
            mainMenu = new TopMainMenu(this, tabs, statusBar);

            Controls.Add(tabs);
            Controls.Add(mainMenu);
            Controls.Add(statusBar);
        }
    }
}
