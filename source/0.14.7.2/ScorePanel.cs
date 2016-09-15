using SevenDaysProfileEditor.GUI;
using SevenDaysSaveManipulator.GameData;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.Stats
{
    class ScorePanel : TableLayoutPanel//, IValueListener<int>
    {
        private PlayerDataFile playerDataFile;
        private TextBoxInt scoreBox;

        public ScorePanel(PlayerDataFile playerDataFile)
        {
            this.playerDataFile = playerDataFile;

            scoreBox = new TextBoxInt(playerDataFile.score, 0/*GetMinScore()*/, int.MaxValue, 60);
            LabeledControl labeledScoreBox = new LabeledControl("Score", scoreBox, 190);
            Controls.Add(labeledScoreBox);

            LabeledControl LabeledPlayerKillsBox = new LabeledControl("Player kills", new TextBoxInt(playerDataFile.playerKills, 0, int.MaxValue, 60), 190);
            Controls.Add(LabeledPlayerKillsBox);

            LabeledControl labeledZombieKillsBox = new LabeledControl("Zombie kills", new TextBoxInt(playerDataFile.zombieKills, 0, int.MaxValue, 60), 190);
            Controls.Add(labeledZombieKillsBox);

            LabeledControl labeledDeathsBox = new LabeledControl("Deaths", new TextBoxInt(playerDataFile.deaths, 0, int.MaxValue, 60), 190);
            Controls.Add(labeledDeathsBox);

            Size = new Size(196, 140);

            /*playerDataFile.playerKills.AddListener(this);
            playerDataFile.zombieKills.AddListener(this);
            playerDataFile.deaths.AddListener(this);*/
        }

        /*public void ValueUpdated(Value<int> source)
        {
            scoreBox.UpdateMin(GetMinScore());
        }

        private int GetMinScore()
        {
            return playerDataFile.playerKills.Get() + playerDataFile.zombieKills.Get() - (5 * playerDataFile.deaths.Get());
        }*/
    }
}
