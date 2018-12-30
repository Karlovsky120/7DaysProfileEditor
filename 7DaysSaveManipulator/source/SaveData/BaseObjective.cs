using System;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    public class BaseObjective {

        //CurrentValue
        public Value<byte> currentValue;

        public BaseObjective() {}

        internal BaseObjective(TypedBinaryReader reader) {
            Read(reader);
        }

        internal virtual void Read(TypedBinaryReader reader) {
            Utils.VerifyVersion(reader.ReadByte(), SaveVersionConstants.BASE_OBJECTIVE);
            currentValue = new Value<byte>(reader);
        }

        internal void Write(TypedBinaryWriter writer) {
            writer.Write(SaveVersionConstants.BASE_OBJECTIVE);
            currentValue.Write(writer);
        }
    }
}