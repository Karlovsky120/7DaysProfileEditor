using SevenDaysProfileEditor.GUI;
using SevenDaysSaveManipulator.PlayerData;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.StatsAndGeneral {

    /// <summary>
    /// Displays live stat.
    /// </summary>
    internal class LiveStatSlot : TableLayoutPanel {
        private LiveStats liveStat;
        private string liveStatName;
        private int max;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="liveStat">Live stat to display</param>
        /// <param name="liveStatName">Name to display</param>
        /// <param name="max">Maximum value of a live stat</param>
        public LiveStatSlot(LiveStats liveStat, string liveStatName, int max) {
            Dock = DockStyle.Fill;
            Size = new Size(196, 28);
            Margin = new Padding(0);

            this.liveStat = liveStat;
            this.liveStatName = liveStatName;
            this.max = max;

            LabeledControl labeledValueBox = new LabeledControl(liveStatName, new PercentageIntegerTextBox(liveStat.lifeLevel, max, 60), 190);
            Controls.Add(labeledValueBox);
        }
    }
}