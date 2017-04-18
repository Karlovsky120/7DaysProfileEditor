using System;
using System.Collections.Generic;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public abstract class StatModifier {

        //G
        public static Dictionary<EnumStatModifierClassId, Type> dictionary;

        //num
        public static Value<int> enumStatModifierClassId;

        //version = 4
        public static Value<int> statModifierVersion;

        //S
        public EnumBuffCategoryFlags buffCategoryFlags;

        //C
        public BuffTimer buffTimer;

        //J
        public EnumStatModifierClassId enumId;

        //Q
        public Value<ushort> fileId;

        //E
        public Value<int> stackCount;

        //O
        public Stat stat;

        //W
        public Value<int> UID;

        static StatModifier() {
            dictionary = new Dictionary<EnumStatModifierClassId, Type>();
            RegisterClass(EnumStatModifierClassId.StatModifierMax, typeof(StatModifierMax));
            RegisterClass(EnumStatModifierClassId.StatModifierValueOT, typeof(StatModifierValueOT));
            RegisterClass(EnumStatModifierClassId.StatModifierModifyValue, typeof(StatModifierModifyValue));
            RegisterClass(EnumStatModifierClassId.StatModifierSetValue, typeof(StatModifierSetValue));
            RegisterClass(EnumStatModifierClassId.StatModifierMulValue, typeof(StatModifierMulValue));
        }

        public static StatModifier Read(BinaryReader reader) {
            statModifierVersion = new Value<int>(reader.ReadInt32());
            enumStatModifierClassId = new Value<int>((int)reader.ReadByte());

            Type type;
            dictionary.TryGetValue((EnumStatModifierClassId)enumStatModifierClassId.Get(), out type);

            StatModifier statModifier = Activator.CreateInstance(type, null) as StatModifier;
            statModifier.enumId = (EnumStatModifierClassId)enumStatModifierClassId.Get();
            statModifier.Read(reader, statModifierVersion.Get());
            return statModifier;
        }

        public static void RegisterClass(EnumStatModifierClassId classId, Type type) {
            dictionary[classId] = type;
        }

        public virtual void Read(BinaryReader reader, int version) {
            UID = new Value<int>(reader.ReadInt32());
            fileId = new Value<ushort>(reader.ReadUInt16());
            buffCategoryFlags = (EnumBuffCategoryFlags)reader.ReadInt32();
            stackCount = new Value<int>(reader.ReadInt32());
            buffTimer = BuffTimer.Read(reader);
        }

        public virtual void Write(BinaryWriter writer) {
            writer.Write(statModifierVersion.Get());
            writer.Write((byte)enumId);
            writer.Write(UID.Get());
            writer.Write(fileId.Get());
            writer.Write((int)buffCategoryFlags);
            writer.Write(stackCount.Get());
            buffTimer.Write(writer);
        }
    }
}