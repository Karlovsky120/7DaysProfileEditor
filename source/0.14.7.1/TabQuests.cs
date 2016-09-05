using SevenDaysProfileEditor.GUI;
using SevenDaysSaveManipulator.GameData;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.Quests
{
    class TabQuests : TabPage
    {
        private PlayerDataFile playerDataFile;
        private TableLayoutPanel panel;

        private CollectionQuest collectionQuest;

        public TabQuests(PlayerDataFile playerDataFile)
        {
            this.playerDataFile = playerDataFile;
            Text = "Quests";

            panel = new TableLayoutPanel();
            panel.Dock = DockStyle.Fill;
            panel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            panel.AutoScroll = true;
            panel.AutoSize = true;

            collectionQuest = new CollectionQuest(playerDataFile.questJournal);

            foreach (BinderQuest binderQuest in collectionQuest.binderQuests)
            {
                panel.Controls.Add(new SlotQuest(binderQuest));
            }

            Controls.Add(panel);
        }
    }
}
