using SevenDaysSaveManipulator.GameData;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.Inventory
{
    class WindowParts : Form
    {
        BinderItem[] partList;
        List<BinderItem> attachmentList;

        private BinderItem[] sortList(BinderItem[] list, string[] nameList)
        {
            BinderItem[] sortedList = new BinderItem[nameList.Length];
            int airSpacesAlreadyIn = 0;

            for (int i = 0; i < nameList.Length; i++)
            {
                bool filled = false;

                foreach (BinderItem itemBinder in list)
                {
                    if ((itemBinder != null) && (itemBinder.name.Equals(nameList[i])))
                    {
                        sortedList[i] = itemBinder;
                        filled = true;
                    }
                }

                if (!filled)
                {
                    int airSpaces = 0;
                    foreach (BinderItem itemBinder in list)
                    {
                        if ((itemBinder != null) && (itemBinder.name.Equals("air")))
                        {
                            airSpaces++;
                            if (airSpaces > airSpacesAlreadyIn)
                            {
                                sortedList[i] = itemBinder;
                                airSpacesAlreadyIn++;
                                break;
                            }                            
                        }
                    }
                }
            }

            return sortedList;
        }

        public WindowParts(BinderItem parent, SlotInventory slotInventory)
        {
            Height = 600;
            Width = 500;

            FormClosed += (sender, e) =>
            {
                bool empty = true;
                for (int i = 0; i < 4; i++)
                {
                    if (parent.parts[i].type.get() != 0)
                    {
                        empty = false;
                    }
                }

                if (empty)
                {
                    slotInventory.selectedItem = "air";
                    slotInventory.resetSlot();
                }

                else
                {
                    int quality = parent.getQuality();
                    if (quality == 0)
                    {
                        slotInventory.qualityBox.Text = "";
                    }

                    else
                    {
                        slotInventory.qualityBox.Text = quality.ToString();
                    }

                    parent.itemValue.quality.set(quality);
                }               
            };

            TableLayoutPanel mainPanel = new TableLayoutPanel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.BackColor = System.Drawing.Color.Yellow;
            mainPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            TableLayoutPanel parts = new TableLayoutPanel();
            parts.Dock = DockStyle.Fill;
            parts.Height = 340;

            Label partsLabel = new Label();
            partsLabel.Text = "Parts";
            parts.Controls.Add(partsLabel, 0, 0);

            partList = new BinderItem[4];

            for (int i = 0; i < 4; i++)
            {
                partList[i] = new BinderItem(parent.itemValue.parts[i]);  
            }

            partList = sortList(partList, parent.partNames);

            int k = 0;

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    string[] list = new string[2] { "air", parent.partNames[k] };

                    parts.Controls.Add(new SlotInventory(partList[k++], list), i, j + 1);
                }
            }
            
            mainPanel.Controls.Add(parts, 0, 1);

            if (parent.attachmentNames != null)
            {
                TableLayoutPanel attachments = new TableLayoutPanel();
                attachments.Dock = DockStyle.Fill;

                Label attachmentsLabel = new Label();
                attachmentsLabel.Text = "Attachments";
                attachments.Controls.Add(attachmentsLabel, 0, 0);

                attachmentList = new List<BinderItem>();

                for (int i = 0; i < parent.attachmentNames.Length; i++)
                {
                    if ((i < parent.attachments.Count) && (parent.attachments[i] != null))
                    {
                        attachmentList.Add(new BinderItem(parent.attachments[i]));
                    }

                    else
                    {
                        if (i < parent.attachments.Count)
                        {
                            attachmentList[i] = new BinderItem(DataItem.getItemDataByName("air"));
                            parent.attachments[i] = attachmentList[i].itemValue;
                        }

                        else
                        {
                            attachmentList.Add(new BinderItem(DataItem.getItemDataByName("air")));
                            parent.attachments.Add(attachmentList[i].itemValue);
                        }

                        
                    }
                }

                k = 0;

                for (int i = 0; i < (parent.attachmentNames.Length / 2) + 1; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (k < attachmentList.Count)
                        {
                            string[] nameList = new string[2] { "air", parent.attachmentNames[k] };

                            attachments.Controls.Add(new SlotInventory(attachmentList[k++], nameList), j, i + 1);
                        }
                    }
                }

                mainPanel.Controls.Add(attachments, 0, 3);
            }

            Controls.Add(mainPanel);
            ShowDialog();
        }
    }
}
