using System;
using System.Collections.Generic;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    public class TileEntityLootContainer : TileEntity {

        //lootListIndex
        Value<ushort> lootListIndex;

        //containerSize
        Vector2D<ushort> containerSize;

        //bTouched
        Value<bool> bTouched;

        //worldTimeTouched
        Value<uint> worldTimeTouched;

        //bPlayerBackpach
        Value<bool> bPlayerBackpack;

        //items
        List<ItemStack> items;

        //bPlayerStorage
        Value<bool> bPlayerStorage;

        public TileEntityLootContainer() {}

        internal TileEntityLootContainer(TypedBinaryReader reader, XmlData xmlData) {
            Read(reader, xmlData);
        }

        internal override void Read(TypedBinaryReader reader, XmlData xmlData) {
            base.Read(reader);

            lootListIndex = new Value<ushort>(reader);
            containerSize = new Vector2D<ushort>(reader);

            bTouched = new Value<bool>(reader);
            worldTimeTouched = new Value<uint>(reader);
            bPlayerBackpack = new Value<bool>(reader);

            int itemsLength = Math.Min((int)reader.ReadInt16(), containerSize.x.Get() * containerSize.y.Get());
            items = new List<ItemStack>(containerSize.x.Get() * containerSize.y.Get());
            for (int i = 0; i < itemsLength; ++i) {
                items[i] = new ItemStack(reader, xmlData);
            }

            bPlayerStorage = new Value<bool>(reader);
        }

        internal override void Write(TypedBinaryWriter writer) {
            base.Write(writer);

            lootListIndex.Write(writer);
            containerSize.Write(writer);
            bTouched.Write(writer);
            worldTimeTouched.Write(writer);
            bPlayerBackpack.Write(writer);

            writer.Write((short)items.Count);
            foreach(ItemStack itemStack in items) {
                itemStack.Write(writer);
            }

            bPlayerStorage.Write(writer);
        }
    }
}
