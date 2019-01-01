using System.Reflection;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI {

    /// <summary>
    /// Main window of the program
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    internal class MainWindow : Form {
        private MainMenuStrip mainMenu;
        private BottomStatusBar statusBar;
        private PlayerTabControl tabs;
        private CentralController centralController;


        /// <summary>
        /// Default constructor.
        /// </summary>
        internal MainWindow() {

            Size = new Size(1000, 850);

            Text = "7 Days Profile Editor - v" + Assembly.GetEntryAssembly().GetName().Version;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            centralController = new CentralController();

            mainMenu = new MainMenuStrip(centralController);
            tabs = new PlayerTabControl(centralController);
            statusBar = new BottomStatusBar();

            centralController.Initialize(mainMenu, tabs, statusBar);

            Controls.Add(tabs);
            Controls.Add(mainMenu);
            Controls.Add(statusBar);

        }
    }
}