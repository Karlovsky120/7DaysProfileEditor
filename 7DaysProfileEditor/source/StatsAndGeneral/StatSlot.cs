using SevenDaysProfileEditor.GUI;
using SevenDaysSaveManipulator;
using SevenDaysSaveManipulator.SaveData;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.StatsAndGeneral {

    /// <summary>
    /// Displays a stat.
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    internal class StatSlot : TableLayoutPanel, IValueListener<float> {
        private float max;
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
        public StatSlot(Stat stat, string statName, float min, float max) {
            Dock = DockStyle.Fill;
            AutoSize = true;
            Margin = new Padding(0);

            this.stat = stat;
            this.statName = statName;
            this.min = min;
            this.max = max;

            valueBox = new NumericTextBox<float>(stat.value, min, max, 60);
            LabeledControl labeledValueBox = new LabeledControl(statName, valueBox, 190);
            stat.value.AddListener(this);

            Controls.Add(labeledValueBox);
        }

        /// <summary>
        /// Updates the display to match the data.
        /// </summary>
        /// <param name="source"></param>
        public void ValueUpdated(Value<float> source) {
            stat.originalValue.Set(stat.value.Get());
        }
    }
}