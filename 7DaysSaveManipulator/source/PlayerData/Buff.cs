using SevenDaysSaveManipulator.source.PlayerData;
using System;
using System.Collections.Generic;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public abstract class Buff {

        //num
        public static Value<int> buffClassId;

        //smth
        public static Value<ushort> buffVersion;

        //obfuscated
        public static Dictionary<EnumBuffClassId, Type> buffClassIdDictionary;

        //obfuscated
        public List<BuffModifier> buffModifierList;

        //obfuscated
        public BuffDescriptor descriptor;

        //obfuscated
        public Value<int> instigatorId;

        //obfuscated
        public Value<bool> isOverriden;

        //obfuscated
        public List<StatModifier> statModifierList;

        //obfuscated
        public BuffTimer timer;

        static Buff() {
            buffClassIdDictionary = new Dictionary<EnumBuffClassId, Type>();
            RegisterClass(EnumBuffClassId.MultiBuff, typeof(MultiBuff));
        }

        public static Buff Read(BinaryReader reader, Dictionary<ushort, StatModifier> idTable) {
            buffVersion = new Value<ushort>((ushort)reader.ReadUInt16());
            buffClassId = new Value<int>((int)reader.ReadByte());

            buffClassIdDictionary.TryGetValue((EnumBuffClassId)buffClassId.Get(), out Type type);

            Buff buff = Activator.CreateInstance(type, null) as Buff;
            buff.Read(reader, buffVersion.Get(), idTable);
            return buff;
        }

        public static void RegisterClass(EnumBuffClassId classId, Type type) {
            buffClassIdDictionary[classId] = type;
        }

        public virtual void Read(BinaryReader reader, int buffVersion, Dictionary<ushort, StatModifier> idTable) {
            timer = BuffTimer.Read(reader);
            descriptor = BuffDescriptor.Read(reader);
            isOverriden = new Value<bool>(reader.ReadBoolean());

            int statModifierListCount = reader.ReadByte();
            statModifierList = new List<StatModifier>();
            for (int i = 0; i < statModifierListCount; ++i) {
                ushort key = reader.ReadUInt16();
                StatModifier statModifier = idTable[key];
                statModifierList.Add(statModifier);
            }

            int buffModiferListCount = reader.ReadByte();
            buffModifierList = new List<BuffModifier>();
            for (int j = 0; j < buffModiferListCount; j++) {
                BuffModifier buffModifier = BuffModifier.Read(reader);
                buffModifier.buff = this;
                buffModifierList.Add(buffModifier);
            }

            instigatorId = new Value<int>(reader.ReadInt32());
        }

        public virtual void Write(BinaryWriter writer) {
            writer.Write(buffVersion.Get());
            writer.Write((byte)buffClassId.Get());
            timer.Write(writer);
            descriptor.Write(writer);
            writer.Write(isOverriden.Get());

            writer.Write((byte)statModifierList.Count);
            List<StatModifier>.Enumerator enumerator = statModifierList.GetEnumerator();
            while (enumerator.MoveNext()) {
                writer.Write(enumerator.Current.fileId.Get());
            }

            writer.Write((byte)buffModifierList.Count);
            foreach (BuffModifier modifier in buffModifierList) {
                modifier.Write(writer);
            }

            writer.Write(instigatorId.Get());
        }
    }
}