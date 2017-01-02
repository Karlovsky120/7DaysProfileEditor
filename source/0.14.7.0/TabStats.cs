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
    class TabStats : TabPage
    {
        private PlayerDataFile playerDataFile;
        private TableLayoutPanel panel;
        private EntityStats stats;

        public TabStats(PlayerDataFile playerDataFile) : base()
        {
            this.playerDataFile = playerDataFile;
            Text = "Stats";

            stats = playerDataFile.ecd.stats;

            panel = new TableLayoutPanel();

            panel.Dock = DockStyle.Fill;

            SlotStat health = new SlotStat(stats.health, "Health");
            panel.Controls.Add(health, 0, 0);

            SlotStat stamina = new SlotStat(stats.stamina, "Stamina");
            panel.Controls.Add(stamina, 0, 1);

            SlotStat sickness = new SlotStat(stats.sickness, "Sickness");
            panel.Controls.Add(sickness, 0, 2);

            SlotStat gassiness = new SlotStat(stats.gassiness, "Gassiness");
            panel.Controls.Add(gassiness, 1, 0);

            SlotStat speedModifier = new SlotStat(stats.speedModifier, "Speed Modifier");
            panel.Controls.Add(speedModifier, 1, 1);

            SlotStat wellness = new SlotStat(stats.wellness, "Wellness");
            panel.Controls.Add(wellness, 1, 2);

            SlotStat coreTemp = new SlotStat(stats.coreTemp, "Core Temp");
            panel.Controls.Add(coreTemp, 2, 0);

            SlotStat food = new SlotStat(stats.food, "Food");
            panel.Controls.Add(food, 2, 1);

            SlotStat water = new SlotStat(stats.water, "Water");
            panel.Controls.Add(water, 2, 2);

            Controls.Add(panel);
        }
    }
}
