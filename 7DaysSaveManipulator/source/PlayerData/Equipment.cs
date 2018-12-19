using SevenDaysSaveManipulator.source.PlayerData;
using System;
using System.Collections.Generic;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class Equipment {

        //m_slots
        public List<ItemValue> slots = new List<ItemValue>(32);

        public Equipment() {}

        internal Equipment(BinaryReader reader) {
            Read(reader);
        }

        internal void Read(BinaryReader reader) {
           Utils.VerifyVersion(reader.ReadByte(), SaveVersionConstants.EQUIPMENT);

            for (int i = 0; i < 32; ++i) {
                slots[i] = new ItemValue(reader);
            }
        }

        internal void Write(BinaryWriter writer) {
            writer.Write(SaveVersionConstants.EQUIPMENT);
            for (int i = 0; i < 32; ++i) {
                slots[i].Write(writer);
            }
        }
    }
}