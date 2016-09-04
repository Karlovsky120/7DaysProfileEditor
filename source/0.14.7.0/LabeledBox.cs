using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI
{
    class LabeledBox : TableLayoutPanel
    {
        public LabeledBox(string name, Control textBox)
        {
            AutoSize = true;

            Label label = new Label();
            label.Text = name + ":";
            label.Width = 110;
            label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            Controls.Add(label, 0, 0);
            Controls.Add(textBox, 1, 0);
        }
    }
}
