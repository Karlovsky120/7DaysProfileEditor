using SevenDaysProfileEditor.GUI;
using SevenDaysSaveManipulator.SaveData;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.StatsAndGeneral {

    /// <summary>
    /// Displays player score.
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    internal class ScorePanel : TableLayoutPanel//, IValueListener<int>
    {
        private PlayerDataFile playerDataFile;
        private NumericTextBox<int> scoreBox;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="playerDataFile">PlayerDataFile to use</param>
        public ScorePanel(PlayerDataFile playerDataFile) {
            this.playerDataFile = playerDataFile;

            AutoSize = true;

            scoreBox = new NumericTextBox<int>(playerDataFile.score, 0/*GetMinScore()*/, int.MaxValue, 60);
            LabeledControl labeledScoreBox = new LabeledControl("Score", scoreBox, 190);
            Controls.Add(labeledScoreBox);

            LabeledControl LabeledPlayerKillsBox = new LabeledControl("Player kills", new NumericTextBox<int>(playerDataFile.playerKills, 0, int.MaxValue, 60), 190);
            Controls.Add(LabeledPlayerKillsBox);

            LabeledControl labeledZombieKillsBox = new LabeledControl("Zombie kills", new NumericTextBox<int>(playerDataFile.zombieKills, 0, int.MaxValue, 60), 190);
            Controls.Add(labeledZombieKillsBox);

            LabeledControl labeledDeathsBox = new LabeledControl("Deaths", new NumericTextBox<int>(playerDataFile.deaths, 0, int.MaxValue, 60), 190);
            Controls.Add(labeledDeathsBox);
        }
    }
}