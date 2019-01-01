using System;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    class TileEntitySecureDoor : TileEntitySecure {

        public TileEntitySecureDoor() {}

        internal TileEntitySecureDoor(TypedBinaryReader reader, AdditionalFileData xmlData) {
            Read(reader, xmlData);
        }

        internal override void Read(TypedBinaryReader reader, AdditionalFileData xmlData) {
            base.Read(reader, xmlData);
        }

        internal override void Write(TypedBinaryWriter writer) {
            base.Write(writer);
        }
    }
}
