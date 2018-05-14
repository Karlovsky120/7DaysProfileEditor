using SevenDaysProfileEditor.GUI;
using SevenDaysProfileEditor.StatsAndGeneral;
using SevenDaysSaveManipulator;
using SevenDaysSaveManipulator.PlayerData;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.Skills {

    /// <summary>
    /// Tab for dealing with skills.
    /// </summary>
    internal class SkillsTab : TabPage, IValueListener<int>{
        private bool locked;
        private TableLayoutPanel panel;
        private PlayerDataFile playerDataFile;
        private List<RecipeBinder> recipes;
        private List<SkillSlot> skillSlots;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="playerDataFile">PlayerDataFile to use</param>
        /// <param name="recipes">List of all the recipes</param>
        public SkillsTab(PlayerDataFile playerDataFile, List<RecipeBinder> recipes) {
            Text = "Skills";

            this.playerDataFile = playerDataFile;
            this.recipes = recipes;

            SetUpSkills();

            panel = new TableLayoutPanel();
            panel.Dock = DockStyle.Fill;

            TableLayoutPanel general = new TableLayoutPanel() {
                Size = new Size(550, 36),
                Anchor = AnchorStyles.Top
            };

            LabeledControl LabeledPlayerLevelBox = new LabeledControl("Player level", new NumericTextBox<int>(playerDataFile.level, 1, SkillData.maxPlayerLevel, 60), 150);
            playerDataFile.level.AddListener(this);
            general.Controls.Add(LabeledPlayerLevelBox, 0, 0);

            LabeledControl LabeledSkillPointsBox = new LabeledControl("Skill points", new NumericTextBox<int>(playerDataFile.skillPoints, 0, int.MaxValue, 60), 150);
            general.Controls.Add(LabeledSkillPointsBox, 1, 0);

            LabeledControl LabeledExpToNextLevelBox = new LabeledControl("Exp to next level", new NumericTextBox<uint>(playerDataFile.experience, 0u, (uint)(SkillData.expToPlayerLevel * SkillData.maxPlayerLevel), 60), 175);
            general.Controls.Add(LabeledExpToNextLevelBox, 2, 0);

            panel.Controls.Add(general);

            TableLayoutPanel skillsPanel = new TableLayoutPanel() {
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Outset,
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            skillsPanel.MouseEnter += (sender, e) => {
                bool focusFree = true;
                foreach (SkillSlot skillSlot in skillSlots) {
                    if (skillSlot.levelBox.Focused || (skillSlot.expToNextLevelBox != null && skillSlot.expToNextLevelBox.Focused)) {
                        focusFree = false;
                        break;
                    }
                }

                if (focusFree) {
                    skillsPanel.Focus();
                }
            };

            TableLayoutPanel centerer = new TableLayoutPanel() {
                Anchor = AnchorStyles.Top,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset,
                AutoSize = true
            };

            skillSlots = new List<SkillSlot>();
            int i = 0;

            foreach (KeyValuePair<int, Skill> skillEntry in playerDataFile.skills.skillDictionary) {
                SkillSlot skillSlot = new SkillSlot(new SkillBinder(skillEntry.Value, playerDataFile.level, recipes));

                skillSlots.Add(skillSlot);

                centerer.Controls.Add(skillSlot, i % 4, i / 4);

                i++;
            }

            skillsPanel.Controls.Add(centerer);
            panel.Controls.Add(skillsPanel);

            Controls.Add(panel);
        }

        /// <summary>
        /// Updates player experience based on level.
        /// </summary>
        /// <param name="source"></param>
        public void ValueUpdated(Value<int> source) {
            if (!locked) {
                locked = true;
                playerDataFile.experience.Set((uint)(SkillData.expToPlayerLevel * System.Math.Pow(SkillData.experienceMultiplier, source.Get())));
                locked = false;
            }
        }

        /// <summary>
        /// Updates player level based on experience.
        /// </summary>
        /// <param name="source"></param>

        // This does not return the correct value, complicated maths involved.
        //public void ValueUpdated(Value<uint> source) {
        //    if (!locked) {
        //        locked = true;
        //        playerDataFile.level.Set((int)source.Get() / SkillData.expToPlayerLevel);
        //        locked = false;
        //    }
        //}

        /// <summary>
        /// Sets up skills for editing.
        /// </summary>
        private void SetUpSkills() {
            SkillBinder.SetSkillDicitionary(playerDataFile);

            foreach (SkillData skillData in SkillData.skillList) {
                bool exists = false;
                int id = Utils.GetMonoHash(skillData.name);

                foreach (KeyValuePair<int, Skill> skillEntry in playerDataFile.skills.skillDictionary) {
                    if (id == skillEntry.Key) {
                        exists = true;
                    }
                }

                if (!exists) {
                    Skill skill = SkillBinder.GetEmptySkill(id, playerDataFile);
                    playerDataFile.skills.skillDictionary.Add(id, skill);
                }
            }

            SortSkills();
        }

        /// <summary>
        /// Sorts skills as in the XML.
        /// </summary>
        private void SortSkills() {
            Dictionary<int, Skill> skilldictionary = new Dictionary<int, Skill>();

            foreach (SkillData skillData in SkillData.skillList) {
                int id = Utils.GetMonoHash(skillData.name);
                Skill skill;
                playerDataFile.skills.skillDictionary.TryGetValue(id, out skill);

                skilldictionary.Add(id, skill);
            }

            playerDataFile.skills.skillDictionary = skilldictionary;
        }
    }
}