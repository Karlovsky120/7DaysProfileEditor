using SevenDaysProfileEditor.Inventory;
using SevenDaysSaveManipulator.GameData;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.Inventory
{
    class TabInventory : TabPage
    {
        private PlayerDataFile playerDataFile;
        private TableLayoutPanel panel;

        public TabInventory(PlayerDataFile playerDataFile)
        {
            this.playerDataFile = playerDataFile;
            Text = "Inventory";

            panel = new TableLayoutPanel();
            
            panel.Dock = DockStyle.Fill;
            panel.AutoScroll = true;
            panel.AutoSize = true;
            panel.RowCount = 5;
            panel.ColumnCount = 8;           

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    ItemStack itemStack = playerDataFile.bag[i * 8 + j];
                    BinderItem itemBinder = new BinderItem(itemStack);

                    SlotInventory slot = new SlotInventory(itemBinder, DataItem.getNameList());
                    panel.Controls.Add(slot, j, i);
                }
            }

            for (int i = 0; i < 8; i++)
            {
                ItemStack itemStack = playerDataFile.inventory[i];
                BinderItem itemBinder = new BinderItem(itemStack);

                SlotInventory slot = new SlotInventory(itemBinder, DataItem.getNameList());
                panel.Controls.Add(slot, i, 4);
            }

            Controls.Add(panel);
        }

    }
}
