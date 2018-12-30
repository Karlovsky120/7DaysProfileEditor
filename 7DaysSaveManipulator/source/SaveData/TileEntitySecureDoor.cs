using System;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    class TileEntitySecureDoor : TileEntitySecure {

        public TileEntitySecureDoor() {}

        internal TileEntitySecureDoor(TypedBinaryReader reader, XmlData xmlData) {
            Read(reader, xmlData);
        }

        internal override void Read(TypedBinaryReader reader, XmlData xmlData) {
            base.Read(reader, xmlData);
        }

        internal override void Write(TypedBinaryWriter writer) {
            base.Write(writer);
        }
    }
}
