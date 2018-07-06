using System;
using System.Collections.Generic;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class MultiBuff : Buff {

        //WJ
        public Dictionary<string, float> buffCounterValues;

        //JJ
        public List<MultiBuffAction> multiBuffActionList;

        //SJ
        public List<MultiBuffAction> multiBuffActionList2;

        //id
        public string multiBuffClassId;

        //OJ
        public List<MultiBuffPrefabAttachmentDescriptor> multiBuffPrefabAttachmentDescriptorList;

        //num = 2
        public Value<int> multiBuffVersion;

        public override void Read(BinaryReader reader, int buffVersion, Dictionary<ushort, StatModifier> idTable) {
            base.Read(reader, buffVersion, idTable);
            multiBuffVersion = new Value<int>(reader.ReadInt32());

            multiBuffClassId = reader.ReadString();

            //num3
            int multiBuffActionListCount = reader.ReadInt32();
            multiBuffActionList = new List<MultiBuffAction>();
            for (int i = 0; i < multiBuffActionListCount; i++) {
                MultiBuffAction item = MultiBuffAction.Read(reader);
                multiBuffActionList.Add(item);
            }

            //num4
            int multiBuffActionList2Count = reader.ReadInt32();
            multiBuffActionList2 = new List<MultiBuffAction>();
            for (int j = 0; j < multiBuffActionList2Count; j++) {
                MultiBuffAction item2 = MultiBuffAction.Read(reader);
                multiBuffActionList2.Add(item2);
            }

            //num5
            int multiBuffPrefabAttachmentDescriptorCount = reader.ReadInt32();
            multiBuffPrefabAttachmentDescriptorList = new List<MultiBuffPrefabAttachmentDescriptor>();
            for (int k = 0; k < multiBuffPrefabAttachmentDescriptorCount; k++) {
                multiBuffPrefabAttachmentDescriptorList.Add(MultiBuffPrefabAttachmentDescriptor.Read(reader));
            }

            //num6
            int buffCounterValuesCount = reader.ReadInt32();
            buffCounterValues = new Dictionary<string, float>();
            for (int l = 0; l < buffCounterValuesCount; l++) {
                string key = reader.ReadString();
                float value = reader.ReadSingle();
                buffCounterValues[key] = value;
            }
        }

        public override void Write(BinaryWriter writer) {
            base.Write(writer);
            writer.Write(multiBuffVersion.Get());
            writer.Write(multiBuffClassId);

            writer.Write(multiBuffActionList.Count);
            List<MultiBuffAction>.Enumerator enumerator = multiBuffActionList.GetEnumerator();
            while (enumerator.MoveNext()) {
                enumerator.Current.Write(writer);
            }

            writer.Write(multiBuffActionList2.Count);
            List<MultiBuffAction>.Enumerator enumerator2 = multiBuffActionList2.GetEnumerator();
            while (enumerator2.MoveNext()) {
                enumerator2.Current.Write(writer);
            }

            writer.Write(multiBuffPrefabAttachmentDescriptorList.Count);
            List<MultiBuffPrefabAttachmentDescriptor>.Enumerator enumerator3 = multiBuffPrefabAttachmentDescriptorList.GetEnumerator();
            while (enumerator3.MoveNext()) {
                enumerator3.Current.Write(writer);
            }

            writer.Write(buffCounterValues.Count);
            Dictionary<string, float>.Enumerator enumerator4 = buffCounterValues.GetEnumerator();
            while (enumerator4.MoveNext()) {
                writer.Write(enumerator4.Current.Key);
                writer.Write(enumerator4.Current.Value);
            }
        }
    }
}