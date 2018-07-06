using System;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class StatModifierMax : StatModifier {

        //F
        public Value<float> unknownF;

        //V
        public Value<float> unknownV;

        public override void Read(BinaryReader reader, int version) {
            base.Read(reader, version);
            unknownV = new Value<float>(reader.ReadSingle());
            unknownF = new Value<float>(reader.ReadSingle());
        }

        public override void Write(BinaryWriter writer) {
            base.Write(writer);
            writer.Write(unknownV.Get());
            writer.Write(unknownF.Get());
        }
    }
}