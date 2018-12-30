using SevenDaysProfileEditor.Data;
using SevenDaysProfileEditor.GUI;
using SevenDaysSaveManipulator;
using SevenDaysSaveManipulator.PlayerData;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.Inventory {

    /// <summary>
    /// Base class for inventory slots
    /// </summary>
    internal abstract class InventorySlotBase : TableLayoutPanel, IValueListener<int> {
        public TableLayoutPanel basicInfo;
        public InvertedIntegerTextBox degradationBox;
        public Label imageLabel;
        public ItemBinder itemBinder;

        public TableLayoutPanel itemCore;
        public NumericTextBox<int> magazineBox;
        public NumericTextBox<int> qualityBox;
        public string selectedItem;
        public ComboBox selector;
        private int labeledControlWidth;
        private ComboBox magazineItemsBox;
        private int textBoxWidth;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="itemBinder">ItemBinder associated with this inventory slot</param>
        /// <param name="textBoxWidth">Width of the textBox</param>
        /// <param name="labeledControlWidth">Width of the labeled control</param>
        public InventorySlotBase(ItemBinder itemBinder, int textBoxWidth, int labeledControlWidth) {
            this.itemBinder = itemBinder;
            this.textBoxWidth = textBoxWidth;
            this.labeledControlWidth = labeledControlWidth;

            selectedItem = itemBinder.itemData.name;
        }

        /// <summary>
        /// Selected item on comboBox seems to have change at random for some reason. This makes sure this never happens.
        /// </summary>
        public virtual void OverrideBug() {
            selector.SelectedItem = selectedItem;
        }

        /// <summary>
        /// Method to override.
        /// </summary>
        /// <param name="source"></param>
        public virtual void ValueUpdated(Value<int> source) {
        }

        /// <summary>
        /// Creates the comboBox using a provided name list.
        /// </summary>
        /// <param name="nameList">Name list to populate the comboBox with</param>
        protected void CreateSelector(string[] nameList) {
            selector = new ComboBox();
            selector.Anchor = AnchorStyles.Top;
            selector.Width = labeledControlWidth;
            selector.BindingContext = new BindingContext();
            selector.DataSource = nameList;
            selector.SelectedIndex = Array.IndexOf(nameList, itemBinder.itemData.name);
            selector.KeyDown += (sender, e) => {
                if (e.KeyCode == Keys.Enter) {
                    SelectionChanged(sender, e);
                }
            };

            selector.SelectedValueChanged += new EventHandler(SelectionChanged);
            selector.LostFocus += new EventHandler(SelectionChanged);
        }

        /// <summary>
        /// Generates basic info for an item. This applies to every subclass.
        /// </summary>
        /// <returns></returns>
        protected TableLayoutPanel GenerateBasicInfo() {
            TableLayoutPanel basicInfo = new TableLayoutPanel() {
                Anchor = AnchorStyles.None,
                AutoSize = true
            };

            if (!itemBinder.itemData.name.Equals("air")) {
                if (itemBinder.itemData.stackNumber > 1) {
                    LabeledControl countBox = new LabeledControl("Count", new NumericTextBox<ushort>(itemBinder.itemStack.count, 1, itemBinder.itemData.stackNumber, textBoxWidth), labeledControlWidth);
                    basicInfo.Controls.Add(countBox);
                }

                if (itemBinder.itemData.hasQuality) {
                    int degradation = itemBinder.GetMaxDegradationForQuality();

                    degradationBox = new InvertedIntegerTextBox(itemBinder.itemValue.useTimes, 1, degradation, textBoxWidth);
                    LabeledControl labeledDegradationBox = new LabeledControl("Durability", degradationBox, labeledControlWidth);

                    qualityBox = new NumericTextBox<int>(itemBinder.itemValue.quality, 1, ItemData.MAX_QUALITY, textBoxWidth);
                    itemBinder.itemValue.quality.AddListener(this);
                    LabeledControl labeledQualityBox = new LabeledControl("Quality", qualityBox, labeledControlWidth);

                    basicInfo.Controls.Add(labeledQualityBox);
                    basicInfo.Controls.Add(labeledDegradationBox);

                    if (itemBinder.itemData.magazineSize > 0) {
                        magazineBox = new NumericTextBox<int>(itemBinder.itemValue.meta, 0, itemBinder.itemData.magazineSize, textBoxWidth);
                        LabeledControl labeledMagazineBox = new LabeledControl("Ammo loaded", magazineBox, labeledControlWidth);

                        basicInfo.Controls.Add(labeledMagazineBox);
                    }

                    if (itemBinder.itemData.partNames != null) {
                        qualityBox.Enabled = false;
                        degradationBox.Enabled = false;

                        if (itemBinder.HasAllParts()) {
                            qualityBox.Text = itemBinder.GetQualityFromParts().ToString();
                        }
                        else {
                            magazineBox.Enabled = false;
                            qualityBox.Text = "";
                            magazineBox.Text = "";
                        }

                        degradationBox.Text = "";
                    }

                    if (itemBinder.itemData.magazineItems != null && itemBinder.itemData.magazineItems.Length > 1) {
                        magazineItemsBox = new ComboBox();
                        magazineItemsBox.BindingContext = new BindingContext();
                        magazineItemsBox.DataSource = itemBinder.itemData.magazineItems;
                        magazineItemsBox.SelectedIndex = itemBinder.itemValue.selectedAmmoTypeIndex.Get();

                        magazineItemsBox.Width = textBoxWidth;
                        magazineItemsBox.DropDownStyle = ComboBoxStyle.DropDownList;

                        magazineItemsBox.DropDownClosed += new EventHandler(MagazineItemBoxChanged);
                        magazineItemsBox.SelectedValueChanged += new EventHandler(MagazineItemBoxChanged);

                        LabeledControl magazineComboBox = new LabeledControl("Selected ammo", magazineItemsBox, labeledControlWidth);
                        basicInfo.Controls.Add(magazineComboBox);
                    }
                }
            }

            return basicInfo;
        }

        protected void SelectionChanged(object sender, EventArgs e) {
            ItemData choosen = ItemData.GetItemDataByName(selector.Text);

            if (choosen == null) {
                selector.Text = selectedItem;
            }
            else if (!choosen.name.Equals(selectedItem)) {
                selectedItem = selector.Text;

                itemBinder.ResetItemBinder(choosen);

                SetImage();

                itemCore.Controls.Remove(basicInfo);
                basicInfo = GenerateBasicInfo();
                itemCore.Controls.Add(basicInfo, 0, 2);

                SelectionChangedAction();
            }
        }

        protected virtual void SelectionChangedAction() {
        }

        protected void SetImage() {
            imageLabel.Anchor = AnchorStyles.None;
            imageLabel.Text = "";
            imageLabel.BackgroundImage = null;

            Bitmap image = itemBinder.GetImage(IconData.ICON_WIDTH, IconData.ICON_HEIGHT);

            if (image != null) {
                imageLabel.BackgroundImage = image;
            }
            else {
                imageLabel.Text = string.Format("({0})", itemBinder.itemData.name);
                imageLabel.TextAlign = ContentAlignment.MiddleCenter;
            }
        }

        /// <summary>
        /// Updates the data in ttp to reflect visual representation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MagazineItemBoxChanged(object sender, EventArgs e) {
            itemBinder.itemValue.selectedAmmoTypeIndex.Set((byte)magazineItemsBox.SelectedIndex);
        }
    }
}