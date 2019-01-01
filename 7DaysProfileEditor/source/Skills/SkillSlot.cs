using SevenDaysProfileEditor.GUI;
using SevenDaysProfileEditor.StatsAndGeneral;
using SevenDaysSaveManipulator;
using SevenDaysSaveManipulator.SaveData;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.Skills {

    /// <summary>
    /// Displays skill
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    internal class SkillSlot : TableLayoutPanel, IValueListener<int> {
        public TextBox expToNextLevelBox;
        public NumericTextBox<int> levelBox;
        private Label requirementTipsLabel;
        private SkillBinder skillBinder;

        /// <summary>
        /// Default contructor.
        /// </summary>
        /// <param name="skillBinder">Skill binder to display</param>
        public SkillSlot(SkillBinder skillBinder) {
            this.skillBinder = skillBinder;

            GenerateSlot();
        }

        /// <summary>
        /// Generates the slot.
        /// </summary>
        public void GenerateSlot() {
            Size = new Size(210, 210);

            TableLayoutPanel title = new TableLayoutPanel() {
                Anchor = AnchorStyles.Top,
                MaximumSize = new Size(int.MaxValue, 94),
                AutoSize = true
            };

            Label imageLabel = new Label() {
                Size = new Size(skillBinder.skillData.iconData.width, skillBinder.skillData.iconData.height),
                Anchor = AnchorStyles.Top,
                Text = "",
                BackgroundImage = null
            };

            Bitmap image = skillBinder.GetImage();

            if (image != null) {
                imageLabel.BackgroundImage = image;
            }

            title.Controls.Add(imageLabel, 0, 0);

            Label name = new Label() {
                TextAlign = ContentAlignment.MiddleLeft,
                Text = skillBinder.skillData.name,
                Anchor = AnchorStyles.None,
                AutoSize = true
            };

            title.Controls.Add(name, 0, 1);

            Controls.Add(title, 0, 0);

            int min = 1;

            if (skillBinder.skillData.type == SkillType.Perk) {
                min = 0;
            }

            levelBox = new NumericTextBox<int>(skillBinder.skill.level, min, skillBinder.GetHighestUnlockedLevel(), 80);
            LabeledControl labeledLevelBox = new LabeledControl("Level", levelBox, 180);
            Controls.Add(labeledLevelBox, 0, 1);

            skillBinder.skill.level.AddListener(this);

            if (skillBinder.skillData.type != SkillType.Perk) {
                expToNextLevelBox = new NumericTextBox<int>(skillBinder.skill.expToNextLevel, 0, skillBinder.skillData.expToLevel, 80);
                LabeledControl labeledExpToNextLevelBox = new LabeledControl("Exp to next level", expToNextLevelBox, 180);
                Controls.Add(labeledExpToNextLevelBox, 0, 2);
            }
            else if (skillBinder.skillData.requirements.Count > 0) {
                RegisterListeners();

                if (skillBinder.GetHighestUnlockedLevel() != skillBinder.skillData.maxLevel) {
                    Controls.Add(GetRequirementTipsLabel(), 0, 3);
                }
            }
        }

        /// <summary>
        /// Updates recipes and display on skill level update.
        /// </summary>
        /// <param name="source"></param>
        public void ValueUpdated(Value<int> source) {
            if (source == skillBinder.skill.level) {
                if (expToNextLevelBox != null) {
                    skillBinder.skill.expToNextLevel.Set(skillBinder.skillData.expToLevel);
                }

                foreach (RecipeBinder recipeBinder in skillBinder.unlockedRecipeList) {
                    bool recipeSkillUnlockState = recipeBinder.CheckIfShouldBeUnlocked();

                    if (recipeSkillUnlockState || !recipeBinder.recipeData.hasSchematic) {
                        recipeBinder.SetUnlocked(recipeSkillUnlockState);
                    }
                }
            }
            else {
                levelBox.UpdateMax(skillBinder.GetHighestUnlockedLevel());

                Controls.Remove(requirementTipsLabel);
                requirementTipsLabel = GetRequirementTipsLabel();
                Controls.Add(requirementTipsLabel);
            }
        }

        /// <summary>
        /// Generates requirement tip.
        /// </summary>
        /// <param name="requirement">Requirement to generate the tip for</param>
        /// <returns>Requirement tip</returns>
        private string FormulateRequirementTip(Requirement requirement) {
            return string.Format("\n- {0} level {1}", requirement.requiredSkillName, requirement.requiredSkillLevel);
        }

        /// <summary>
        /// Creates a label containing all the requirements.
        /// </summary>
        /// <returns>Label containing all the requirements</returns>
        private Label GetRequirementTipsLabel() {
            requirementTipsLabel = new Label();
            requirementTipsLabel.TextAlign = ContentAlignment.MiddleLeft;
            requirementTipsLabel.AutoSize = true;

            int unlockedLevel = skillBinder.GetHighestUnlockedLevel();

            if (unlockedLevel != skillBinder.skillData.maxLevel) {
                requirementTipsLabel.Text = string.Format("Requirements to unlock level {0}:", unlockedLevel + 1);

                List<Requirement> requirementsForNextLevel = skillBinder.GetRequirementByLevel(unlockedLevel + 1);

                List<string> requirementTips = new List<string>();

                foreach (Requirement requirement in requirementsForNextLevel) {
                    if (!requirement.IsRequirementMet()) {
                        requirementTips.Add(FormulateRequirementTip(requirement));
                    }
                }

                foreach (string requirementTip in requirementTips) {
                    requirementTipsLabel.Text += requirementTip;
                }
            }

            return requirementTipsLabel;
        }

        /// <summary>
        /// Adds skill references to requirements and adds this as a listener to all required skills.
        /// </summary>
        private void RegisterListeners() {
            List<Value<int>> requiredSkills = new List<Value<int>>();

            foreach (Requirement requirement in skillBinder.skillData.requirements) {
                Skill requiredSkill;

                if (requirement.requiredSkillName.Equals("Player Level")) {
                    if (!requiredSkills.Contains(skillBinder.playerLevel)) {
                        requiredSkills.Add(skillBinder.playerLevel);
                    }
                }
                else if (SkillBinder.skillDictionary.TryGetValue(Utils.GetMonoHash(requirement.requiredSkillName), out requiredSkill)) {
                    if (!requiredSkills.Contains(requiredSkill.level)) {
                        requiredSkills.Add(requiredSkill.level);
                    }
                }
            }

            foreach (Value<int> skillLevel in requiredSkills) {
                skillLevel.AddListener(this);
            }
        }
    }
}