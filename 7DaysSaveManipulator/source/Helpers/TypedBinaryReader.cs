using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SevenDaysSaveManipulator {

    public class TypedBinaryReader : BinaryReader {

        private readonly Dictionary<Type, object> valueTypeFunctors;

        public TypedBinaryReader(Stream input) : this(input, Encoding.UTF8, false) { }

        public TypedBinaryReader(Stream input, Encoding encoding) : this(input, encoding, false) { }

        public TypedBinaryReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen) {
            valueTypeFunctors = new Dictionary<Type, object>() {
                {typeof(byte), new Func<byte>(ReadByte)},
                {typeof(int), new Func<int>(ReadInt32)},
                {typeof(short), new Func<short>(ReadInt16)},
                {typeof(long), new Func<long>(ReadInt64)},
                {typeof(sbyte), new Func<sbyte>(ReadSByte)},
                {typeof(uint), new Func<uint>(ReadUInt32)},
                {typeof(ushort), new Func<ushort>(ReadUInt16)},
                {typeof(ulong), new Func<ulong>(ReadUInt64)},
                {typeof(bool), new Func<bool>(ReadBoolean)},
                {typeof(float), new Func<float>(ReadSingle)},
                {typeof(double), new Func<double>(ReadDouble)},
                {typeof(decimal), new Func<decimal>(ReadDecimal)},
                {typeof(string), new Func<string>(ReadString)}
            };
        }

        /// <summary>
        /// Reads a basic <typeparamref name="T"/> type from the stream
        /// </summary>
        /// <typeparam name="T">Type of basic item to read. Type must be one of the following:
        /// byte, int, short, long, sbyte, uint, ushort, ulong, bool, float, double, decimal, string</typeparam>
        /// <returns>Read item</returns>
        public T ReadBasicType<T>() {
            if (valueTypeFunctors.ContainsKey(typeof(T))) {
                return ((Func<T>)valueTypeFunctors[typeof(T)])();
            }
            throw new ArgumentException(string.Format("{0} is not a supported value type.", typeof(T)));
        }

        /// <summary>
        /// Read a complex T type from the stream
        /// </summary>
        /// <typeparam name="T">Type of complex item to read</typeparam>
        /// <returns>Read item</returns>
        /*public T ReadComplexType<T>() where T : ISaveSegment, new() {
            T item = new T();
            item.Read(this);
            return item;
        }*/
    }
}
