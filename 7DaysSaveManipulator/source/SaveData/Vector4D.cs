using System;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    public class Vector4D<T, R> : Vector3D<T> {
        public Value<R> w;

        public Vector4D() {}

        internal Vector4D(TypedBinaryReader reader) {
            Read(reader);
        }

        /// <summary>
        /// Reads x, y, z and w variables, in that order
        /// </summary>
        /// <param name="reader"></param>
        internal new void Read(TypedBinaryReader reader) {
            ((Vector3D<T>)this).Read(reader);
            w = new Value<R>(reader.ReadBasicType<R>());
        }

        /// <summary>
        /// Writes x, y, z and w variables, in that order
        /// </summary>
        /// <param name="writer"></param>
        internal new void Write(TypedBinaryWriter writer) {
            ((Vector3D<T>)this).Write(writer);
            writer.WriteBasicType(w.Get());
        }
    }
}
