using SevenDaysProfileEditor.Data;
using SevenDaysProfileEditor.GUI;
using SevenDaysSaveManipulator.GameData;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.Inventory {

    /// <summary>
    /// Shows an item.
    /// </summary>
    internal class InventorySlotItem : InventorySlotBase {
        private TableLayoutPanel attachments;
        private TableLayoutPanel parts;
        private ScrollPanel scrollPanel;
        private ViewPanel viewPanel;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="itemBinder">An item to show</param>
        /// <param name="viewPanel">Panel that holds the image of the item</param>
        /// <param name="textBoxWidth">Width of the textBox</param>
        /// <param name="labeledControlWidth">Width of the labeled control</param>
        public InventorySlotItem(ItemBinder itemBinder, ViewPanel viewPanel, int textBoxWidth, int labeledControlWidth)
            : base(itemBinder, textBoxWidth, labeledControlWidth) {
            this.viewPanel = viewPanel;

            Size = new Size(956, 268);
            CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;

            itemCore = new TableLayoutPanel();
            itemCore.Size = new Size(262, 248);

            CreateSelector(ItemData.GetNameList());
            selector.DropDownStyle = ComboBoxStyle.DropDown;
            selector.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            itemCore.Controls.Add(selector, 0, 0);

            imageLabel = new Label();
            imageLabel.Size = new Size(IconData.ICON_WIDTH, IconData.ICON_HEIGHT);
            SetImage();
            itemCore.Controls.Add(imageLabel, 0, 1);

            basicInfo = GenerateBasicInfo();
            basicInfo.Anchor = AnchorStyles.Top;
            itemCore.Controls.Add(basicInfo, 0, 2);

            Controls.Add(itemCore, 0, 0);

            parts = GenerateParts();
            attachments = GenerateAttachments();

            if (parts != null || attachments != null) {
                scrollPanel = new ScrollPanel();
                scrollPanel.Size = new Size(676, 248);

                if (parts != null) {
                    scrollPanel.Controls.Add(parts, 0, 0);
                }

                if (attachments != null) {
                    scrollPanel.Controls.Add(attachments, 1, 0);
                }

                Controls.Add(scrollPanel, 1, 0);
            }
        }

        /// <summary>
        /// Selected item on comboBox seems to have change at random for some reason. This makes sure this never happens. It also calls it for any of its children.
        /// </summary>
        public override void OverrideBug() {
            selector.SelectedItem = selectedItem;

            if (parts != null) {
                foreach (InventorySlotPart slotPart in parts.Controls) {
                    slotPart.OverrideBug();
                }
            }

            if (attachments != null) {
                foreach (InventorySlotAttachment slotAttachment in attachments.Controls) {
                    slotAttachment.OverrideBug();
                }
            }
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

        /// <summary>
        /// Called when new item is selected.
        /// </summary>
        protected override void SelectionChangedAction() {
            viewPanel.SetImage(itemBinder);

            if (scrollPanel != null) {
                Controls.Remove(scrollPanel);
            }

            parts = GenerateParts();
            attachments = GenerateAttachments();

            if (parts != null || attachments != null) {
                scrollPanel = new ScrollPanel();
                scrollPanel.Size = new Size(676, 248);

                if (parts != null) {
                    scrollPanel.Controls.Add(parts, 0, 0);
                }

                if (attachments != null) {
                    scrollPanel.Controls.Add(attachments, 1, 0);
                }

                Controls.Add(scrollPanel, 1, 0);
            }
        }

        /// <summary>
        /// Generates a panel holding all attachment info.
        /// </summary>
        /// <returns>Panel holding all attachment info</returns>
        private TableLayoutPanel GenerateAttachments() {
            if (itemBinder.itemData.attachmentNames != null) {
                TableLayoutPanel attachments = new TableLayoutPanel() {
                    CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset,
                    AutoSize = true
                };

                string[] addedAttachments = new string[itemBinder.itemValue.attachments.Count];

                for (int i = 0; i < itemBinder.itemValue.attachments.Count; ++i) {
                    ItemBinder attachment = new ItemBinder(itemBinder.itemValue.attachments[i]);
                    InventorySlotAttachment slotAttachment = new InventorySlotAttachment(attachment, this, i, 80, 180);

                    attachments.Controls.Add(slotAttachment, i / 2, i % 2);
                }

                return attachments;
            }

            return null;
        }

        /// <summary>
        /// Generates a panel holding all parts info.
        /// </summary>
        /// <returns>Panel holding all parts info</returns>
        private TableLayoutPanel GenerateParts() {
            if (itemBinder.itemData.partNames != null) {
                TableLayoutPanel parts = new TableLayoutPanel();
                parts.Size = new Size(670, 226);
                parts.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;

                for (int i = 0; i < 4; i++) {
                    ItemBinder part = new ItemBinder(itemBinder.itemValue.parts[i]);
                    InventorySlotPart slotPart = new InventorySlotPart(part, this, i, 80, 180);

                    parts.Controls.Add(slotPart, i / 2, i % 2);
                }

                return parts;
            }

            return null;
        }
    }
}