using SevenDaysProfileEditor.GUI;
using SevenDaysSaveManipulator.GameData;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.Stats
{
    class SlotStat : TableLayoutPanel
    {
        private Stat stat;

        private void generateSlot(Stat stat, string statName)
        {
            Width = 270;
            Height = 250;
            CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            Label name = new Label();
            name.Text = statName;
            name.AutoSize = true;

            Controls.Add(name);

            LabeledControl labeledValueBox = new LabeledControl("Value", new TextBoxFloat(stat.value, float.MinValue, float.MaxValue, 80), 200);
            Controls.Add(labeledValueBox);

            LabeledControl labeledOriginalValueBox = new LabeledControl("Original Value", new TextBoxFloat(stat.originalValue, float.MinValue, float.MaxValue, 80), 200);
            Controls.Add(labeledOriginalValueBox);

            LabeledControl labeledValueModifierBox = new LabeledControl("Value Modifier", new TextBoxFloat(stat.valueModifier, float.MinValue, float.MaxValue, 80), 200);
            Controls.Add(labeledValueModifierBox);

            LabeledControl labeledBaseMaxBox = new LabeledControl("Base Max", new TextBoxFloat(stat.baseMax, float.MinValue, float.MaxValue, 80), 200);
            Controls.Add(labeledBaseMaxBox);

            LabeledControl labeledOriginalMaxBox = new LabeledControl("Original Max", new TextBoxFloat(stat.originalMax, float.MinValue, float.MaxValue, 80), 200);
            Controls.Add(labeledValueBox);

            LabeledControl labeledMaxModifierBox = new LabeledControl("Max Modifier", new TextBoxFloat(stat.maxModifier, float.MinValue, float.MaxValue, 80), 200);
            Controls.Add(labeledMaxModifierBox);
        }

        public SlotStat(Stat stat, string statName) : base()
        {
            Dock = DockStyle.Fill;

            this.stat = stat;

            generateSlot(stat, statName);
        }
    }
}
