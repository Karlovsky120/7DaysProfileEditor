using SevenDaysSaveManipulator.source.PlayerData;
using System;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class BaseObjective {

        //CurrentValue
        public Value<byte> currentValue;

        public BaseObjective() {}

        internal BaseObjective(BinaryReader reader) {
            Read(reader);
        }

        internal virtual void Read(BinaryReader reader) {
            Utils.VerifyVersion<byte>(reader.ReadByte(), SaveVersionConstants.BASE_OBJECTIVE);
            currentValue = new Value<byte>(reader.ReadByte());
        }

        internal void Write(BinaryWriter writer) {
            writer.Write(SaveVersionConstants.BASE_OBJECTIVE);
            writer.Write(currentValue.Get());
        }
    }
}