using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI {

    /// <summary>
    /// Deals with the AboutWindow.
    /// </summary>
    internal class AboutWindow : Form {

        /// <summary>
        /// Default constructor.
        /// </summary>
        public AboutWindow() {
            Text = "About";
            AutoSize = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;

            TableLayoutPanel panel = new TableLayoutPanel() {
                Dock = DockStyle.Fill
            };

            Label createdBy = new Label() {
                Font = new Font("Arial", 18, FontStyle.Bold),
                Text = "Created by Karlovsky120 with help of 7 Days To Die Forum members.\n\n" +
                       "Special thanks to:\n\n" + "Gazz, who has demystified parts of save file to me. Without him I might have never figured it out.\n" +
                       "DerPopo, without whom no icons of any type would be present. Thank you.",
                Margin = new Padding(5),
                AutoSize = true
            };

            panel.Controls.Add(createdBy);
            Controls.Add(createdBy);

            ShowDialog();
        }
    }
}