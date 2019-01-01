using System;
using System.Collections.Generic;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    class TraderData {

        //traderID
        public Value<int> traderID;

        //lastInventoryUpdate
        Value<ulong> lastInventoryUpdate;

        //primatyInventory
        List<ItemStack> primaryInventory;

        //TierItemGroups
        List<List<ItemStack>> tierItemGroups;

        //AvailableMoney
        Value<int> availableMoney;

        //priceMarkupList
        List<sbyte> priceMarkupList;

        public TraderData() {}

        internal TraderData(TypedBinaryReader reader, AdditionalFileData xmlData) {
            Read(reader, xmlData);
        }

        internal void Read(TypedBinaryReader reader, AdditionalFileData xmlData) {
            traderID = new Value<int>(reader);
            lastInventoryUpdate = new Value<ulong>(reader);

            Utils.VerifyVersion(reader.ReadByte(), SaveVersionConstants.TRADER_DATA, "TraderData");

            ushort itemStackLength = reader.ReadUInt16();
            primaryInventory = new List<ItemStack>(itemStackLength);
            for (int i = 0; i < itemStackLength; ++i) {
                primaryInventory.Add(new ItemStack(reader, xmlData));
            }

            byte tierItemGroupsCount = reader.ReadByte();
            tierItemGroups = new List<List<ItemStack>>(tierItemGroupsCount);
            for (byte i = 0; i < tierItemGroupsCount; ++i) {
                ushort itemGroupLength = reader.ReadUInt16();
                tierItemGroups[i] = new List<ItemStack>();
                for (int j = 0; j < itemGroupLength; ++j) {
                    tierItemGroups[i].Add(new ItemStack(reader, xmlData));
                }
                tierItemGroups.Add(tierItemGroups[i]);
            }

            availableMoney = new Value<int>(reader);

            int priceMarkupCount = reader.ReadInt32();
            priceMarkupList = new List<sbyte>(priceMarkupCount);
            for (int i = 0; i < priceMarkupCount; ++i) {
                priceMarkupList.Add(reader.ReadSByte());
            }
        }

        internal void Write(TypedBinaryWriter writer) {
            traderID.Write(writer);
            lastInventoryUpdate.Write(writer);
            writer.Write(SaveVersionConstants.TRADER_DATA);

            writer.Write((ushort)primaryInventory.Count);
            foreach (ItemStack inventorySlot in primaryInventory) {
                inventorySlot.Write(writer);
            }

            writer.Write((byte)tierItemGroups.Count);
            foreach (List<ItemStack> itemGroup in tierItemGroups) {
                foreach(ItemStack item in itemGroup) {
                    item.Write(writer);
                }
            }

            availableMoney.Write(writer);

            writer.Write(priceMarkupList.Count);
            foreach (float priceMarkup in priceMarkupList) {
                writer.Write(priceMarkup);
            }
        }
    }
}
