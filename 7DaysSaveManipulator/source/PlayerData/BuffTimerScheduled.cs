using System;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class BuffTimerScheduled : BuffTimer {

        //num = 3
        public Value<int> buffTimerScheduledVersion;

        //O
        public Value<int> duration;

        //W
        public Value<int> elapsed;

        //E
        public Value<float> unknownE;

        //G
        public Value<ulong> unknownG;

        public override void Read(BinaryReader reader, int buffVersion) {
            base.Read(reader, buffVersion);
            buffTimerScheduledVersion = new Value<int>(reader.ReadInt32());

            unknownG = new Value<ulong>(reader.ReadUInt64());
            duration = new Value<int>(reader.ReadInt32());
            unknownE = new Value<float>(reader.ReadSingle());
            elapsed = new Value<int>(reader.ReadInt32());
        }

        public override void Write(BinaryWriter writer) {
            base.Write(writer);
            writer.Write(buffTimerScheduledVersion.Get());
            writer.Write(unknownG.Get());
            writer.Write(duration.Get());
            writer.Write(unknownE.Get());
            writer.Write(elapsed.Get());
        }
    }
}