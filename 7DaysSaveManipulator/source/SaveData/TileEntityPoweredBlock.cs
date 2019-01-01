using System;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    class TileEntityPoweredBlock : TileEntityPowered {

        public TileEntityPoweredBlock() {}

        internal TileEntityPoweredBlock(TypedBinaryReader reader) {
            Read(reader);
        }

        internal override void Read(TypedBinaryReader reader, AdditionalFileData xmlData = null) {
            base.Read(reader);
        }

        internal override void Write(TypedBinaryWriter writer) {
            base.Write(writer);
        }
    }
}
