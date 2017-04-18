using SevenDaysSaveManipulator;
using SevenDaysSaveManipulator.PlayerData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.StatsAndGeneral {

    /// <summary>
    /// Displays a recipeBinder.
    /// </summary>
    internal class LabeledSchematicRecipe : TableLayoutPanel, IValueListener<bool> {
        public bool locked = false;
        private CheckBox isUnlockedCheckBox;
        private RecipeBinder recipeBinder;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="recipeBinder">RecipeBinder to display</param>
        /// <param name="unlockedRecipesList">List of all unlocked recipes in playerDataFile</param>
        public LabeledSchematicRecipe(RecipeBinder recipeBinder, List<string> unlockedRecipesList) {
            Size = new Size(180, 20);

            this.recipeBinder = recipeBinder;
            recipeBinder.unlocked.AddListener(this);

            isUnlockedCheckBox = new CheckBox();
            isUnlockedCheckBox.Size = new Size(12, 12);
            isUnlockedCheckBox.CheckedChanged += CheckBoxChecked;
            Controls.Add(isUnlockedCheckBox, 0, 0);

            Label recipeNameLabel = new Label() {
                Text = recipeBinder.recipeData.name,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = true,
                Anchor = AnchorStyles.Left,
            };

            Controls.Add(recipeNameLabel, 1, 0);

            ValueUpdated(recipeBinder.unlocked);
        }

        /// <summary>
        /// Updates the display to match the data on data change.
        /// </summary>
        /// <param name="source"></param>
        public void ValueUpdated(Value<bool> source) {
            locked = true;
            if (source.Get()) {
                isUnlockedCheckBox.CheckState = CheckState.Checked;
            }
            else {
                isUnlockedCheckBox.CheckState = CheckState.Unchecked;
            }

            locked = false;
        }

        /// <summary>
        /// Determines whether to change the check box or not upon the request.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxChecked(object sender, EventArgs e) {
            if (!locked) {
                locked = true;
                if (isUnlockedCheckBox.CheckState == CheckState.Checked) {
                    recipeBinder.SetUnlocked(true);
                }
                else if (!recipeBinder.CheckIfShouldBeUnlocked()) {
                    recipeBinder.SetUnlocked(false);
                }
                else {
                    isUnlockedCheckBox.CheckState = CheckState.Checked;
                }

                locked = false;
            }
        }
    }
}