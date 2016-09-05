using SevenDaysProfileEditor.GUI;
using SevenDaysSaveManipulator.GameData;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.General
{
    class TabGeneral : TabPage
    {
        private PlayerDataFile playerDataFile;
        private TableLayoutPanel panel;

        public TabGeneral(PlayerDataFile playerDataFile)
        {
            this.playerDataFile = playerDataFile;
            Text = "General";

            panel = new TableLayoutPanel();
            panel.Dock = DockStyle.Fill;
            panel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;          
            panel.AutoScroll = true;
            panel.AutoSize = true;

            LabeledBox score = new LabeledBox("Score", new TextBoxInt(playerDataFile.score, 0, int.MaxValue));
            panel.Controls.Add(score);

            LabeledBox playerKills = new LabeledBox("Player kills", new TextBoxInt(playerDataFile.playerKills, 0, int.MaxValue));
            panel.Controls.Add(playerKills);

            LabeledBox zombieKills = new LabeledBox("Zombie kills", new TextBoxInt(playerDataFile.zombieKills, 0, int.MaxValue));
            panel.Controls.Add(zombieKills);

            LabeledBox deaths = new LabeledBox("Deaths", new TextBoxInt(playerDataFile.deaths, 0, int.MaxValue));
            panel.Controls.Add(deaths);


            LabeledBox posX = new LabeledBox("Position x", new TextBoxFloat(playerDataFile.ecd.pos.x, float.MinValue, float.MaxValue));
            panel.Controls.Add(posX);

            LabeledBox posY = new LabeledBox("Position y", new TextBoxFloat(playerDataFile.ecd.pos.y, float.MinValue, float.MaxValue));
            panel.Controls.Add(posY);

            LabeledBox posZ = new LabeledBox("Position z", new TextBoxFloat(playerDataFile.ecd.pos.z, float.MinValue, float.MaxValue));
            panel.Controls.Add(posZ);


            LabeledBox rotX = new LabeledBox("Rotation x", new TextBoxFloat(playerDataFile.ecd.rot.x, float.MinValue, float.MaxValue));
            panel.Controls.Add(rotX);

            LabeledBox rotY = new LabeledBox("Rotation y", new TextBoxFloat(playerDataFile.ecd.rot.y, float.MinValue, float.MaxValue));
            panel.Controls.Add(rotY);

            LabeledBox rotZ = new LabeledBox("Rotation z", new TextBoxFloat(playerDataFile.ecd.rot.z, float.MinValue, float.MaxValue));
            panel.Controls.Add(rotZ);


            LabeledBox homeX = new LabeledBox("Home x", new TextBoxInt(playerDataFile.ecd.homePosition.x, int.MinValue, int.MaxValue));
            panel.Controls.Add(homeX);

            LabeledBox homeY = new LabeledBox("Home y", new TextBoxInt(playerDataFile.ecd.homePosition.y, int.MinValue, int.MaxValue));
            panel.Controls.Add(homeY);

            LabeledBox homeZ = new LabeledBox("Home z", new TextBoxInt(playerDataFile.ecd.homePosition.z, int.MinValue, int.MaxValue));
            panel.Controls.Add(homeZ);


            LabeledBox backpackX = new LabeledBox("Backpack position x", new TextBoxInt(playerDataFile.droppedBackpackPosition.x, int.MinValue, int.MaxValue));
            panel.Controls.Add(backpackX);

            LabeledBox backpackY = new LabeledBox("Backpack position y", new TextBoxInt(playerDataFile.droppedBackpackPosition.y, int.MinValue, int.MaxValue));
            panel.Controls.Add(backpackY);

            LabeledBox backpackZ = new LabeledBox("Backpack position z", new TextBoxInt(playerDataFile.droppedBackpackPosition.z, int.MinValue, int.MaxValue));
            panel.Controls.Add(backpackZ);

            Controls.Add(panel);
        }
    }
}
