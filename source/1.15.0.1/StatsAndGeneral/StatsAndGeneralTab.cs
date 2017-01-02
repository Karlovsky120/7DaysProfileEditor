using SevenDaysProfileEditor.GUI;
using SevenDaysSaveManipulator.GameData;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.StatsAndGeneral {

    /// <summary>
    /// Tab for dealing with stats and general stuff.
    /// </summary>
    internal class StatsAndGeneralTab : TabPage {
        private TableLayoutPanel panel;
        private PlayerDataFile playerDataFile;
        private List<RecipeBinder> recipes;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="playerDataFile">PlayerDataFile to be used</param>
        /// <param name="recipes">List of recipeBinders</param>
        public StatsAndGeneralTab(PlayerDataFile playerDataFile, List<RecipeBinder> recipes) {
            Text = "Stats & General";

            this.playerDataFile = playerDataFile;
            this.recipes = recipes;

            panel = new TableLayoutPanel();
            panel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;
            panel.Size = new Size(676, 414);

            StatsPanel statsPanel = new StatsPanel(playerDataFile);
            panel.SetRowSpan(statsPanel, 2);
            panel.Controls.Add(statsPanel, 0, 0);

            ScorePanel scorePanel = new ScorePanel(playerDataFile);
            panel.Controls.Add(new ScorePanel(playerDataFile), 0, 2);

            LabeledCoordinate<float> position = new LabeledCoordinate<float>("Position", playerDataFile.ecd.pos, float.MinValue, float.MaxValue);
            panel.Controls.Add(position, 1, 0);

            LabeledCoordinate<float> rotation = new LabeledCoordinate<float>("Rotation", playerDataFile.ecd.rot, float.MinValue, float.MaxValue);
            panel.Controls.Add(rotation, 1, 1);

            LabeledCoordinate<int> home = new LabeledCoordinate<int>("Home position", playerDataFile.ecd.homePosition, int.MinValue, int.MaxValue);
            panel.Controls.Add(home, 2, 0);

            LabeledCoordinate<int> backPack = new LabeledCoordinate<int>("Backpack position", playerDataFile.droppedBackpackPosition, int.MinValue, int.MaxValue);
            panel.Controls.Add(backPack, 2, 1);

            SchematicsRecipesPanel schematicsRecipes = new SchematicsRecipesPanel(recipes, playerDataFile.unlockedRecipeList);
            panel.SetRowSpan(schematicsRecipes, 2);
            panel.Controls.Add(schematicsRecipes, 3, 0);

            Controls.Add(panel);
        }
    }
}