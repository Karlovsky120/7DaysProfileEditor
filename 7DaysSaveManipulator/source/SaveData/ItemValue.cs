using System;
using System.Collections.Generic;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    public class ItemValue {

        //Activated
        public Value<byte> activated;

        //CosmeticMods
        public List<ItemValue> cosmeticMods;

        //Meta
        public Value<ushort> meta;

        //Modifications
        public List<ItemValue> modifications;

        //Quality
        public Value<ushort> quality;

        //SelectedAmmoTypeIndex
        public Value<byte> selectedAmmoTypeIndex;

        //Seed
        public Value<ushort> seed;

        //type
        public Value<ushort> type;

        //UseTimes
        public Value<ushort> useTimes;

        private XmlData xmlData;

        public ItemValue() {}

        internal ItemValue(TypedBinaryReader reader, XmlData xmlData) {
            this.xmlData = xmlData;
            Read(reader);
        }

        internal void Read(TypedBinaryReader reader) {
            Utils.VerifyVersion(reader.ReadByte(), SaveVersionConstants.ITEM_VALUE);

            type = new Value<ushort>(reader);
            useTimes = new Value<ushort>(reader);
            quality = new Value<ushort>(reader);
            meta = new Value<ushort>(reader);

            if (!xmlData.IsItemModifier(type.Get())) {
                byte modificationCount = reader.ReadByte();
                modifications = new List<ItemValue>();
                for (byte i = 0; i < modificationCount; ++i) {
                    if (reader.ReadBoolean()) {
                        modifications.Add(new ItemValue(reader, xmlData));
                    } else {
                        modifications.Add(null);
                    }
                }

                byte cosmeticModCount = reader.ReadByte();
                cosmeticMods = new List<ItemValue>();
                for (byte i = 0; i < cosmeticModCount; ++i) {
                    if (reader.ReadBoolean()) {
                        cosmeticMods.Add(new ItemValue(reader, xmlData));
                    } else {
                        cosmeticMods.Add(null);
                    }
                }
            }

            activated = new Value<byte>(reader);
            selectedAmmoTypeIndex = new Value<byte>(reader);
            seed = new Value<ushort>(reader);
        }

        internal void Write(TypedBinaryWriter writer) {
            writer.Write(SaveVersionConstants.ITEM_VALUE);

            type.Write(writer);
            useTimes.Write(writer);
            quality.Write(writer);
            meta.Write(writer);

            if (!xmlData.IsItemModifier(type.Get())) {
                writer.Write((byte)modifications.Count);
                foreach (ItemValue modification in modifications) {
                    writer.Write(modification != null);
                    if (modification != null) {
                        modification.Write(writer);
                    }
                }

                writer.Write((byte)cosmeticMods.Count);
                foreach (ItemValue cosmeticMod in cosmeticMods) {
                    writer.Write(cosmeticMod != null);
                    if (cosmeticMod != null) {
                        cosmeticMod.Write(writer);
                    }
                }
            }

            activated.Write(writer);
            selectedAmmoTypeIndex.Write(writer);
            seed.Write(writer);
        }
    }
}