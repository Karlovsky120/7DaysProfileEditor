using System;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class MultiBuffVariable {

        //notSaved = 1
        public static Value<int> multiBuffVariableVersion;

        //C
        public Value<float> unknownC;

        //J
        public Value<float> unknownJ;

        //Q
        public Value<float> unknownQ;

        //S
        public Value<float> unknownS;

        public static MultiBuffVariable Read(BinaryReader reader) {
            multiBuffVariableVersion = new Value<int>(reader.ReadInt32());
            return new MultiBuffVariable() {
                unknownQ = new Value<float>(reader.ReadSingle()),
                unknownJ = new Value<float>(reader.ReadSingle()),
                unknownS = new Value<float>(reader.ReadSingle()),
                unknownC = new Value<float>(reader.ReadSingle())
            };
        }

        public void Write(BinaryWriter writer) {
            writer.Write(multiBuffVariableVersion.Get());
            writer.Write(unknownQ.Get());
            writer.Write(unknownJ.Get());
            writer.Write(unknownS.Get());
            writer.Write(unknownC.Get());
        }
    }
}