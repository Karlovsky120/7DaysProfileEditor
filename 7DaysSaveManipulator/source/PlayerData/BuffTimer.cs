using System;
using System.Collections.Generic;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public abstract class BuffTimer {

        //num
        public static Value<int> buffTimerClassId;

        //version = 2
        public static Value<int> buffTimerVersion;

        //S
        public static Dictionary<EnumBuffTimerClassId, Type> dictionary;

        //Null
        public static BuffTimerNull Null;

        //Q
        public EnumBuffTimerClassId classId;

        static BuffTimer() {
            Null = new BuffTimerNull();
            dictionary = new Dictionary<EnumBuffTimerClassId, Type>();
            RegisterClass(EnumBuffTimerClassId.Null, typeof(BuffTimerNull));
            RegisterClass(EnumBuffTimerClassId.Duration, typeof(BuffTimerDuration));
            RegisterClass(EnumBuffTimerClassId.Scheduled, typeof(BuffTimerScheduled));
        }

        public BuffTimer(EnumBuffTimerClassId classId) {
            this.classId = classId;
        }

        public BuffTimer() {
        }

        public static BuffTimer Read(BinaryReader reader) {
            buffTimerVersion = new Value<int>(reader.ReadInt32());
            buffTimerClassId = new Value<int>((int)reader.ReadByte());

            Type type;
            dictionary.TryGetValue((EnumBuffTimerClassId)buffTimerClassId.Get(), out type);

            BuffTimer buffTimer = Activator.CreateInstance(type, null) as BuffTimer;
            buffTimer.classId = (EnumBuffTimerClassId)buffTimerClassId.Get();
            buffTimer.Read(reader, buffTimerVersion.Get());
            return buffTimer;
        }

        public static void RegisterClass(EnumBuffTimerClassId classId, Type type) {
            dictionary[classId] = type;
        }

        public virtual void Read(BinaryReader reader, int version) {
        }

        public virtual void Write(BinaryWriter writer) {
            writer.Write(buffTimerVersion.Get());
            writer.Write((byte)classId);
        }
    }
}