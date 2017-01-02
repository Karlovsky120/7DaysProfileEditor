using SevenDaysProfileEditor.GUI;
using SevenDaysSaveManipulator;
using SevenDaysSaveManipulator.GameData;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.General
{
    internal class StatSlot : TableLayoutPanel, IValueListener<float>
    {
        private Stat stat;
        private string statName;
        private float min;
        private Stat max;

        private NumericTextBox<float> valueBox;

        private void GenerateSlot()
        {
            Size = new Size(196, 28);
            Margin = new Padding(0);

            float maxValue = stat.baseMax.Get();

            if (max != null)
            {
                maxValue = max.value.Get();
                max.value.AddListener(this);
            }

            valueBox = new NumericTextBox<float>(stat.value, min, maxValue, 60);
            LabeledControl labeledValueBox = new LabeledControl(statName, valueBox, 190);
            stat.value.AddListener(this);

            Controls.Add(labeledValueBox);
        }

        public StatSlot(Stat stat, string statName, float min, Stat max)
        {
            Dock = DockStyle.Fill;

            this.stat = stat;
            this.statName = statName;
            this.min = min;
            this.max = max;

            GenerateSlot();
        }

        public void ValueUpdated(Value<float> source)
        {
            if (source == stat.value)
            {
                stat.originalValue.Set(stat.value.Get());
            }
            else if (source == max.value)
            {
                valueBox.UpdateMax(max.value.Get());
            }
        }
    }
}