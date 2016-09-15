using SevenDaysProfileEditor.Inventory;
using SevenDaysProfileEditor.Skills;
using SevenDaysProfileEditor.Stats;
using SevenDaysSaveManipulator.GameData;
using System;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI
{
    class PlayerTab : TabPage
    {
        public PlayerDataFile playerDataFile;
        public string path;
        public string fileName;

        private TabControl saveFileTabControl;
        private bool[] tabInitialized;

        public PlayerTab(PlayerDataFile playerDataFile, string path)
        {
            this.path = path;
            fileName = path.Substring(path.LastIndexOf('\\') + 1);

            this.playerDataFile = playerDataFile;
            Text = playerDataFile.ecd.entityName.Get() + " (" + fileName + ")          ";

            saveFileTabControl = new TabControl();
            saveFileTabControl.Dock = DockStyle.Fill;

            saveFileTabControl.Controls.Add(new StatsAndGeneralTab(playerDataFile));
            saveFileTabControl.Controls.Add(new InventoryTab(playerDataFile));
            saveFileTabControl.Controls.Add(new SkillsTab(playerDataFile));
            //saveFileTabControl.Controls.Add(new QuestsTab(playerDataFile));
            saveFileTabControl.SelectedIndexChanged += OnSelectedIndexChanged;

            tabInitialized = new bool[saveFileTabControl.TabCount];

            ((IInitializable)saveFileTabControl.Controls[0]).Initialize();
            tabInitialized[0] = true;

            Controls.Add(saveFileTabControl);
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!tabInitialized[saveFileTabControl.SelectedIndex])
            {
                ((IInitializable)saveFileTabControl.SelectedTab).Initialize();

                tabInitialized[saveFileTabControl.SelectedIndex] = true;
            }
        }
    }
}
