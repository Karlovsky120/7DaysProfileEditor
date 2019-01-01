using System;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    public class BaseObjective {

        //currentValue
        public Value<byte> currentValue;

        //CurrentVersion
        public Value<byte> currentVersion;

        public BaseObjective() {}

        internal BaseObjective(TypedBinaryReader reader) {
            Read(reader);
        }

        internal virtual void Read(TypedBinaryReader reader) {
            currentVersion = new Value<byte>(reader);
            currentValue = new Value<byte>(reader);
        }

        internal void Write(TypedBinaryWriter writer) {
            currentVersion.Write(writer);
            currentValue.Write(writer);
        }
    }
}