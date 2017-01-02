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
            BackColor = System.Drawing.Color.Coral;

            AutoSize = true;

            Label label = new Label();
            label.BackColor = System.Drawing.Color.Chartreuse;
            label.Text = name + ":";
            label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            Controls.Add(label, 0, 0);
            Controls.Add(textBox, 1, 0);
        }
    }
}
