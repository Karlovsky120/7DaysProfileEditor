using System;
using System.Collections.Generic;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public abstract class BuffModifier {

        //num
        public static Value<int> buffModifierClassId;

        //version = 1
        public static Value<int> buffModifierVersion;

        //C
        public static Dictionary<EnumBuffModifierClassId, Type> dictionary;

        //J
        public Buff buff;

        //S
        public EnumBuffModifierClassId enumId;//S

        //Q
        public Value<int> UID;

        static BuffModifier() {
            dictionary = new Dictionary<EnumBuffModifierClassId, Type>();
            RegisterClass(EnumBuffModifierClassId.BuffModifierSetTickRate, typeof(BuffModifierSetTickRate));
        }

        public static BuffModifier Read(BinaryReader reader) {
            buffModifierVersion = new Value<int>(reader.ReadInt32());
            buffModifierClassId = new Value<int>((int)reader.ReadByte());

            Type type;
            dictionary.TryGetValue((EnumBuffModifierClassId)buffModifierClassId.Get(), out type);

            BuffModifier buffModifier = Activator.CreateInstance(type, null) as BuffModifier;
            buffModifier.enumId = (EnumBuffModifierClassId)buffModifierClassId.Get();
            buffModifier.Read(reader, buffModifierVersion.Get());
            return buffModifier;
        }

        public static void RegisterClass(EnumBuffModifierClassId classId, Type type) {
            dictionary[classId] = type;
        }

        public virtual void Read(BinaryReader reader, int version) {
            UID = new Value<int>(reader.ReadInt32());
        }

        public virtual void Write(BinaryWriter writer) {
            writer.Write(buffModifierVersion.Get());
            writer.Write((byte)buffModifierClassId.Get());
            writer.Write(UID.Get());
        }
    }
}