using System;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class LiveStats {

        //L
        public Value<float> exhaustionLevel;

        //S
        public Value<int> lifeLevel;

        //P
        public Value<float> saturationLevel;

        //F
        public Value<int> unknownW;

        public void Read(BinaryReader reader) {
            lifeLevel = new Value<int>((int)reader.ReadInt16());
            unknownW = new Value<int>((int)reader.ReadInt16());
            saturationLevel = new Value<float>(reader.ReadSingle());
            exhaustionLevel = new Value<float>(reader.ReadSingle());
        }

        public void Write(BinaryWriter writer) {
            writer.Write((ushort)lifeLevel.Get());
            writer.Write((ushort)unknownW.Get());
            writer.Write(saturationLevel.Get());
            writer.Write(exhaustionLevel.Get());
        }
    }
}