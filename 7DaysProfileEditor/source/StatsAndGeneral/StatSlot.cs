using SevenDaysProfileEditor.GUI;
using SevenDaysSaveManipulator;
using SevenDaysSaveManipulator.PlayerData;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.StatsAndGeneral {

    /// <summary>
    /// Displays a stat.
    /// </summary>
    internal class StatSlot : TableLayoutPanel, IValueListener<float> {
        private Stat max;
        private float min;
        private Stat stat;
        private string statName;
        private NumericTextBox<float> valueBox;

        /// <summary>
        /// Default construcotor
        /// </summary>
        /// <param name="stat">Stat to display</param>
        /// <param name="statName">Stat name to display</param>
        /// <param name="min">Min allowed value of the stat</param>
        /// <param name="max">Max allowed value of the stat</param>
        public StatSlot(Stat stat, string statName, float min, Stat max) {
            Dock = DockStyle.Fill;
            Size = new Size(196, 28);
            Margin = new Padding(0);

            this.stat = stat;
            this.statName = statName;
            this.min = min;
            this.max = max;

            float maxValue = stat.baseMax.Get();

            if (max != null) {
                maxValue = max.value.Get();
                max.value.AddListener(this);
            }

            valueBox = new NumericTextBox<float>(stat.value, min, maxValue, 60);
            LabeledControl labeledValueBox = new LabeledControl(statName, valueBox, 190);
            stat.value.AddListener(this);

            Controls.Add(labeledValueBox);
        }

        /// <summary>
        /// Updates the display to match the data.
        /// </summary>
        /// <param name="source"></param>
        public void ValueUpdated(Value<float> source) {
            if (source == stat.value) {
                stat.originalValue.Set(stat.value.Get());
            }
            else if (source == max.value) {
                valueBox.UpdateMax(max.value.Get());
            }
        }
    }
}