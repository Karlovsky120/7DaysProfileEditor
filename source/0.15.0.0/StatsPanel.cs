using SevenDaysSaveManipulator.GameData;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.General
{
    internal class StatsPanel : TableLayoutPanel
    {
        public StatsPanel(PlayerDataFile playerDataFile)
        {
            Size = new Size(204, 262);
            Margin = new Padding(0);

            EntityStats stats = playerDataFile.ecd.stats;

            StatSlot health = new StatSlot(stats.health, "Health", 0f, stats.wellness);
            Controls.Add(health, 0, 0);

            StatSlot stamina = new StatSlot(stats.stamina, "Stamina", 0f, stats.wellness);
            Controls.Add(stamina, 0, 1);

            StatSlot wellness = new StatSlot(stats.wellness, "Wellness", 70f, null);
            Controls.Add(wellness, 0, 2);

            LiveStatSlot food = new LiveStatSlot(playerDataFile.food, "Food", 72);
            Controls.Add(food, 0, 3);

            LiveStatSlot water = new LiveStatSlot(playerDataFile.drink, "Water", 72);
            Controls.Add(water, 0, 4);

            StatSlot coreTemp = new StatSlot(stats.coreTemp, "Core Temp", 0f, null);
            Controls.Add(coreTemp, 0, 5);

            StatSlot speedModifier = new StatSlot(stats.speedModifier, "Speed Modifier", 0f, null);
            Controls.Add(speedModifier, 0, 6);

            StatSlot sickness = new StatSlot(stats.sickness, "Sickness (no effect)", 0f, null);
            Controls.Add(sickness, 0, 7);

            StatSlot gassiness = new StatSlot(stats.gassiness, "Gassiness (no effect)", 0f, null);
            Controls.Add(gassiness, 0, 8);
        }
    }
}