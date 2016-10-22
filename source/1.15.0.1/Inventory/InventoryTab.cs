using SevenDaysProfileEditor.GUI;
using SevenDaysSaveManipulator.GameData;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.Inventory {

    /// <summary>
    /// Tab for dealing with inventory.
    /// </summary>
    internal class InventoryTab : TabPage {
        public ItemBinder[] itemBinders;
        private int activeIndex;
        private Color COLOR_HOVER = Color.Cyan;
        private Color COLOR_SELECT = Color.Red;
        private InventorySlotItem[] itemSlots;
        private TableLayoutPanel itemView;
        private PlayerDataFile playerDataFile;
        private ViewPanel[] viewPanels;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="playerDataFile">PlayerDataFile which inventory is to be used</param>
        public InventoryTab(PlayerDataFile playerDataFile) {
            Text = "Inventory";

            this.playerDataFile = playerDataFile;

            SetUpInventory(playerDataFile.bag);
            SetUpInventory(playerDataFile.inventory);

            itemBinders = new ItemBinder[40];
            itemSlots = new InventorySlotItem[40];
            viewPanels = new ViewPanel[40];

            for (int i = 0; i < 32; i++) {
                itemBinders[i] = new ItemBinder(playerDataFile.bag[i]);
                viewPanels[i] = new ViewPanel(i, this);
                itemSlots[i] = new InventorySlotItem(itemBinders[i], viewPanels[i], 140, 250);
            }

            for (int i = 32; i < 40; i++) {
                itemBinders[i] = new ItemBinder(playerDataFile.inventory[i - 32]);
                viewPanels[i] = new ViewPanel(i, this);
                itemSlots[i] = new InventorySlotItem(itemBinders[i], viewPanels[i], 140, 250);
            }

            TableLayoutPanel basicPanel = new TableLayoutPanel() {
                Dock = DockStyle.Fill,
                AutoSize = true
            };

            itemView = new TableLayoutPanel();
            itemView.Dock = DockStyle.Top;
            itemView.AutoSize = true;
            itemView.MinimumSize = new Size(0, 260);

            activeIndex = 0;
            viewPanels[activeIndex].BackColor = COLOR_SELECT;
            itemView.Controls.Add(itemSlots[activeIndex]);

            basicPanel.Controls.Add(itemView);

            TableLayoutPanel inventoryDisplay = new TableLayoutPanel() {
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Outset,
                Anchor = AnchorStyles.Bottom,
                Size = new Size(946, 412)
            };

            for (int i = 0; i < 40; i++) {
                inventoryDisplay.Controls.Add(viewPanels[i], i % 8, i / 8);
            }

            basicPanel.Controls.Add(inventoryDisplay);

            Controls.Add(basicPanel);
        }

        /// <summary>
        /// Manages view panel color changes on click events.
        /// </summary>
        /// <param name="index"></param>
        public void SlotClicked(int index) {
            viewPanels[activeIndex].BackColor = default(Color);
            viewPanels[index].BackColor = COLOR_SELECT;

            activeIndex = index;
        }

        /// <summary>
        /// Manages view panel color changes on mouse enter events.
        /// </summary>
        /// <param name="index"></param>
        public void SlotEntered(int index) {
            viewPanels[index].BackColor = COLOR_HOVER;

            itemView.Controls.RemoveAt(0);
            itemView.Controls.Add(itemSlots[index]);
            itemSlots[index].OverrideBug();
        }

        /// <summary>
        /// Manages view panel color changes on mouse exit events.
        /// </summary>
        /// <param name="index"></param>
        public void SlotExited(int index) {
            if (index == activeIndex) {
                viewPanels[index].BackColor = COLOR_SELECT;
            }
            else {
                viewPanels[index].BackColor = default(Color);

                itemView.Controls.RemoveAt(0);
                itemView.Controls.Add(itemSlots[activeIndex]);
                itemSlots[activeIndex].OverrideBug();
            }
        }

        /// <summary>
        /// Sets up inventory for editing. Adds air items to any potential attachment slots.
        /// </summary>
        /// <param name="itemStacks">Inventory to be set up</param>
        private void SetUpInventory(ItemStack[] itemStacks) {
            for (int i = 0; i < itemStacks.Length; i++) {
                string[] attachmentNames = ItemData.GetItemDataByItemValue(itemStacks[i].itemValue).attachmentNames;

                if (attachmentNames != null) {
                    List<ItemValue> attachments = new List<ItemValue>();

                    foreach (string attachmentName in attachmentNames) {
                        bool added = false;

                        foreach (ItemValue attachment in itemStacks[i].itemValue.attachments) {
                            if (attachmentNames.Equals(ItemData.GetItemDataByItemValue(itemStacks[i].itemValue).name)) {
                                attachments.Add(itemStacks[i].itemValue);
                                added = true;
                            }
                        }

                        if (!added) {
                            attachments.Add(ItemBinder.GetAir().itemValue);
                        }
                    }

                    itemStacks[i].itemValue.attachments = attachments;
                }
            }
        }
    }
}