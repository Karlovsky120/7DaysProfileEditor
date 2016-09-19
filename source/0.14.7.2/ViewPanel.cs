using SevenDaysProfileEditor.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.Inventory
{
    public class ViewPanel : Label
    {
        private InventoryTab tabInventory;
        private int index;

        public ViewPanel(int index, InventoryTab inventoryTab)
        {
            this.index = index;
            this.tabInventory = inventoryTab;

            Size cellSize = new Size(IconData.ICON_WIDTH, IconData.ICON_HEIGHT);
            Size = cellSize;
            Margin = new Padding(0);

            SetImage(inventoryTab.itemBinders[index]);

            MouseEnter += (sender, e) =>
            {
                inventoryTab.SlotEntered(index);
            };

            MouseClick += (sender, e) =>
            {
                inventoryTab.SlotClicked(index);
            };

            MouseLeave += (sender, e) =>
            {
                inventoryTab.SlotExited(index);
            };
        }

        public void SetImage(ItemBinder binderItem)
        {
            Text = "";
            BackgroundImage = null;

            Bitmap image = binderItem.GetImage(IconData.ICON_WIDTH, IconData.ICON_HEIGHT);

            if (image != null)
            {
                BackgroundImage = image;
            }
            else
            {
                Text = "(" + binderItem.name + ")";
                TextAlign = ContentAlignment.MiddleCenter;
            }
        }
    }
}