using SevenDaysProfileEditor.Inventory;
using SevenDaysProfileEditor.Skills;
using SevenDaysSaveManipulator.GameData;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI
{
    class TabPlayer : TabPage
    {
        public PlayerDataFile playerDataFile;
        public string path;
        public string fileName;
        public string folderPath;
        private TabControl tabs;


        private void populateStrings(string path)
        {
            int index = path.LastIndexOf('\\');
            folderPath = path.Substring(0, index);
            fileName = path.Substring(index + 1);

            this.path = path;
        }

        public TabPlayer(string path) : base()
        {
            populateStrings(path);

            playerDataFile = new PlayerDataFile(path);
            Text = playerDataFile.ecd.entityName.get() + " (" + fileName + ")          ";

            tabs = new TabControl();
            tabs.Dock = DockStyle.Fill;
            tabs.Controls.Add(new TabInventory(playerDataFile));
            tabs.Controls.Add(new TabSkills(playerDataFile));

            Controls.Add(tabs);
        }
    }
}
