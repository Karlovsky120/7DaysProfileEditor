using SevenDaysProfileEditor.Inventory;
using SevenDaysProfileEditor.Skills;
using SevenDaysProfileEditor.StatsAndGeneral;
using SevenDaysSaveManipulator.GameData;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI {

    /// <summary>
    /// Holds and shows info about a ttp.
    /// </summary>
    internal class PlayerTab : TabPage {
        public string fileName;
        public string path;
        public PlayerDataFile playerDataFile;
        public List<RecipeBinder> recipes = new List<RecipeBinder>();

        public TabControl ttpFileTabControl;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="playerDataFile">PlayerDataFile to be used</param>
        /// <param name="path">Path to the file used</param>
        /// <param name="selectedIndex">Index to show of the sub-tab</param>
        public PlayerTab(PlayerDataFile playerDataFile, string path, int selectedIndex) {
            this.path = path;
            fileName = path.Substring(path.LastIndexOf('\\') + 1);

            this.playerDataFile = playerDataFile;
            Text = playerDataFile.ecd.entityName.Get() + "          ";

            ttpFileTabControl = new TabControl();
            ttpFileTabControl.Dock = DockStyle.Fill;

            foreach (RecipeData recipeData in RecipeData.recipeList) {
                RecipeBinder recipeBinder = new RecipeBinder(recipeData, playerDataFile.unlockedRecipeList);

                recipes.Add(recipeBinder);
            }

            ttpFileTabControl.Controls.Add(new StatsAndGeneralTab(playerDataFile, recipes));
            ttpFileTabControl.Controls.Add(new InventoryTab(playerDataFile));
            ttpFileTabControl.Controls.Add(new SkillsTab(playerDataFile, recipes));

            Controls.Add(ttpFileTabControl);
        }
    }
}