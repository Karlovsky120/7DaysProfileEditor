using SevenDaysProfileEditor.Data;
using SevenDaysSaveManipulator.SaveData;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.Inventory {

    /// <summary>
    /// Shows a part.
    /// </summary>
    internal class InventorySlotPart : InventorySlotBase {
        private InventorySlotItem parent;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="itemBinder">The part</param>
        /// <param name="parent">GUI parent of the part</param>
        /// <param name="partIndex">Index of the part</param>
        /// <param name="textBoxWidth">Width of the textBox</param>
        /// <param name="labeledControlWidth">Width of the labeledBox</param>
        public InventorySlotPart(ItemBinder itemBinder, InventorySlotItem parent, int partIndex, int textBoxWidth, int labeledControlWidth)
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

            CreateSelector(new string[] { parent.itemBinder.itemData.partNames[partIndex], "air" });
            selector.DropDownStyle = ComboBoxStyle.DropDownList;
            itemCore.Controls.Add(selector, 0, 0);

            basicInfo = GenerateBasicInfo();
            itemCore.Controls.Add(basicInfo, 0, 1);

            Controls.Add(itemCore, 1, 0);
        }

        /// <summary>
        /// Updates max degradation when quality is changed and propagates the change to the parent.
        /// </summary>
        /// <param name="source"></param>
        public override void ValueUpdated(Value<int> source) {
            if (source.Equals(itemBinder.itemValue.quality)) {
                degradationBox.UpdateMax(itemBinder.GetMaxDegradationForQuality());

                parent.qualityBox.Text = parent.itemBinder.GetQualityFromParts().ToString();
            }
        }

        /// <summary>
        /// Called when part selection is changed.
        /// </summary>
        protected override void SelectionChangedAction() {
            if (parent.itemBinder.HasAllParts()) {
                parent.qualityBox.Text = parent.itemBinder.GetQualityFromParts().ToString();
                parent.itemBinder.itemValue.meta.Set(parent.itemBinder.itemData.magazineSize);
                parent.magazineBox.Enabled = true;
            }
            else {
                parent.qualityBox.Text = "";
                parent.itemBinder.itemValue.meta.Set(0);
                parent.magazineBox.Text = "";
                parent.magazineBox.Enabled = false;
            }
        }
    }
}