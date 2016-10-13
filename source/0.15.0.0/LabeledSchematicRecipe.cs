using SevenDaysSaveManipulator.GameData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.General
{
    internal class LabeledSchematicRecipe : TableLayoutPanel
    {
        public LabeledSchematicRecipe(string recipeName, List<Skill> skills, List<string> learnedRecipes)
        {
            AutoSize = true;
            BackColor = Color.Blue;

            CheckBox isLearnedCheckBox = new CheckBox();
            isLearnedCheckBox.Size = new Size(12, 12);
            isLearnedCheckBox.CheckedChanged += CheckBoxChecked;
            Controls.Add(isLearnedCheckBox, 0, 0);

            Label recipeNameLabel = new Label();
            recipeNameLabel.Text = recipeName;
            recipeNameLabel.TextAlign = ContentAlignment.MiddleLeft;
            recipeNameLabel.AutoSize = true;
            recipeNameLabel.Anchor = AnchorStyles.None;
            Controls.Add(recipeNameLabel, 1, 0);
        }

        private void CheckBoxChecked(object sender, EventArgs e)
        {
        }
    }
}