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
            Width = 250;
            Height = 250;
            CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            Label name = new Label();
            name.Text = statName;
            name.AutoSize = true;

            Controls.Add(name);

            LabeledBox labeledValueBox = new LabeledBox("Value", new TextBoxFloat(stat.value, float.MinValue, float.MaxValue));
            Controls.Add(labeledValueBox);

            LabeledBox labeledOriginalValueBox = new LabeledBox("Original Value", new TextBoxFloat(stat.originalValue, float.MinValue, float.MaxValue));
            Controls.Add(labeledOriginalValueBox);

            LabeledBox labeledValueModifierBox = new LabeledBox("Value Modifier", new TextBoxFloat(stat.valueModifier, float.MinValue, float.MaxValue));
            Controls.Add(labeledValueModifierBox);

            LabeledBox labeledBaseMaxBox = new LabeledBox("Base Max", new TextBoxFloat(stat.baseMax, float.MinValue, float.MaxValue));
            Controls.Add(labeledBaseMaxBox);

            LabeledBox labeledOriginalMaxBox = new LabeledBox("Original Max", new TextBoxFloat(stat.originalMax, float.MinValue, float.MaxValue));
            Controls.Add(labeledValueBox);

            LabeledBox labeledMaxModifierBox = new LabeledBox("Max Modifier", new TextBoxFloat(stat.maxModifier, float.MinValue, float.MaxValue));
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
