using SevenDaysProfileEditor.GUI;
using SevenDaysSaveManipulator.GameData;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.Stats
{
    internal class LiveStatSlot : TableLayoutPanel
    {
        private LiveStats liveStat;
        private string liveStatName;
        private int max;

        private void GenerateSlot()
        {
            Size = new Size(196, 28);
            Margin = new Padding(0);

            LabeledControl labeledValueBox = new LabeledControl(liveStatName, new PercentageIntegerTextBox(liveStat.lifeLevel, max, 60), 190);
            Controls.Add(labeledValueBox);
        }

        public LiveStatSlot(LiveStats liveStat, string liveStatName, int max)
        {
            Dock = DockStyle.Fill;

            this.liveStat = liveStat;
            this.liveStatName = liveStatName;
            this.max = max;

            GenerateSlot();
        }
    }
}