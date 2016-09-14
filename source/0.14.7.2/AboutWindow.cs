using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI
{
    class AboutWindow : Form
    {
        public AboutWindow()
        {
            Text = "About";
            AutoSize = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;

            TableLayoutPanel panel = new TableLayoutPanel();
            panel.Dock = DockStyle.Fill;

            Label createdBy = new Label();
            createdBy.Font = new Font("Arial", 18, FontStyle.Bold);
            createdBy.Text = "Created by Karlovsky120 with help of 7 Days To Die Forum members.\n\n" +
                "Special thanks to:\n\n" +
                "Gazz, who has demystified parts of save file to me. Without him I might have never figured it out.\n" +
                "DerPopo, without whom no icons of any type would be present. Thank you.";
            createdBy.Margin = new Padding(5);
            createdBy.AutoSize = true;

            panel.Controls.Add(createdBy);
            Controls.Add(createdBy);

            ShowDialog();
        }
    }
}
