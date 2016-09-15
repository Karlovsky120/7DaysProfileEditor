using SevenDaysProfileEditor.Data;
using SevenDaysSaveManipulator.GameData;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.Inventory
{
    public class AttachmentSlot : InventorySlot
    {
        private ItemSlot parent;

        public AttachmentSlot(ItemBinder itemBinder, ItemSlot parent, int partIndex, int textBoxWidth, int labeledControlWidth) : base(itemBinder, textBoxWidth, labeledControlWidth)
        {
            this.parent = parent;

            Size = new Size(326, 104);

            imageLabel = new Label();
            imageLabel.Anchor = AnchorStyles.Left;
            imageLabel.Size = new Size(IconData.ICON_WIDTH, IconData.ICON_HEIGHT);

            SetImage();

            Controls.Add(imageLabel, 0, 0);

            itemCore = new TableLayoutPanel();
            itemCore.Anchor = AnchorStyles.Right;
            itemCore.AutoSize = true;

            CreateSelector(new string[] { parent.itemBinder.attachmentNames[partIndex], "air" });
            selector.DropDownStyle = ComboBoxStyle.DropDownList;
            itemCore.Controls.Add(selector, 0, 0);

            basicInfo = GenerateBasicInfo();
            itemCore.Controls.Add(basicInfo, 0, 1);

            Controls.Add(itemCore, 1, 0);
        }

        public override void ValueUpdated(Value<int> source)
        {
            if (source.Equals(itemBinder.quality))
            {
                degradationBox.UpdateMax(itemBinder.GetMaxDegradationForQuality());
            }
        }
    }
}
