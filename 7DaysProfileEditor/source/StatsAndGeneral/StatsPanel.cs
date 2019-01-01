using SevenDaysSaveManipulator.SaveData;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.StatsAndGeneral {

    /// <summary>
    /// Displays the stats.
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    internal class StatsPanel : TableLayoutPanel {

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="playerDataFile">PlayerDataFile to be used</param>
        public StatsPanel(PlayerDataFile playerDataFile) {
            AutoSize = true;
            Margin = new Padding(0);

            Label nameLabel = new Label() {
                Text = "Stats",
                AutoSize = true
            };

            Controls.Add(nameLabel, 0, 0);

            TableLayoutPanel statsSubPanel = new TableLayoutPanel() {
                AutoSize = true
            };

            EntityStats stats = playerDataFile.ecd.stats;

            StatSlot health = new StatSlot(stats.health, "Health", 0f, 100f);
            statsSubPanel.Controls.Add(health, 0, 0);

            StatSlot stamina = new StatSlot(stats.stamina, "Stamina", 0f, 100f);
            statsSubPanel.Controls.Add(stamina, 0, 1);

            StatSlot water = new StatSlot(stats.water, "Water", 0, 100f);
            statsSubPanel.Controls.Add(water, 0, 4);

            StatSlot coreTemp = new StatSlot(stats.coreTemp, "Core Temp", -200f, 200f);
            statsSubPanel.Controls.Add(coreTemp, 0, 5);

            Controls.Add(statsSubPanel, 0, 1);
        }
    }
}