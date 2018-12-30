using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SevenDaysSaveManipulator {

    public class TypedBinaryWriter : BinaryWriter {

        private readonly Dictionary<Type, object> valueTypeFunctors;

        public TypedBinaryWriter(Stream input) : this(input, Encoding.UTF8, false) { }

        public TypedBinaryWriter(Stream input, Encoding encoding) : this(input, encoding, false) { }

        public TypedBinaryWriter(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen) {
            valueTypeFunctors = new Dictionary<Type, object>() {
                {typeof(byte), new Action<byte>(Write)},
                {typeof(int), new Action<int>(Write)},
                {typeof(short), new Action<short>(Write)},
                {typeof(long), new Action<long>(Write)},
                {typeof(sbyte), new Action<sbyte>(Write)},
                {typeof(uint), new Action<uint>(Write)},
                {typeof(ushort), new Action<ushort>(Write)},
                {typeof(ulong), new Action<ulong>(Write)},
                {typeof(bool), new Action<bool>(Write)},
                {typeof(float), new Action<float>(Write)},
                {typeof(string), new Action<string>(Write)}
            };
        }

        /// <summary>
        /// Writes a basic type to the stream
        /// </summary>
        /// <typeparam name="T">Type of basic item to write. Type must be one of the following:
        /// byte, int, short, long, sbyte, uint, ushort, ulong, bool, float, double, decimal, string</typeparam>
        /// <param name="data">Item to write</param>
        public void WriteBasicType<T>(T data) {
            if (valueTypeFunctors.ContainsKey(typeof(T))) {
                ((Action<T>)valueTypeFunctors[typeof(T)])(data);
            } else {
                throw new ArgumentException(string.Format("{0} is not a supported value type.", typeof(T)));
            }
        }

        /// <summary>
        /// Writes a complex type to the stream
        /// </summary>
        /// <typeparam name="T">Type of complex item to write</typeparam>
        /// <param name="data">Item to write</param>
        /*public void WriteComplexType<T>(T data) where T : ISaveSegment {
            ((ISaveSegment)data).Write(this);
        }*/
    }
}
