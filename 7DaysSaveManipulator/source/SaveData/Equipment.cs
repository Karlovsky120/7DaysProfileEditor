using System;
using System.Collections.Generic;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    public class Equipment {

        //m_slots
        public List<ItemValue> slots;

        //Hardcoded in the game
        private readonly int slotsLength = 32;

        public Equipment() {}

        internal Equipment(TypedBinaryReader reader, XmlData xmlData) {
            Read(reader, xmlData);
        }

        internal void Read(TypedBinaryReader reader, XmlData xmlData) {
           Utils.VerifyVersion(reader.ReadByte(), SaveVersionConstants.EQUIPMENT);
   
            slots = new List<ItemValue>(slotsLength);
            for (int i = 0; i < slotsLength; ++i) {
                slots.Add(new ItemValue(reader, xmlData));
            }
        }

        internal void Write(TypedBinaryWriter writer) {
            writer.Write(SaveVersionConstants.EQUIPMENT);
            foreach(ItemValue slot in slots) {
                slot.Write(writer);
            }
        }
    }
}