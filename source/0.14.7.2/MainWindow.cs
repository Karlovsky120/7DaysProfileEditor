using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI
{
    class MainWindow : Form
    {
        public TopMainMenu mainMenu;
        public PlayerTabControl tabs;
        public BottomStatusBar statusBar;

        public Label focusDummy;

        public MainWindow()
        {
            Size = new Size(1000, 850);

            Text = "7 Days Profile Editor";
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            tabs = new PlayerTabControl(this);
            statusBar = new BottomStatusBar();
            mainMenu = new TopMainMenu(this, tabs, statusBar);

            focusDummy = new Label();
            focusDummy.Size = new Size(0, 0);
            Controls.Add(focusDummy);

            Controls.Add(tabs);
            Controls.Add(mainMenu);
            Controls.Add(statusBar);
        }
    }
}
