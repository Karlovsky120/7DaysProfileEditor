using SevenDaysProfileEditor.GUI;
using SevenDaysProfileEditor.Inventory;
using SevenDaysSaveManipulator;
using SevenDaysSaveManipulator.GameData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.Inventory
{
    public class ItemSlot : InventorySlot
    {
        private ViewPanel viewPanel;

        private TableLayoutPanel parts;
        private TableLayoutPanel attachments;
        private ScrollPanel scrollPanel;

        public ItemSlot(ItemBinder itemBinder, ViewPanel viewPanel, int textBoxWidth, int labeledControlWidth) : base(itemBinder, textBoxWidth, labeledControlWidth)
        {
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

            if (parts != null || attachments != null)
            {
                scrollPanel = new ScrollPanel();
                scrollPanel.Size = new Size(676, 248);

                if (parts != null)
                {
                    scrollPanel.Controls.Add(parts, 0, 0);
                }


                if (attachments != null)
                {
                    scrollPanel.Controls.Add(attachments, 1, 0);
                }

                Controls.Add(scrollPanel, 1, 0);
            }
        }

        public override void OverrideBug()
        {
            selector.SelectedItem = selectedItem;

            if (parts != null)
            {
                foreach (PartSlot slotPart in parts.Controls)
                {
                    slotPart.OverrideBug();
                }
            }

            if (attachments != null)
            {
                foreach (AttachmentSlot slotAttachment in attachments.Controls)
                {
                    slotAttachment.OverrideBug();
                }
            }
        }

        protected override void SelectionChangedAction()
        {
            viewPanel.SetImage(itemBinder);

            if (scrollPanel != null)
            {
                Controls.Remove(scrollPanel);
            }

            parts = GenerateParts();
            attachments = GenerateAttachments();

            if (parts != null || attachments != null)
            {
                scrollPanel = new ScrollPanel();
                scrollPanel.Size = new Size(676, 248);

                if (parts != null)
                {
                    scrollPanel.Controls.Add(parts, 0, 0);
                }


                if (attachments != null)
                {
                    scrollPanel.Controls.Add(attachments, 1, 0);
                }

                Controls.Add(scrollPanel, 1, 0);
            }
        }

        private TableLayoutPanel GenerateParts()
        {
            if (itemBinder.partNames != null)
            {
                TableLayoutPanel parts = new TableLayoutPanel();
                parts.Size = new Size(670, 226);
                parts.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;

                for (int i = 0; i < 4; i++)
                {                    
                        ItemBinder part = new ItemBinder(itemBinder.parts[i]);
                        PartSlot slotPart = new PartSlot(part, this, i, 80, 180);

                        parts.Controls.Add(slotPart, i / 2, i % 2);                   
                }

                return parts;
            }

            return null;
        }

        private TableLayoutPanel GenerateAttachments()
        {
            if (itemBinder.attachmentNames != null)
            {
                TableLayoutPanel attachments = new TableLayoutPanel();
                attachments.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;
                attachments.AutoSize = true;

                string[] addedAttachments = new string[itemBinder.attachments.Count];

                for (int i = 0; i < itemBinder.attachments.Count; i++)
                {
                    ItemBinder attachment = new ItemBinder(itemBinder.attachments[i]);
                    AttachmentSlot slotAttachment = new AttachmentSlot(attachment, this, i, 80, 180);

                    attachments.Controls.Add(slotAttachment, i / 2, i % 2);
                }

                return attachments;
            }

            return null;
        }

        public override void ValueUpdated(Value<int> source)
        {
            if (source.Equals(itemBinder.quality))
            {
                degradationBox.updateMax(itemBinder.GetMaxDegradationForQuality());
            }
        }
    }
}