using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI
{
    class WindowAbout : Form
    {
        public WindowAbout()
        {
            Text = "About";
            AutoSize = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;

            TableLayoutPanel panel = new TableLayoutPanel();
            panel.Dock = DockStyle.Fill;

            Label createdBy = new Label();
            createdBy.Text = "Created by Karlovsky120 with help of 7 Days To Die Forum members. Special thanks to Gazz!";
            createdBy.Padding = new Padding(5);
            createdBy.AutoSize = true;

            panel.Controls.Add(createdBy);
            Controls.Add(createdBy);

            ShowDialog();
        }
    }
}
