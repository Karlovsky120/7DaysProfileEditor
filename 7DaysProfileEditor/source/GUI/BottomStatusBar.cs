using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI {

    /// <summary>
    /// Bottom status bar of the main window.
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    internal class BottomStatusBar : StatusStrip {
        private ToolStripStatusLabel label;

        /// <summary>
        /// Default constructor.
        /// </summary>
        internal BottomStatusBar() {
            SizingGrip = false;

            label = new ToolStripStatusLabel {
                BorderStyle = Border3DStyle.Sunken,
                Text = "Ready!",
                ToolTipText = "Status bar",
            };

            Items.Add(label);
        }

        /// <summary>
        /// Resets the status bar to say "Ready!".
        /// </summary>
        internal void Reset() {
            SetText("Ready!");
        }

        /// <summary>
        /// Sets the text of the status bar.
        /// </summary>
        /// <param name="text">Text to be set</param>
        internal void SetText(string text) {
            label.Text = text;
        }
    }
}