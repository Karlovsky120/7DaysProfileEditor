using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI {

    /// <summary>
    /// Class used to display a control with a name tag in front of it.
    /// </summary>
    internal class LabeledControl : TableLayoutPanel {

        /// <summary>
        /// Creates LabeledControl.
        /// </summary>
        /// <param name="name">Name to be displayed to the left of the control</param>
        /// <param name="control">Control to be displayed</param>
        /// <param name="width">Overall width of the structure</param>
        public LabeledControl(string name, Control control, int width) {
            Size = new Size(width, 28);

            Label label = new Label() {
                Text = name + ":",
                Width = width - control.Width - 10,
                TextAlign = ContentAlignment.MiddleLeft,
                Anchor = AnchorStyles.Left
            };

            control.Anchor = AnchorStyles.Right;

            Controls.Add(label, 0, 0);
            Controls.Add(control, 1, 0);
        }
    }
}