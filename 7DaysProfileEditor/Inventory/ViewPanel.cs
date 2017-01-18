using SevenDaysProfileEditor.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.Inventory {

    /// <summary>
    /// Displays the icon or name of the item
    /// </summary>
    internal class ViewPanel : Label {
        private int index;
        private InventoryTab tabInventory;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="index">Index of the viewPanel</param>
        /// <param name="inventoryTab">InventoryTab that contains the viewPanel</param>
        public ViewPanel(int index, InventoryTab inventoryTab) {
            this.index = index;
            this.tabInventory = inventoryTab;

            Size = new Size(IconData.ICON_WIDTH, IconData.ICON_HEIGHT);
            Margin = new Padding(0);

            SetImage(inventoryTab.itemBinders[index]);

            MouseClick += (sender, e) => {
                inventoryTab.SlotClicked(index);
            };

             MouseEnter += (sender, e) => {
                inventoryTab.SlotEntered(index);
            };

            MouseLeave += (sender, e) => {
                inventoryTab.SlotExited(index);
            };
        }

        /// <summary>
        /// Sets the image of specified itemBinder
        /// </summary>
        /// <param name="itemBinder"></param>
        public void SetImage(ItemBinder itemBinder) {
            Text = "";
            BackgroundImage = null;

            Bitmap image = itemBinder.GetImage(IconData.ICON_WIDTH, IconData.ICON_HEIGHT);

            if (image != null) {
                BackgroundImage = image;
            }
            else {
                Text = string.Format("({0})", itemBinder.itemData.name);
                TextAlign = ContentAlignment.MiddleCenter;
            }
        }
    }
}
 