using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.StatsAndGeneral {

    /// <summary>
    /// Displays recipes that can be unlocked with a schematic.
    /// </summary>
    internal class SchematicsRecipesPanel : TableLayoutPanel {

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="recipeBinderList">List of recipeBInders</param>
        /// <param name="unlockedRecipeList">List of unlocked recipes from playerDataFile</param>
        public SchematicsRecipesPanel(List<RecipeBinder> recipeBinderList, List<string> unlockedRecipeList) {
            Size = new Size(206, 260);
            Anchor = AnchorStyles.None;
            Margin = new Padding(0);

            Label knownRecipes = new Label() {
                Text = "Known recipes",
                TextAlign = ContentAlignment.MiddleLeft
            };

            Controls.Add(knownRecipes, 0, 0);

            TableLayoutPanel recipePanel = new TableLayoutPanel() {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 3, 0, 0),
                AutoScroll = true
            };

            foreach (RecipeBinder recipeBinder in recipeBinderList) {
                if (recipeBinder.recipeData.hasSchematic) {
                    recipePanel.Controls.Add(new LabeledSchematicRecipe(recipeBinder, unlockedRecipeList));
                }
            }

            Controls.Add(recipePanel, 0, 1);
        }
    }
}