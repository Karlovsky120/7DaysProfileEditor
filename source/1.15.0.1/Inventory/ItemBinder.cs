using SevenDaysSaveManipulator.GameData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SevenDaysProfileEditor.Inventory {

    /// <summary>
    /// Used as a binder between static data in xmls and dynamic data in the ttp file.
    /// </summary>
    internal class ItemBinder {
        public ItemData itemData;
        public ItemStack itemStack;
        public ItemValue itemValue;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="itemStack">ItemStack to be used</param>
        public ItemBinder(ItemStack itemStack) {
            this.itemStack = itemStack;

            ReferenceValueAndData(itemStack.itemValue);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="itemValue">ItemValue to be used</param>
        public ItemBinder(ItemValue itemValue) {
            ReferenceValueAndData(itemValue);
        }

        /// <summary>
        /// Creates an itemBinder that represents air.
        /// </summary>
        /// <returns>ItemBinder that represents air</returns>
        public static ItemBinder GetAir() {
            ItemValue air = new ItemValue();
            ResetItemValue(ItemData.GetItemDataById(0), air);

            return new ItemBinder(air);
        }

        /// <summary>
        /// Creates image for byte array containing the image pixel data.
        /// </summary>
        /// <param name="width">Width of the image</param>
        /// <param name="height">Height of the image</param>
        /// <returns>Bitmap image</returns>
        public Bitmap GetImage(int width, int height) {
            if (itemData.iconPixels != null) {
                Bitmap bmp = new Bitmap(width, height);

                PixelFormat pxf = PixelFormat.Format32bppArgb;
                BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, pxf);
                IntPtr ptr = bmpData.Scan0;
                Marshal.Copy(itemData.iconPixels, 0, ptr, width * height * 4);
                bmp.UnlockBits(bmpData);

                return bmp;
            }

            return null;
        }

        /// <summary>
        /// Gets max degradation for current quality level.
        /// </summary>
        /// <returns>Max degradation</returns>
        public int GetMaxDegradationForQuality() {
            return (int)(((itemData.degradationMax - itemData.degradationMin) / (float)ItemData.MAX_QUALITY) * itemValue.quality.Get() + itemData.degradationMin);
        }

        /// <summary>
        /// Generates quality based on all the parts quality.
        /// </summary>
        /// <returns>Quality</returns>
        public int GetQualityFromParts() {
            if (HasAllParts()) {
                int quality = 0;

                foreach (ItemValue part in itemValue.parts) {
                    quality += part.quality.Get();
                }

                return quality / 4;
            }

            return 0;
        }

        /// <summary>
        /// Check if item has all the parts,
        /// </summary>
        /// <returns>True if item has all the parts, false otherwise</returns>
        public bool HasAllParts() {
            if (itemValue.parts.Length > 0) {
                for (int i = 0; i < 4; i++) {
                    if ((itemValue.parts[i] == null) || (itemValue.parts[i].type.Get() == 0)) {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Resets itemBinder to specified itemData.
        /// </summary>
        /// <param name="itemData">ItemData to reset the itemBinder to</param>
        public void ResetItemBinder(ItemData itemData) {
            if (itemStack != null) {
                itemStack.count.Set(itemData.stackNumber);
            }

            ItemBinder.ResetItemValue(itemData, itemValue);

            ReferenceValueAndData(itemValue);
        }

        /// <summary>
        /// Resets itemValue to speficied itemData.
        /// </summary>
        /// <param name="itemData">ItemData to reset to</param>
        /// <param name="itemValue">ItemValue to reset</param>
        private static void ResetItemValue(ItemData itemData, ItemValue itemValue) {
            itemValue.type = new Value<int>(itemData.id);
            itemValue.useTimes = new Value<int>(0);
            itemValue.quality = new Value<int>(0);
            itemValue.meta = new Value<int>(0);
            itemValue.selectedAmmoTypeIndex = new Value<byte>(0);
            itemValue.activated = new Value<bool>(false);
            itemValue.parts = new ItemValue[4];
            itemValue.attachments = new List<ItemValue>();

            if (itemData.name != "air") {
                if (itemData.hasQuality) {
                    itemValue.quality.Set(ItemData.MAX_QUALITY);

                    if (itemData.partNames != null) {
                        itemValue.parts = new ItemValue[4];

                        int quality = 0;

                        for (int i = 0; i < 4; i++) {
                            itemValue.parts[i] = new ItemValue();
                            itemValue.parts[i].itemValueVersion = new Value<byte>(3);
                            ItemBinder.ResetItemValue(ItemData.GetItemDataByName(itemData.partNames[i]), itemValue.parts[i]);
                            quality += itemValue.parts[i].quality.Get();
                        }

                        itemValue.quality.Set(quality / 4);
                    }

                    if (itemData.attachmentNames != null) {
                        itemValue.attachments = new List<ItemValue>();

                        for (int i = 0; i < itemData.attachmentNames.Length; i++) {
                            itemValue.attachments.Add(new ItemValue());
                            itemValue.attachments[i].itemValueVersion = new Value<byte>(3);
                            ItemBinder.ResetItemValue(ItemData.GetItemDataByName(itemData.attachmentNames[i]), itemValue.attachments[i]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds references to itemValue and itemData.
        /// </summary>
        /// <param name="itemValue">ItemValue to reference</param>
        private void ReferenceValueAndData(ItemValue itemValue) {
            this.itemValue = itemValue;
            itemData = ItemData.GetItemDataById(itemValue.type.Get());
        }
    }
}