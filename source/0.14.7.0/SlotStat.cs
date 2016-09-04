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
    class SlotStat : TableLayoutPanel
    {
        private Stat stat;

        public void generateSlot(Stat stat, string statName)
        {
            Width = 250;
            Height = 250;
            CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            Label name = new Label();
            name.Text = statName;
            name.AutoSize = true;

            Controls.Add(name);

            TextBoxFloat valueBox = new TextBoxFloat(stat.value, float.MinValue, float.MaxValue);
            LabeledBox labeledValueBox = new LabeledBox("Value", valueBox);

            Controls.Add(labeledValueBox);

            TextBoxFloat originalValueBox = new TextBoxFloat(stat.originalValue, float.MinValue, float.MaxValue);
            LabeledBox labeledOriginalValueBox = new LabeledBox("Original Value", originalValueBox);

            Controls.Add(labeledOriginalValueBox);

            TextBoxFloat valueModifierBox = new TextBoxFloat(stat.valueModifier, float.MinValue, float.MaxValue);
            LabeledBox labeledValueModifierBox = new LabeledBox("Value Modifier", valueModifierBox);

            Controls.Add(labeledValueModifierBox);

            TextBoxFloat baseMaxBox = new TextBoxFloat(stat.baseMax, float.MinValue, float.MaxValue);
            LabeledBox labeledBaseMaxBox = new LabeledBox("Base Max", baseMaxBox);

            Controls.Add(labeledBaseMaxBox);

            TextBoxFloat originalMaxBox = new TextBoxFloat(stat.originalMax, float.MinValue, float.MaxValue);
            LabeledBox labeledOriginalMaxBox = new LabeledBox("Original Max", originalMaxBox);

            Controls.Add(labeledValueBox);

            TextBoxFloat maxModifierBox = new TextBoxFloat(stat.maxModifier, float.MinValue, float.MaxValue);
            LabeledBox labeledMaxModifierBox = new LabeledBox("Max Modifier", maxModifierBox);

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
