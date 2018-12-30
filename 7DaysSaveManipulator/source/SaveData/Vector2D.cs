using System;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    public class Vector2D<T> {
        public Value<T> x;
        public Value<T> y;

        public Vector2D() {}

        internal Vector2D(TypedBinaryReader reader) {
            Read(reader);
        }

        /// <summary>
        /// Reads x and y variables, in that order
        /// </summary>
        /// <param name="reader"></param>
        internal void Read(TypedBinaryReader reader) {
            x = new Value<T>(reader.ReadBasicType<T>());
            y = new Value<T>(reader.ReadBasicType<T>());
        }

        /// <summary>
        /// Writes x and y variables, in that order
        /// </summary>
        /// <param name="writer"></param>
        internal void Write(TypedBinaryWriter writer) {
            writer.WriteBasicType(x.Get());
            writer.WriteBasicType(y.Get());
        }
    }
}