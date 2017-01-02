using SevenDaysProfileEditor.GUI;
using SevenDaysSaveManipulator;
using SevenDaysSaveManipulator.GameData;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.Skills
{
    internal class SkillsTab : TabPage, IInitializable, IValueListener<int>, IValueListener<uint>
    {
        private PlayerDataFile playerDataFile;
        private TableLayoutPanel panel;

        private List<SkillSlot> skillSlots;

        private bool locked;

        public SkillsTab(PlayerDataFile playerDataFile)
        {
            Text = "Skills";

            this.playerDataFile = playerDataFile;
        }

        public void Initialize()
        {
            SetUpSkills();

            panel = new TableLayoutPanel();
            panel.Dock = DockStyle.Fill;

            TableLayoutPanel general = new TableLayoutPanel();
            general.Size = new Size(478, 36);
            general.Anchor = AnchorStyles.Top;

            LabeledControl LabeledPlayerLevelBox = new LabeledControl("Player level", new NumericTextBox<int>(playerDataFile.level, 1, SkillData.maxPlayerLevel, 80), 150);
            playerDataFile.level.AddListener(this);
            general.Controls.Add(LabeledPlayerLevelBox, 0, 0);

            LabeledControl LabeledSkillPointsBox = new LabeledControl("Skill points", new NumericTextBox<int>(playerDataFile.skillPoints, 0, int.MaxValue, 80), 150);
            general.Controls.Add(LabeledSkillPointsBox, 1, 0);

            LabeledControl LabeledExperienceBox = new LabeledControl("Experience", new NumericTextBox<uint>(playerDataFile.experience, 0u, (uint)(SkillData.expToPlayerLevel * SkillData.maxPlayerLevel), 80), 150);
            playerDataFile.experience.AddListener(this);
            general.Controls.Add(LabeledExperienceBox, 2, 0);

            panel.Controls.Add(general);

            TableLayoutPanel skillsPanel = new TableLayoutPanel();
            skillsPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Outset;
            skillsPanel.Dock = DockStyle.Fill;
            skillsPanel.AutoScroll = true;
            skillsPanel.MouseEnter += (sender, e) =>
            {
                bool focusFree = true;
                foreach (SkillSlot skillSlot in skillSlots)
                {
                    if (skillSlot.levelBox.Focused || (skillSlot.expToNextLevelBox != null && skillSlot.expToNextLevelBox.Focused))
                    {
                        focusFree = false;
                        break;
                    }
                }

                if (focusFree)
                {
                    skillsPanel.Focus();
                }
            };

            TableLayoutPanel centerer = new TableLayoutPanel();
            centerer.Anchor = AnchorStyles.Top;
            centerer.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;
            centerer.AutoSize = true;

            skillSlots = new List<SkillSlot>();
            int i = 0;

            foreach (KeyValuePair<int, Skill> skillEntry in playerDataFile.skills.skillDictionary)
            {
                SkillSlot skillSlot = new SkillSlot(new SkillBinder(skillEntry.Value, playerDataFile.level, playerDataFile.unlockedRecipeList));

                skillSlots.Add(skillSlot);

                centerer.Controls.Add(skillSlot, i % 4, i / 4);

                i++;
            }

            skillsPanel.Controls.Add(centerer);
            panel.Controls.Add(skillsPanel);

            Controls.Add(panel);
        }

        private void SetUpSkills()
        {
            SkillBinder.SetSkillDicitionary(playerDataFile);

            foreach (SkillData skillData in SkillData.skillList)
            {
                bool exists = false;
                int id = Utils.GetMonoHash(skillData.name);

                foreach (KeyValuePair<int, Skill> skillEntry in playerDataFile.skills.skillDictionary)
                {
                    if (id == skillEntry.Key)
                    {
                        exists = true;
                    }
                }

                if (!exists)
                {
                    Skill skill = SkillBinder.GetEmptySkill(id, playerDataFile);
                    playerDataFile.skills.skillDictionary.Add(id, skill);
                }
            }

            SortSkills();
        }

        private void SortSkills()
        {
            Dictionary<int, Skill> skilldictionary = new Dictionary<int, Skill>();

            foreach (SkillData skillData in SkillData.skillList)
            {
                int id = Utils.GetMonoHash(skillData.name);
                Skill skill;
                playerDataFile.skills.skillDictionary.TryGetValue(id, out skill);

                skilldictionary.Add(id, skill);
            }

            playerDataFile.skills.skillDictionary = skilldictionary;
        }

        public void ValueUpdated(Value<int> source)
        {
            if (!locked)
            {
                locked = true;
                playerDataFile.experience.Set((uint)(source.Get() * SkillData.expToPlayerLevel));
                locked = false;
            }
        }

        public void ValueUpdated(Value<uint> source)
        {
            if (!locked)
            {
                locked = true;
                playerDataFile.level.Set((int)source.Get() / SkillData.expToPlayerLevel);
                locked = false;
            }
        }
    }
}