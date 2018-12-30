using System;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    class TileEntityPoweredRangedTrap : TileEntityPoweredBlock {

        //ownerID
        public Value<string> ownerID;

        public TileEntityPoweredRangedTrap() {}

        internal TileEntityPoweredRangedTrap(TypedBinaryReader reader) {
            Read(reader);
        }

        internal override void Read(TypedBinaryReader reader, XmlData xmlData = null) {
            base.Read(reader);

            if (powerItemType.Get() == PoweredItemType.RangedTrap) {
                ownerID = new Value<string>(reader);
            }
        }

        internal override void Write(TypedBinaryWriter writer) {
            base.Write(writer);

            if (powerItemType.Get() == PoweredItemType.RangedTrap) {
                ownerID.Write(writer);
            }
        }
    }
}
