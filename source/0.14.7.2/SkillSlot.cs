using SevenDaysProfileEditor.GUI;
using SevenDaysSaveManipulator;
using SevenDaysSaveManipulator.GameData;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.Skills
{
    class SkillSlot : TableLayoutPanel, IValueListener<int>
    {
        private SkillBinder skillBinder;

        public TextBoxInt levelBox;
        public TextBox expToNextLevelBox;

        private Label requirementTipsLabel;

        public void generateSlot(SkillBinder skillBinder)
        {
            Size = new Size(210, 210);

            TableLayoutPanel title = new TableLayoutPanel();
            title.Anchor = AnchorStyles.Top;
            title.MaximumSize = new Size(int.MaxValue, 94);
            title.AutoSize = true;

            Label imageLabel = new Label();
            imageLabel.Size = new Size(skillBinder.iconData.width, skillBinder.iconData.height);
            imageLabel.Anchor = AnchorStyles.Top;
            imageLabel.Text = "";
            imageLabel.BackgroundImage = null;

            Bitmap image = skillBinder.GetImage();

            if (image != null)
            {
                imageLabel.BackgroundImage = image;
            }

            title.Controls.Add(imageLabel, 0, 0);

            Label name = new Label();
            name.TextAlign = ContentAlignment.MiddleLeft;
            name.Text = skillBinder.name;
            name.Anchor = AnchorStyles.None;
            name.AutoSize = true;

            title.Controls.Add(name, 0, 1);

            Controls.Add(title, 0, 0);

            int min = 1;

            if (skillBinder.type == SkillType.Perk)
            {
                min = 0;
            }

            levelBox = new TextBoxInt(skillBinder.level, min, skillBinder.GetHighestUnlockedLevel(), 80);
            LabeledControl labeledLevelBox = new LabeledControl("Level", levelBox, 180);
            Controls.Add(labeledLevelBox, 0, 1);

            skillBinder.level.AddListener(this);

            if (skillBinder.type != SkillType.Perk)
            {
                expToNextLevelBox = new TextBoxInt(skillBinder.expToNextLevel, 0, skillBinder.expToLevel, 80);
                LabeledControl labeledExpToNextLevelBox = new LabeledControl("Exp to next level", expToNextLevelBox, 180);
                Controls.Add(labeledExpToNextLevelBox, 0, 2);
            }

            else if (skillBinder.requirements.Count > 0)
            {
                RegisterListeners();

                if (skillBinder.GetHighestUnlockedLevel() != skillBinder.maxLevel)
                {
                    Controls.Add(GetRequirementTipsLabel(), 0, 3); 
                }                              
            }
        }

        private Label GetRequirementTipsLabel()
        {
            requirementTipsLabel = new Label();
            requirementTipsLabel.TextAlign = ContentAlignment.MiddleLeft;
            requirementTipsLabel.AutoSize = true;

            int unlockedLevel = skillBinder.GetHighestUnlockedLevel();

            if (unlockedLevel != skillBinder.maxLevel)
            {
                requirementTipsLabel.Text = "Requirements to unlock level " + (unlockedLevel + 1) + ":";

                List<Requirement> requirementsForNextLevel = skillBinder.GetRequirementByLevel(unlockedLevel + 1);

                List<string> requirementTips = new List<string>();

                foreach (Requirement requirement in requirementsForNextLevel)
                {
                    if (!requirement.IsRequirementSatisfied())
                    {
                        requirementTips.Add(FormulateRequirementTip(requirement));
                    }
                }

                foreach (string requirementTip in requirementTips)
                {
                    requirementTipsLabel.Text += requirementTip;
                }                
            }

            return requirementTipsLabel;
        }

        private string FormulateRequirementTip(Requirement requirement)
        {
            return "\n- " + requirement.requiredSkillName + " level " + requirement.requiredSkillLevel;
        }

        private void RegisterListeners()
        {
            List<Value<int>> requiredSkills = new List<Value<int>>();

            foreach (Requirement requirement in skillBinder.requirements)
            {
                Skill requiredSkill;

                if (requirement.requiredSkillName.Equals("Player Level"))
                {
                    if (!requiredSkills.Contains(skillBinder.playerLevel))
                    {
                        requiredSkills.Add(skillBinder.playerLevel);
                    }
                }

                else if (SkillBinder.skillDictionary.TryGetValue(Utils.GetMonoHash(requirement.requiredSkillName), out requiredSkill))
                {
                    if (!requiredSkills.Contains(requiredSkill.level))
                    {
                        requiredSkills.Add(requiredSkill.level);
                    }
                }
            }

            foreach (Value<int> skillLevel in requiredSkills)
            {
                skillLevel.AddListener(this);
            }
        }

        public SkillSlot(SkillBinder skillBinder)
        {
            this.skillBinder = skillBinder;

            generateSlot(skillBinder);
        }

        public void ValueUpdated(Value<int> source)
        {
            if (source == skillBinder.level)
            {
                if (expToNextLevelBox != null)
                {
                    skillBinder.expToNextLevel.Set(skillBinder.expToLevel);
                }

                foreach (KeyValuePair<string, int> recipe in skillBinder.recipes)
                {
                    if (recipe.Value >= skillBinder.level.Get())
                    {
                        if (!skillBinder.unlockedRecipeList.Contains(recipe.Key))
                        {
                            skillBinder.unlockedRecipeList.Add(recipe.Key);
                        }
                    }

                    else
                    {
                        if (skillBinder.unlockedRecipeList.Contains(recipe.Key))
                        {
                            skillBinder.unlockedRecipeList.Remove(recipe.Key);
                        }
                    }
                }
            }

            else
            {
                levelBox.updateMax(skillBinder.GetHighestUnlockedLevel());

                Controls.Remove(requirementTipsLabel);
                requirementTipsLabel = GetRequirementTipsLabel();
                Controls.Add(requirementTipsLabel);
            }
        }
    }
}