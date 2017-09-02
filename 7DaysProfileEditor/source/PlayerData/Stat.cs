using System;
using System.Collections.Generic;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class Stat {

        //Q
        public Value<float> baseMax;

        //S
        public Value<float> maxModifier;

        //O
        public Value<float> originalMax;

        //W
        public Value<float> originalValue;

        //V
        public List<StatModifier> statModifierList;

        //num = 5
        public Value<int> statVersion;

        //G
        public Value<bool> unknownG;

        //J
        public Value<float> value;

        //C
        public Value<float> valueModifier;

        public void Read(BinaryReader reader, Dictionary<ushort, StatModifier> idTable) {
            statVersion = new Value<int>(reader.ReadInt32());
            value = new Value<float>(reader.ReadSingle());
            maxModifier = new Value<float>(reader.ReadSingle());
            valueModifier = new Value<float>(reader.ReadSingle());
            baseMax = new Value<float>(reader.ReadSingle());
            originalMax = new Value<float>(reader.ReadSingle());
            originalValue = new Value<float>(reader.ReadSingle());
            unknownG = new Value<bool>(reader.ReadBoolean());

            //num3
            int statModifierListCount = reader.ReadInt32();
            statModifierList = new List<StatModifier>();
            for (int j = 0; j < statModifierListCount; j++) {
                StatModifier statModifier = StatModifier.Read(reader);
                statModifier.stat = this;
                statModifierList.Add(statModifier);
                idTable[statModifier.fileId.Get()] = statModifier;
            }
        }

        public void Write(BinaryWriter writer) {
            writer.Write(statVersion.Get());
            writer.Write(value.Get());
            writer.Write(maxModifier.Get());
            writer.Write(valueModifier.Get());
            writer.Write(baseMax.Get());
            writer.Write(originalMax.Get());
            writer.Write(originalValue.Get());
            writer.Write(unknownG.Get());
            writer.Write(statModifierList.Count);

            List<StatModifier>.Enumerator enumerator = statModifierList.GetEnumerator();
            while (enumerator.MoveNext()) {
                enumerator.Current.Write(writer);
            }
        }
    }
}