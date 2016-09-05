using SevenDaysProfileEditor.General;
using SevenDaysProfileEditor.Inventory;
using SevenDaysProfileEditor.Skills;
using SevenDaysProfileEditor.Stats;
using SevenDaysProfileEditor.Quests;
using SevenDaysSaveManipulator.GameData;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI
{
    class TabPlayer : TabPage
    {
        public PlayerDataFile playerDataFile;
        public string path;
        public string fileName;
        private TabControl tabs;


        private void populateStrings(string path)
        {
            int index = path.LastIndexOf('\\');
            fileName = path.Substring(index + 1);

            this.path = path;
        }

        public TabPlayer(PlayerDataFile playerDataFile, string path) : base()
        {
            populateStrings(path);

            this.playerDataFile = playerDataFile;
            Text = playerDataFile.ecd.entityName.get() + " (" + fileName + ")          ";

            tabs = new TabControl();
            tabs.Dock = DockStyle.Fill;
            tabs.Controls.Add(new TabGeneral(playerDataFile));
            tabs.Controls.Add(new TabStats(playerDataFile));
            tabs.Controls.Add(new TabInventory(playerDataFile));
            tabs.Controls.Add(new TabSkills(playerDataFile));
            //tabs.Controls.Add(new TabQuests(playerDataFile));

            Controls.Add(tabs);
        }
    }
}
