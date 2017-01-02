using SevenDaysProfileEditor.Data;
using SevenDaysSaveManipulator.GameData;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.Inventory {

    /// <summary>
    /// Shows an attachment.
    /// </summary>
    internal class InventorySlotAttachment : InventorySlotBase {
        private InventorySlotItem parent;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="itemBinder">ItemBinder representing the attachment</param>
        /// <param name="parent">GUI parent of the attachment</param>
        /// <param name="attachmentIndex">Index of attachment</param>
        /// <param name="textBoxWidth">Width of the textBox</param>
        /// <param name="labeledControlWidth">Width of the labeled control</param>
        public InventorySlotAttachment(ItemBinder itemBinder, InventorySlotItem parent, int attachmentIndex, int textBoxWidth, int labeledControlWidth)
            : base(itemBinder, textBoxWidth, labeledControlWidth) {
            this.parent = parent;

            Size = new Size(326, 104);

            imageLabel = new Label();
            imageLabel.Anchor = AnchorStyles.Left;
            imageLabel.Size = new Size(IconData.ICON_WIDTH, IconData.ICON_HEIGHT);

            SetImage();

            Controls.Add(imageLabel, 0, 0);

            itemCore = new TableLayoutPanel();
            itemCore.Anchor = AnchorStyles.Right;
            itemCore.Size = new Size(200, 100);

            CreateSelector(new string[] { parent.itemBinder.itemData.attachmentNames[attachmentIndex], "air" });
            selector.DropDownStyle = ComboBoxStyle.DropDownList;
            itemCore.Controls.Add(selector, 0, 0);

            basicInfo = GenerateBasicInfo();
            itemCore.Controls.Add(basicInfo, 0, 1);

            Controls.Add(itemCore, 1, 0);
        }

        /// <summary>
        /// Updates max degradation when quality is changed.
        /// </summary>
        /// <param name="source"></param>
        public override void ValueUpdated(Value<int> source) {
            if (source.Equals(itemBinder.itemValue.quality)) {
                degradationBox.UpdateMax(itemBinder.GetMaxDegradationForQuality());
            }
        }
    }
}