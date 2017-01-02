using SevenDaysProfileEditor.GUI;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.Skills
{
    class SlotSkill : TableLayoutPanel
    {
        private BinderSkill binderSkill;

        public void update()
        {
            int controlNumber = Controls.Count;
            for (int i = 0; i < controlNumber; i++)
            {
                Controls.RemoveAt(0);
            }

            generateSlot(binderSkill);

            Invalidate();
        }

        public void generateSlot(BinderSkill binderSkill)
        {
            Width = 250;
            Height = 80;
            CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            Label name = new Label();
            name.Text = binderSkill.name;
            name.AutoSize = true;

            Controls.Add(name);

            int min = 1;

            if (binderSkill.type == SkillType.Perk)
            {
                min = 0;
            }

            LabeledBox levelBox = new LabeledBox("Level", new TextBoxLevel(binderSkill.level, min, binderSkill.maxLevelUnlocked, binderSkill));
            Controls.Add(levelBox);

            if (binderSkill.requirements != null)
            {
                string skill;
                int level;

                if (binderSkill.getNextRequirement(out skill, out level))
                {
                    Label requirement = new Label();
                    requirement.Text = "Required for next level: " + skill + " level " + level;
                    requirement.AutoSize = true;

                    Controls.Add(requirement);
                }

                else
                {
                    Label placeholder = new Label();
                    Controls.Add(placeholder);
                }
            }

            else if (binderSkill.type != SkillType.Perk)
            {
                LabeledBox expToNextLevelBox = new LabeledBox("Exp to next level", new TextBoxInt(binderSkill.expToNextLevel, 0, binderSkill.expToLevel));
                Controls.Add(expToNextLevelBox);                
            }
        }
        public SlotSkill(BinderSkill binderSkill) : base()
        {
            Dock = DockStyle.Fill;

            this.binderSkill = binderSkill;
            binderSkill.skillSlot = this;

            generateSlot(binderSkill);
        }
    }
}
