using System;
using System.Collections.Generic;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class ItemValue {

        //Activated
        public Value<bool> activated;

        //Attachments
        public List<ItemValue> attachments = new List<ItemValue>();

        //b = 3
        public Value<byte> itemValueVersion;

        //Meta
        public Value<int> meta;

        //Parts
        public ItemValue[] parts = new ItemValue[0];

        //Quality
        public Value<int> quality;

        //SelectedAmmoTypeIndex
        public Value<byte> selectedAmmoTypeIndex;

        //type
        public Value<int> type;

        //UseTimes
        public Value<int> useTimes;

        public void Read(BinaryReader reader) {
            itemValueVersion = new Value<byte>(reader.ReadByte());
            type = new Value<int>((int)reader.ReadUInt16());
            useTimes = new Value<int>((int)reader.ReadUInt16());
            quality = new Value<int>((int)reader.ReadUInt16());
            meta = new Value<int>((int)reader.ReadInt16());

            //b2
            byte partNumber = reader.ReadByte();
            parts = new ItemValue[4];

            if (partNumber != 0) {
                for (int i = 0; i < (int)partNumber; i++) {
                    if (reader.ReadBoolean()) {
                        parts[i] = new ItemValue();
                        parts[i].Read(reader);
                    }
                    else {
                        parts[i] = null;
                    }
                }
            }

            //notSaved
            bool hasAttachments = reader.ReadBoolean();
            if (hasAttachments) {
                //int
                int attachmentsLength = (int)reader.ReadByte();
                for (int j = 0; j < attachmentsLength; j++) {
                    attachments.Add(new ItemValue());
                    attachments[j].Read(reader);
                }
            }

            activated = new Value<bool>(reader.ReadBoolean());
            selectedAmmoTypeIndex = new Value<byte>(reader.ReadByte());
        }

        public void Write(BinaryWriter writer) {
            writer.Write(itemValueVersion.Get());
            writer.Write((ushort)type.Get());
            writer.Write((ushort)useTimes.Get());
            writer.Write((ushort)quality.Get());
            writer.Write((ushort)meta.Get());
            int num = 0;
            for (int i = 0; i < parts.Length; i++) {
                if (parts[i] != null) {
                    num = i + 1;
                }
            }
            writer.Write((byte)num);

            for (int j = 0; j < num; j++) {
                bool hasPart = parts[j] != null;
                writer.Write(hasPart);

                if (hasPart) {
                    parts[j].Write(writer);
                }
            }

            bool hasAttachments = attachments.Count > 0;
            writer.Write(hasAttachments);
            if (hasAttachments) {
                writer.Write((byte)attachments.Count);

                for (int k = 0; k < attachments.Count; k++) {
                    attachments[k].Write(writer);
                }
            }

            writer.Write(activated.Get());
            writer.Write(selectedAmmoTypeIndex.Get());
        }
    }
}