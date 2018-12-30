using System;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    class TileEntitySleeper : TileEntity {

        //priorityMultiplier
        public Value<float> priorityMultiplier;

        //sightRange
        public Value<short> sightRange;

        //hearingPercent
        public Value<float> hearingPercent;

        //sightAngle
        public Value<short> sightAngle;

        public TileEntitySleeper() { }

        internal TileEntitySleeper(TypedBinaryReader reader) {
            Read(reader);
        }

        internal override void Read(TypedBinaryReader reader, XmlData xmlData = null) {
            base.Read(reader);

            priorityMultiplier = new Value<float>(reader);
            sightRange = new Value<short>(reader);
            hearingPercent = new Value<float>(reader);
            sightAngle = new Value<short>(reader);
        }

        internal override void Write(TypedBinaryWriter writer) {
            base.Write(writer);

            priorityMultiplier.Write(writer);
            sightRange.Write(writer);
            hearingPercent.Write(writer);
            sightAngle.Write(writer);
        }
    }
}
