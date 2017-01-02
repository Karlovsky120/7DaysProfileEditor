using System;
using System.IO;

namespace SevenDaysSaveManipulator.GameData {

    [Serializable]
    public class BaseObjective {

        //CurrentValue
        public Value<byte> currentValue;

        //CurrentVersion
        public Value<byte> currentVersion;

        public virtual void Read(BinaryReader reader) {
            currentVersion = new Value<byte>(reader.ReadByte());
            currentValue = new Value<byte>(reader.ReadByte());
        }

        public void Write(BinaryWriter writer) {
            writer.Write(currentVersion.Get());
            writer.Write(currentValue.Get());
        }
    }
}