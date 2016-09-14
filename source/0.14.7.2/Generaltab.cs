using SevenDaysProfileEditor.GUI;
using SevenDaysSaveManipulator.GameData;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.General
{
    class GeneralTab : TabPage, IInitializable
    {
        private PlayerDataFile playerDataFile;
        private TableLayoutPanel panel;

        public GeneralTab(PlayerDataFile playerDataFile)
        {
            Text = "General";   

            this.playerDataFile = playerDataFile;               
        }

        public void Initialize()
        {
            panel = new TableLayoutPanel();
            panel.Dock = DockStyle.Fill;
            panel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            panel.AutoScroll = true;
            panel.AutoSize = true;

            LabeledControl score = new LabeledControl("Score", new TextBoxInt(playerDataFile.score, 0, int.MaxValue, 80), 200);
            panel.Controls.Add(score);

            LabeledControl playerKills = new LabeledControl("Player kills", new TextBoxInt(playerDataFile.playerKills, 0, int.MaxValue, 80), 200);
            panel.Controls.Add(playerKills);

            LabeledControl zombieKills = new LabeledControl("Zombie kills", new TextBoxInt(playerDataFile.zombieKills, 0, int.MaxValue, 80), 200);
            panel.Controls.Add(zombieKills);

            LabeledControl deaths = new LabeledControl("Deaths", new TextBoxInt(playerDataFile.deaths, 0, int.MaxValue, 80), 200);
            panel.Controls.Add(deaths);


            LabeledControl posX = new LabeledControl("Position x", new TextBoxFloat(playerDataFile.ecd.pos.x, float.MinValue, float.MaxValue, 80), 200);
            panel.Controls.Add(posX);

            LabeledControl posY = new LabeledControl("Position y", new TextBoxFloat(playerDataFile.ecd.pos.y, float.MinValue, float.MaxValue, 80), 200);
            panel.Controls.Add(posY);

            LabeledControl posZ = new LabeledControl("Position z", new TextBoxFloat(playerDataFile.ecd.pos.z, float.MinValue, float.MaxValue, 80), 200);
            panel.Controls.Add(posZ);


            LabeledControl rotX = new LabeledControl("Rotation x", new TextBoxFloat(playerDataFile.ecd.rot.x, float.MinValue, float.MaxValue, 80), 200);
            panel.Controls.Add(rotX);

            LabeledControl rotY = new LabeledControl("Rotation y", new TextBoxFloat(playerDataFile.ecd.rot.y, float.MinValue, float.MaxValue, 80), 200);
            panel.Controls.Add(rotY);

            LabeledControl rotZ = new LabeledControl("Rotation z", new TextBoxFloat(playerDataFile.ecd.rot.z, float.MinValue, float.MaxValue, 80), 200);
            panel.Controls.Add(rotZ);


            LabeledControl homeX = new LabeledControl("Home x", new TextBoxInt(playerDataFile.ecd.homePosition.x, int.MinValue, int.MaxValue, 80), 200);
            panel.Controls.Add(homeX);

            LabeledControl homeY = new LabeledControl("Home y", new TextBoxInt(playerDataFile.ecd.homePosition.y, int.MinValue, int.MaxValue, 80), 200);
            panel.Controls.Add(homeY);

            LabeledControl homeZ = new LabeledControl("Home z", new TextBoxInt(playerDataFile.ecd.homePosition.z, int.MinValue, int.MaxValue, 80), 200);
            panel.Controls.Add(homeZ);


            LabeledControl backpackX = new LabeledControl("Backpack position x", new TextBoxInt(playerDataFile.droppedBackpackPosition.x, int.MinValue, int.MaxValue, 80), 200);
            panel.Controls.Add(backpackX);

            LabeledControl backpackY = new LabeledControl("Backpack position y", new TextBoxInt(playerDataFile.droppedBackpackPosition.y, int.MinValue, int.MaxValue, 80), 200);
            panel.Controls.Add(backpackY);

            LabeledControl backpackZ = new LabeledControl("Backpack position z", new TextBoxInt(playerDataFile.droppedBackpackPosition.z, int.MinValue, int.MaxValue, 80), 200);
            panel.Controls.Add(backpackZ);

            Controls.Add(panel);
        }
    }
}
