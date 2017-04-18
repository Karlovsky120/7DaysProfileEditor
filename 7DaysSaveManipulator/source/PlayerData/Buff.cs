using System;
using System.Collections.Generic;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public abstract class Buff {

        //num
        public static Value<int> buffClassId;

        //version = 6
        public static Value<int> buffVersion;

        //D
        public static Dictionary<EnumBuffClassId, Type> dictionary;

        //W
        public List<BuffModifier> buffModifierList;

        //C
        public BuffDescriptor descriptor;

        //RandomLetter
        public Value<int> instigatorId;

        //G
        public Value<bool> isOverriden;

        //O
        public List<StatModifier> statModifierList;

        //E
        public BuffTimer timer;

        static Buff() {
            dictionary = new Dictionary<EnumBuffClassId, Type>();
            RegisterClass(EnumBuffClassId.MultiBuff, typeof(MultiBuff));
        }

        public static Buff Read(BinaryReader reader, Dictionary<ushort, StatModifier> idTable) {
            buffVersion = new Value<int>((int)reader.ReadUInt16());
            buffClassId = new Value<int>((int)reader.ReadByte());

            Type type;
            dictionary.TryGetValue((EnumBuffClassId)buffClassId.Get(), out type);

            Buff buff = Activator.CreateInstance(type, null) as Buff;
            buff.Read(reader, buffVersion.Get(), idTable);
            return buff;
        }

        public static void RegisterClass(EnumBuffClassId classId, Type type) {
            dictionary[classId] = type;
        }

        public virtual void Read(BinaryReader reader, int buffVersion, Dictionary<ushort, StatModifier> idTable) {
            timer = BuffTimer.Read(reader);
            descriptor = BuffDescriptor.Read(reader);
            isOverriden = new Value<bool>(reader.ReadBoolean());

            int statModifierListCount = reader.ReadByte();
            statModifierList = new List<StatModifier>();
            for (int i = 0; i < statModifierListCount; i++) {
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
            writer.Write((ushort)buffVersion.Get());
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