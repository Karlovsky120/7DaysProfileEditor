using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI
{
    internal class LabeledControl : TableLayoutPanel
    {
        public LabeledControl(string name, Control control, int width)
        {
            Size = new Size(width, 28);

            Label label = new Label();
            label.Text = name + ":";
            label.Width = width - control.Width - 10;
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.Anchor = AnchorStyles.Left;

            control.Anchor = AnchorStyles.Right;

            Controls.Add(label, 0, 0);
            Controls.Add(control, 1, 0);
        }
    }
}