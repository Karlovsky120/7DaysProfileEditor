using SevenDaysProfileEditor.GUI;
using SevenDaysSaveManipulator.GameData;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.General
{
    internal class GeneralTab : TabPage, IInitializable
    {
        private TableLayoutPanel panel;
        private PlayerDataFile playerDataFile;

        public GeneralTab(PlayerDataFile playerDataFile)
        {
            Text = "Stats & General";

            this.playerDataFile = playerDataFile;
        }

        public void Initialize()
        {
            panel = new TableLayoutPanel();
            panel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;
            panel.AutoSize = true;
            //panel.Size = new Size(470, 414);

            StatsPanel statsPanel = new StatsPanel(playerDataFile);
            panel.SetRowSpan(statsPanel, 2);
            panel.Controls.Add(statsPanel, 0, 0);

            ScorePanel scorePanel = new ScorePanel(playerDataFile);
            panel.Controls.Add(new ScorePanel(playerDataFile), 0, 2);

            LabeledCoordinateFloat position = new LabeledCoordinateFloat("Position", playerDataFile.ecd.pos);
            panel.Controls.Add(position, 1, 0);

            LabeledCoordinateFloat rotation = new LabeledCoordinateFloat("Rotation", playerDataFile.ecd.rot);
            panel.Controls.Add(rotation, 1, 1);

            LabeledCoordinateInt home = new LabeledCoordinateInt("Home position", playerDataFile.ecd.homePosition);
            panel.Controls.Add(home, 2, 0);

            LabeledCoordinateInt backPack = new LabeledCoordinateInt("Backpack position", playerDataFile.droppedBackpackPosition);
            panel.Controls.Add(backPack, 2, 1);

            /*SchematicsRecipesPanel schematicsRecipes = new SchematicsRecipesPanel();
            schematicsRecipes.BackColor = Color.Red;
            schematicsRecipes.AutoSize = true;
            panel.Controls.Add(schematicsRecipes, 3, 0);*/

            Controls.Add(panel);
        }
    }
}