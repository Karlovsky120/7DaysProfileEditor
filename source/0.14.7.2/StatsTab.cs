using SevenDaysProfileEditor.GUI;
using SevenDaysSaveManipulator.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.Stats
{
    class StatsTab : TabPage, IInitializable
    {
        private PlayerDataFile playerDataFile;
        private TableLayoutPanel panel;
        private EntityStats stats;

        public StatsTab(PlayerDataFile playerDataFile)
        {
            Text = "Stats";

            this.playerDataFile = playerDataFile;
        }

        public void Initialize()
        {
            stats = playerDataFile.ecd.stats;

            panel = new TableLayoutPanel();

            panel.Dock = DockStyle.Fill;

            StatSlot health = new StatSlot(stats.health, "Health");
            panel.Controls.Add(health, 0, 0);

            StatSlot stamina = new StatSlot(stats.stamina, "Stamina");
            panel.Controls.Add(stamina, 0, 1);

            StatSlot sickness = new StatSlot(stats.sickness, "Sickness");
            panel.Controls.Add(sickness, 0, 2);

            StatSlot gassiness = new StatSlot(stats.gassiness, "Gassiness");
            panel.Controls.Add(gassiness, 1, 0);

            StatSlot speedModifier = new StatSlot(stats.speedModifier, "Speed Modifier");
            panel.Controls.Add(speedModifier, 1, 1);

            StatSlot wellness = new StatSlot(stats.wellness, "Wellness");
            panel.Controls.Add(wellness, 1, 2);

            StatSlot coreTemp = new StatSlot(stats.coreTemp, "Core Temp");
            panel.Controls.Add(coreTemp, 2, 0);

            StatSlot food = new StatSlot(stats.food, "Food");
            panel.Controls.Add(food, 2, 1);

            StatSlot water = new StatSlot(stats.water, "Water");
            panel.Controls.Add(water, 2, 2);

            Controls.Add(panel);
        }
    }
}
