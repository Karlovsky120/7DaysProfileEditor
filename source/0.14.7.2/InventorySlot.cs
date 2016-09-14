using SevenDaysProfileEditor.GUI;
using SevenDaysProfileEditor.Inventory;
using SevenDaysSaveManipulator;
using SevenDaysSaveManipulator.GameData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.Inventory
{
    public abstract class InventorySlot : TableLayoutPanel, IValueListener<int>
    {
        public ItemBinder itemBinder;

        public TextBoxInt qualityBox;
        public TextBoxIntInverted degradationBox;
        public TextBoxInt magazineBox;

        public ComboBox selector;
        public Label imageLabel;
        public TableLayoutPanel basicInfo;
        public TableLayoutPanel itemCore;

        public string selectedItem;

        private int textBoxWidth;
        private int labeledControlWidth;

        public InventorySlot(ItemBinder itemBinder, int textBoxWidth, int labeledControlWidth)
        {
            this.itemBinder = itemBinder;
            this.textBoxWidth = textBoxWidth;
            this.labeledControlWidth = labeledControlWidth;

            selectedItem = itemBinder.name;
        }

        public virtual void OverrideBug()
        {
            selector.SelectedItem = selectedItem;
        }

        protected void CreateSelector(string[] nameList)
        {
            selector = new ComboBox();
            selector.Anchor = AnchorStyles.Top;
            selector.Width = labeledControlWidth;
            selector.BindingContext = new BindingContext();
            selector.DataSource = nameList;
            selector.SelectedIndex = Array.IndexOf(nameList, itemBinder.name);
            selector.KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    SelectionChanged(sender, e);
                }
            };

            selector.SelectedIndexChanged += new EventHandler(SelectionChanged);
            selector.LostFocus += new EventHandler(SelectionChanged);
        }

        protected void SelectionChanged(object sender, EventArgs e)
        {
            ItemData choosen = ItemData.GetItemDataByName(selector.Text);

            if (choosen == null)
            {
                selector.Text = selectedItem;
            }

            else if (!choosen.name.Equals(selectedItem))
            {
                selectedItem = selector.Text;

                itemBinder.ResetItemBinder(choosen);

                SetImage();

                itemCore.Controls.Remove(basicInfo);
                basicInfo = GenerateBasicInfo();
                itemCore.Controls.Add(basicInfo, 0, 2);

                SelectionChangedAction();
            }
        }

        protected virtual void SelectionChangedAction()
        {

        }

        protected TableLayoutPanel GenerateBasicInfo()
        {
            TableLayoutPanel basicInfo = new TableLayoutPanel();
            basicInfo.Anchor = AnchorStyles.None;
            basicInfo.AutoSize = true;

            if (!itemBinder.name.Equals("air"))
            {
                if (itemBinder.stackNumber > 1)
                {
                    LabeledControl countBox = new LabeledControl("Count", new TextBoxInt(itemBinder.count, 1, itemBinder.stackNumber, textBoxWidth), labeledControlWidth);
                    basicInfo.Controls.Add(countBox);
                }

                if (itemBinder.hasQuality)
                {
                    int degradation = itemBinder.GetMaxDegradationForQuality();

                    degradationBox = new TextBoxIntInverted(itemBinder.useTimes, 1, degradation, textBoxWidth);
                    LabeledControl labeledDegradationBox = new LabeledControl("Durability", degradationBox, labeledControlWidth);

                    qualityBox = new TextBoxInt(itemBinder.quality, 1, ItemData.MAX_QUALITY, textBoxWidth);
                    itemBinder.quality.AddListener(this);
                    LabeledControl labeledQualityBox = new LabeledControl("Quality", qualityBox, labeledControlWidth);

                    basicInfo.Controls.Add(labeledQualityBox);
                    basicInfo.Controls.Add(labeledDegradationBox);

                    if (itemBinder.magazineSize > 0)
                    {
                        magazineBox = new TextBoxInt(itemBinder.meta, 0, itemBinder.magazineSize, textBoxWidth);
                        LabeledControl labeledMagazineBox = new LabeledControl("Ammo loaded", magazineBox, labeledControlWidth);

                        basicInfo.Controls.Add(labeledMagazineBox);
                    }

                    if (itemBinder.partNames != null)
                    {
                        qualityBox.Enabled = false;
                        degradationBox.Enabled = false;

                        if (itemBinder.HasAllParts())
                        {
                            qualityBox.Text = itemBinder.GetQualityFromParts().ToString();
                        }

                        else
                        {
                            magazineBox.Enabled = false;
                            qualityBox.Text = "";
                            magazineBox.Text = "";
                        }

                        degradationBox.Text = "";
                    }

                    if (itemBinder.magazineItems != null && itemBinder.magazineItems.Length > 1)
                    {
                        ComboBox magazineItems = new ComboBox();
                        magazineItems.BindingContext = new BindingContext();
                        magazineItems.DataSource = itemBinder.magazineItems;
                        magazineItems.SelectedIndex = itemBinder.selectedAmmoTypeIndex.Get();

                        magazineItems.Width = textBoxWidth;
                        magazineItems.DropDownStyle = ComboBoxStyle.DropDownList;
                        magazineItems.DropDownClosed += (sender, e) =>
                        {
                            itemBinder.selectedAmmoTypeIndex.Set((byte)magazineItems.SelectedIndex);
                        };

                        LabeledControl magazineComboBox = new LabeledControl("Selected ammo", magazineItems, labeledControlWidth);
                        basicInfo.Controls.Add(magazineComboBox);
                    }
                }
            }

            return basicInfo;
        }

        protected void SetImage()
        {
            imageLabel.Anchor = AnchorStyles.None;
            imageLabel.Text = "";
            imageLabel.BackgroundImage = null;

            Bitmap image = itemBinder.GetImage(IconData.ICON_WIDTH, IconData.ICON_HEIGHT);

            if (image != null)
            {
                imageLabel.BackgroundImage = image;
            }

            else
            {
                imageLabel.Text = "(" + itemBinder.name + ")";
                imageLabel.TextAlign = ContentAlignment.MiddleCenter;
            }
        }

        public virtual void ValueUpdated(Value<int> source)
        {

        }
    }
}
