using System;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    public class Stat {

        //m_baseMax
        public Value<float> baseMax;

        //m_maxModifier
        public Value<float> maxModifier;

        //m_originalBaseMax
        public Value<float> originalBaseMax;

        //m_originalValue
        public Value<float> originalValue;

        //m_value
        public Value<float> value;

        //m_valueModifier
        public Value<float> valueModifier;

        public Stat() {}

        internal Stat(TypedBinaryReader reader) {
            Read(reader);
        }

        internal void Read(TypedBinaryReader reader) {
            Utils.VerifyVersion(reader.ReadInt32(), SaveVersionConstants.STAT, "Stat");

            value = new Value<float>(reader);
            maxModifier = new Value<float>(reader);
            valueModifier = new Value<float>(reader);
            baseMax = new Value<float>(reader);
            originalBaseMax = new Value<float>(reader);
            originalValue = new Value<float>(reader);
        }

        internal void Write(TypedBinaryWriter writer) {
            writer.Write(SaveVersionConstants.STAT);

            value.Write(writer);
            maxModifier.Write(writer);
            valueModifier.Write(writer);
            baseMax.Write(writer);
            originalBaseMax.Write(writer);
            originalValue.Write(writer);
        }
    }
}