using System;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class BuffModifierSetTickRate : BuffModifier {

        //O
        public Value<float> tickRate;

        public override void Read(BinaryReader reader, int version) {
            base.Read(reader, version);
            tickRate = new Value<float>(reader.ReadSingle());
        }

        public override void Write(BinaryWriter writer) {
            base.Write(writer);
            writer.Write(tickRate.Get());
        }
    }
}