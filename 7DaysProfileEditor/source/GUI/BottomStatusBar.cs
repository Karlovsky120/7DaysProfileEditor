using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI {

    /// <summary>
    /// Bottom status bar of the main window.
    /// </summary>
    internal class BottomStatusBar : StatusBar {
        public StatusBarPanel statusPanel;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BottomStatusBar() {
            SizingGrip = false;

            statusPanel = new StatusBarPanel();

            statusPanel.BorderStyle = StatusBarPanelBorderStyle.Sunken;
            statusPanel.Text = "Ready!";
            statusPanel.ToolTipText = "Last Activity";
            statusPanel.AutoSize = StatusBarPanelAutoSize.Spring;

            Panels.Add(statusPanel);

            ShowPanels = true;
        }

        /// <summary>
        /// Resets the status bar to say "Ready!".
        /// </summary>
        public void Reset() {
            SetText("Ready!");
        }

        /// <summary>
        /// Sets the text of the status bar.
        /// </summary>
        /// <param name="text">Text to be set</param>
        public void SetText(string text) {
            statusPanel.Text = text;
        }
    }
}