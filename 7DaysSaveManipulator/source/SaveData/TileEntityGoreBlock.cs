using System;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    class TileEntityGoreBlock : TileEntityLootContainer {

        //tickTimeToRemove
        Value<ulong> tickTimeToRemove;

        public TileEntityGoreBlock() {}

        internal TileEntityGoreBlock(TypedBinaryReader reader, XmlData xmlData) {
            Read(reader, xmlData);
        }

        internal override void Read(TypedBinaryReader reader, XmlData xmlData) {
            base.Read(reader, xmlData);
            tickTimeToRemove = new Value<ulong>(reader);
        }

        internal override void Write(TypedBinaryWriter writer) {
            base.Write(writer);
            tickTimeToRemove.Write(writer);
        }
    }
}
