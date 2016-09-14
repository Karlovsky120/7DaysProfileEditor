using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI
{
    class LabeledControl : TableLayoutPanel
    {
        public LabeledControl(string name, Control control, int width)
        {
            Size = new Size(width, 28); 

            Label label = new Label();
            label.Text = name + ":";
            label.AutoSize = true;
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.Anchor = AnchorStyles.Left;
            
            control.Anchor = AnchorStyles.Right;

            Controls.Add(label, 0, 0);
            Controls.Add(control, 1, 0);
        }
    }
}
