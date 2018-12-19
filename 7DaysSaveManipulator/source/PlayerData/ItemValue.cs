using SevenDaysSaveManipulator.source.PlayerData;
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

        //Meta
        public Value<ushort> meta;

        //Parts
        public List<ItemValue> parts = new List<ItemValue>();

        //Quality
        public Value<ushort> quality;

        //SelectedAmmoTypeIndex
        public Value<byte> selectedAmmoTypeIndex;

        //type
        public Value<ushort> type;

        //UseTimes
        public Value<ushort> useTimes;

        public ItemValue() {}

        internal ItemValue(BinaryReader reader) {
            Read(reader);
        }

        internal void Read(BinaryReader reader) {
            Utils.VerifyVersion<byte>(reader.ReadByte(), SaveVersionConstants.ITEM_VALUE);

            type = new Value<ushort>(reader.ReadUInt16());
            useTimes = new Value<ushort>(reader.ReadUInt16());
            quality = new Value<ushort>(reader.ReadUInt16());
            meta = new Value<ushort>(reader.ReadUInt16());

            //b2
            byte partCount = reader.ReadByte();

            for (byte i = 0; i < partCount; ++i) {
                if (reader.ReadBoolean()) {
                    parts.Add(new ItemValue(reader));
                }
                else {
                    parts.Add(null);
                }
            }

            if (reader.ReadBoolean()) {
                byte attachmentsLength = reader.ReadByte();
                for (int j = 0; j < attachmentsLength; j++) {
                    attachments.Add(new ItemValue(reader));
                }
            }

            activated = new Value<bool>(reader.ReadBoolean());
            selectedAmmoTypeIndex = new Value<byte>(reader.ReadByte());
        }

        internal void Write(BinaryWriter writer) {
            writer.Write(SaveVersionConstants.ITEM_VALUE);
            writer.Write(type.Get());
            writer.Write(useTimes.Get());
            writer.Write(quality.Get());
            writer.Write(meta.Get());

            int partCount = 0;
            foreach(ItemValue part in parts) {
                if (part != null) {
                    ++partCount;
                }
            }

            writer.Write((byte)partCount);

            foreach (ItemValue part in parts) {
                writer.Write(part != null);
                if (part != null) {
                    part.Write(writer);
                }
            }

            writer.Write(attachments.Count > 0);
            if (attachments.Count > 0) {
                writer.Write((byte)attachments.Count);

                foreach(ItemValue attachment in attachments) {
                    attachment.Write(writer);
                }
            }

            writer.Write(activated.Get());
            writer.Write(selectedAmmoTypeIndex.Get());
        }
    }
}