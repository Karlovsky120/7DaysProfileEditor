using System;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class ItemStack {

        //count
        public Value<ushort> count;

        //itemValue
        public ItemValue itemValue;

        public static ItemStack[] ReadItemStack(BinaryReader reader) {
            ushort itemStackLength = reader.ReadUInt16();
            ItemStack[] array = new ItemStack[itemStackLength];
            for (int i = 0; i < itemStackLength; ++i) {
                array[i] = new ItemStack(reader);
            }

            return array;
        }

        public static void WriteItemStack(BinaryWriter writer, ItemStack[] itemStacks) {
            writer.Write((ushort)itemStacks.Length);
            for (int i = 0; i < itemStacks.Length; ++i) {
                itemStacks[i].Write(writer);
            }
        }

        public void Read(BinaryReader reader) {
            itemValue = new ItemValue(reader);
            count = new Value<ushort>(reader.ReadUInt16());
        }

        public void Write(BinaryWriter writer) {
            itemValue.Write(writer);
            writer.Write(count.Get());
        }
    }
}