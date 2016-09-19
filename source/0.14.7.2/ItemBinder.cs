using SevenDaysSaveManipulator.GameData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SevenDaysProfileEditor.Inventory
{
    public class ItemBinder
    {
        public ItemStack itemStack;
        public ItemValue itemValue;
        public ItemData itemData;

        public Value<int> count;

        public Value<int> type;
        public Value<int> quality;
        public Value<int> useTimes;
        public Value<int> meta;
        public Value<byte> selectedAmmoTypeIndex;
        public Value<bool> activated;
        public ItemValue[] parts;
        public List<ItemValue> attachments;

        public int id;
        public string name;
        public int stackNumber;

        public bool isBlock;
        public bool hasQuality;
        public int degradationMin;
        public int degradationMax;
        public int magazineSize;
        public string partType;
        public string[] attachmentNames;
        public string[] partNames;
        public string[] magazineItems;
        public byte[] iconPixels;

        public bool isDeveloper;

        public ItemBinder(ItemStack itemStack)
        {
            this.itemStack = itemStack;
            count = itemStack.count;

            ReferenceValueAndData(itemStack.itemValue);
        }

        public ItemBinder(ItemValue itemValue)
        {
            ReferenceValueAndData(itemValue);
        }

        private void ReferenceValueAndData(ItemValue itemValue)
        {
            this.itemValue = itemValue;

            type = itemValue.type;
            quality = itemValue.quality;
            useTimes = itemValue.useTimes;
            meta = itemValue.meta;
            selectedAmmoTypeIndex = itemValue.selectedAmmoTypeIndex;
            activated = itemValue.activated;
            parts = itemValue.parts;
            attachments = itemValue.attachments;

            itemData = ItemData.GetItemDataById(type.Get());

            id = itemData.id;
            name = itemData.name;
            stackNumber = itemData.stackNumber;
            isBlock = itemData.isBlock;
            hasQuality = itemData.hasQuality;
            degradationMin = itemData.degradationMin;
            degradationMax = itemData.degradationMax;
            magazineSize = itemData.magazineSize;
            partType = itemData.partType;
            attachmentNames = itemData.attachmentNames;
            partNames = itemData.partNames;
            magazineItems = itemData.magazineItems;
            iconPixels = itemData.iconPixels;
            isDeveloper = itemData.isDeveloper;
        }

        public void ResetItemBinder(ItemData itemData)
        {
            if (itemStack != null)
            {
                count.Set(itemData.stackNumber);
            }

            ItemBinder.ResetItemValue(itemData, itemValue);

            ReferenceValueAndData(itemValue);
        }

        private static void ResetItemValue(ItemData itemData, ItemValue itemValue)
        {
            itemValue.type = new Value<int>(itemData.id);
            itemValue.useTimes = new Value<int>(0);
            itemValue.quality = new Value<int>(0);
            itemValue.meta = new Value<int>(0);
            itemValue.selectedAmmoTypeIndex = new Value<byte>(0);
            itemValue.activated = new Value<bool>(false);
            itemValue.parts = new ItemValue[4];
            itemValue.attachments = new List<ItemValue>();

            if (itemData.name != "air")
            {
                if (itemData.hasQuality)
                {
                    itemValue.quality.Set(ItemData.MAX_QUALITY);

                    if (itemData.partNames != null)
                    {
                        itemValue.parts = new ItemValue[4];

                        int quality = 0;

                        for (int i = 0; i < 4; i++)
                        {
                            itemValue.parts[i] = new ItemValue();
                            itemValue.parts[i].itemValueVersion = new Value<byte>(3);
                            ItemBinder.ResetItemValue(ItemData.GetItemDataByName(itemData.partNames[i]), itemValue.parts[i]);
                            quality += itemValue.parts[i].quality.Get();
                        }

                        itemValue.quality.Set(quality / 4);
                    }

                    if (itemData.attachmentNames != null)
                    {
                        itemValue.attachments = new List<ItemValue>();

                        for (int i = 0; i < itemData.attachmentNames.Length; i++)
                        {
                            itemValue.attachments.Add(new ItemValue());
                            itemValue.attachments[i].itemValueVersion = new Value<byte>(3);
                            ItemBinder.ResetItemValue(ItemData.GetItemDataByName(itemData.attachmentNames[i]), itemValue.attachments[i]);
                        }
                    }
                }
            }
        }

        public bool HasAllParts()
        {
            if (parts.Length > 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    if ((parts[i] == null) || (parts[i].type.Get() == 0))
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        public int GetMaxDegradationForQuality()
        {
            return (int)(((degradationMax - degradationMin) / (float)ItemData.MAX_QUALITY) * quality.Get() + degradationMin);
        }

        public int GetQualityFromParts()
        {
            if (HasAllParts())
            {
                int quality = 0;

                foreach (ItemValue part in parts)
                {
                    quality += part.quality.Get();
                }

                return quality / 4;
            }

            return 0;
        }

        public Bitmap GetImage(int width, int height)
        {
            if (iconPixels != null)
            {
                Bitmap bmp = new Bitmap(width, height);

                PixelFormat pxf = PixelFormat.Format32bppArgb;
                BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, pxf);
                IntPtr ptr = bmpData.Scan0;
                Marshal.Copy(iconPixels, 0, ptr, width * height * 4);
                bmp.UnlockBits(bmpData);

                return bmp;
            }

            return null;
        }

        public static ItemBinder GetAir()
        {
            ItemValue air = new ItemValue();
            ResetItemValue(ItemData.GetItemDataById(0), air);

            return new ItemBinder(air);
        }
    }
}