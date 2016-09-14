using SevenDaysProfileEditor.GUI;
using SevenDaysSaveManipulator.GameData;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.Stats
{
    class StatSlot : TableLayoutPanel
    {
        private Stat stat;

        private void GenerateSlot(Stat stat, string statName)
        {
            Width = 270;
            Height = 250;
            CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            Label name = new Label();
            name.Text = statName;
            name.AutoSize = true;

            Controls.Add(name, 0, 0);

            LabeledControl labeledValueBox = new LabeledControl("Value", new TextBoxFloat(stat.value, float.MinValue, float.MaxValue, 80), 200);
            Controls.Add(labeledValueBox, 0, 1);

            LabeledControl labeledOriginalValueBox = new LabeledControl("Original Value", new TextBoxFloat(stat.originalValue, float.MinValue, float.MaxValue, 80), 200);
            Controls.Add(labeledOriginalValueBox, 0, 2);

            LabeledControl labeledValueModifierBox = new LabeledControl("Value Modifier", new TextBoxFloat(stat.valueModifier, float.MinValue, float.MaxValue, 80), 200);
            Controls.Add(labeledValueModifierBox, 0, 3);

            LabeledControl labeledBaseMaxBox = new LabeledControl("Base Max", new TextBoxFloat(stat.baseMax, float.MinValue, float.MaxValue, 80), 200);
            Controls.Add(labeledBaseMaxBox, 0, 4);

            LabeledControl labeledOriginalMaxBox = new LabeledControl("Original Max", new TextBoxFloat(stat.originalMax, float.MinValue, float.MaxValue, 80), 200);
            Controls.Add(labeledOriginalMaxBox, 0, 5);

            LabeledControl labeledMaxModifierBox = new LabeledControl("Max Modifier", new TextBoxFloat(stat.maxModifier, float.MinValue, float.MaxValue, 80), 200);
            Controls.Add(labeledMaxModifierBox, 0, 6);
        }

        public StatSlot(Stat stat, string statName)
        {
            Dock = DockStyle.Fill;

            this.stat = stat;

            GenerateSlot(stat, statName);
        }
    }
}
