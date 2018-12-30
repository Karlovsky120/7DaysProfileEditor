using System;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    public class Vector3D<T> : Vector2D<T> {
        public Value<T> z;

        public Vector3D() {}

        internal Vector3D(TypedBinaryReader reader) {
            Read(reader);
        }

        /// <summary>
        /// Reads x, y and z variables, in that order
        /// </summary>
        /// <param name="reader"></param>
        internal new void Read(TypedBinaryReader reader) {
            ((Vector2D<T>)this).Read(reader);
            z = new Value<T>(reader.ReadBasicType<T>());
        }

        /// <summary>
        /// Writes x, y and z variables, in that order
        /// </summary>
        /// <param name="writer"></param>
        internal new void Write(TypedBinaryWriter writer) {
            ((Vector2D<T>)this).Write(writer);
            writer.WriteBasicType(z.Get());
        }
    }
}