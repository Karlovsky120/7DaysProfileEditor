using System;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class BuffTimerDuration : BuffTimer {

        //O
        public Value<float> duration;

        //C
        public Value<float> elapsed;

        public override void Read(BinaryReader reader, int version) {
            base.Read(reader, version);
            elapsed = new Value<float>(reader.ReadSingle());
            duration = new Value<float>(reader.ReadSingle());
        }

        public override void Write(BinaryWriter writer) {
            base.Write(writer);
            writer.Write(elapsed.Get());
            writer.Write(duration.Get());
        }
    }
}