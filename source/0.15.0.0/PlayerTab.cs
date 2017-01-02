using SevenDaysProfileEditor.Inventory;
using SevenDaysProfileEditor.Skills;
using SevenDaysProfileEditor.General;
using SevenDaysSaveManipulator.GameData;
using System;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI
{
    internal class PlayerTab : TabPage
    {
        public PlayerDataFile playerDataFile;
        public string path;
        public string fileName;

        public TabControl saveFileTabControl;

        private bool[] tabInitialized;

        public PlayerTab(PlayerDataFile playerDataFile, string path, int selectedIndex)
        {
            this.path = path;
            fileName = path.Substring(path.LastIndexOf('\\') + 1);

            this.playerDataFile = playerDataFile;
            Text = playerDataFile.ecd.entityName.Get() + "          ";

            saveFileTabControl = new TabControl();
            saveFileTabControl.Dock = DockStyle.Fill;

            saveFileTabControl.Controls.Add(new GeneralTab(playerDataFile));
            saveFileTabControl.Controls.Add(new InventoryTab(playerDataFile));
            saveFileTabControl.Controls.Add(new SkillsTab(playerDataFile));

            saveFileTabControl.SelectedIndex = selectedIndex;

            saveFileTabControl.SelectedIndexChanged += OnSelectedIndexChanged;

            tabInitialized = new bool[saveFileTabControl.TabCount];

            ((IInitializable)saveFileTabControl.Controls[selectedIndex]).Initialize();
            tabInitialized[selectedIndex] = true;

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