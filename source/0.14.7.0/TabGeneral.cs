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

            TextBoxInt score = new TextBoxInt(playerDataFile.score, 0, int.MaxValue);
            LabeledBox scoreBox = new LabeledBox("Score", score);
            panel.Controls.Add(scoreBox);

            TextBoxInt playerKills = new TextBoxInt(playerDataFile.playerKills, 0, int.MaxValue);
            LabeledBox playerK = new LabeledBox("Player kills", playerKills);
            panel.Controls.Add(playerK);

            TextBoxInt zombieKills = new TextBoxInt(playerDataFile.zombieKills, 0, int.MaxValue);
            LabeledBox zombieK = new LabeledBox("Zombie kills", zombieKills);
            panel.Controls.Add(zombieK);

            TextBoxInt deaths = new TextBoxInt(playerDataFile.deaths, 0, int.MaxValue);
            LabeledBox deathsBox = new LabeledBox("Deaths", deaths);
            panel.Controls.Add(deathsBox);

            TextBoxFloat positionX = new TextBoxFloat(playerDataFile.ecd.pos.x, float.MinValue, float.MaxValue);
            LabeledBox posX = new LabeledBox("Position x", positionX);
            panel.Controls.Add(posX);

            TextBoxFloat positionY = new TextBoxFloat(playerDataFile.ecd.pos.y, float.MinValue, float.MaxValue);
            LabeledBox posY = new LabeledBox("Position y", positionY);
            panel.Controls.Add(posY);

            TextBoxFloat positionZ = new TextBoxFloat(playerDataFile.ecd.pos.z, float.MinValue, float.MaxValue);
            LabeledBox posZ = new LabeledBox("Position z", positionZ);
            panel.Controls.Add(posZ);


            TextBoxFloat rotationX = new TextBoxFloat(playerDataFile.ecd.rot.x, float.MinValue, float.MaxValue);
            LabeledBox rotX = new LabeledBox("Rotation x", rotationX);
            panel.Controls.Add(rotX);

            TextBoxFloat rotationY = new TextBoxFloat(playerDataFile.ecd.rot.y, float.MinValue, float.MaxValue);
            LabeledBox rotY = new LabeledBox("Rotation y", rotationY);
            panel.Controls.Add(rotY);

            TextBoxFloat rotationZ = new TextBoxFloat(playerDataFile.ecd.rot.z, float.MinValue, float.MaxValue);
            LabeledBox rotZ = new LabeledBox("Rotation z", rotationZ);
            panel.Controls.Add(rotZ);


            TextBoxInt homeX = new TextBoxInt(playerDataFile.ecd.homePosition.x, int.MinValue, int.MaxValue);
            LabeledBox hX = new LabeledBox("Home x", homeX);
            panel.Controls.Add(hX);

            TextBoxInt homeY = new TextBoxInt(playerDataFile.ecd.homePosition.y, int.MinValue, int.MaxValue);
            LabeledBox hY = new LabeledBox("Home y", homeY);
            panel.Controls.Add(hY);

            TextBoxInt homeZ = new TextBoxInt(playerDataFile.ecd.homePosition.z, int.MinValue, int.MaxValue);
            LabeledBox hZ = new LabeledBox("Home z", homeZ);
            panel.Controls.Add(hZ);


            TextBoxInt backpackX = new TextBoxInt(playerDataFile.droppedBackpackPosition.x, int.MinValue, int.MaxValue);
            LabeledBox bX = new LabeledBox("Backpack position x", backpackX);
            panel.Controls.Add(bX);

            TextBoxInt backpackY = new TextBoxInt(playerDataFile.droppedBackpackPosition.y, int.MinValue, int.MaxValue);
            LabeledBox bY = new LabeledBox("Backpack position y", backpackY);
            panel.Controls.Add(bY);

            TextBoxInt backpackZ = new TextBoxInt(playerDataFile.droppedBackpackPosition.z, int.MinValue, int.MaxValue);
            LabeledBox bZ = new LabeledBox("Backpack position z", backpackZ);
            panel.Controls.Add(bZ);

            Controls.Add(panel);
        }
    }
}
