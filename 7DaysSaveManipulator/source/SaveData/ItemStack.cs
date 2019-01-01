using System;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    public class ItemStack {

        //count
        public Value<short> count;

        //itemValue
        public ItemValue itemValue;

        public ItemStack() {}

        internal ItemStack(TypedBinaryReader reader, AdditionalFileData xmlData) {
            Read(reader, xmlData);
        }

        internal void Read(TypedBinaryReader reader, AdditionalFileData xmlData) {
            count = new Value<short>(reader);
            if (count.Get() > 0) {
                itemValue = new ItemValue(reader, xmlData);
            }
        }

        internal void Write(TypedBinaryWriter writer) {
            count.Write(writer);
            if (count.Get() > 0) {
                itemValue.Write(writer);
            }
        }
    }
}