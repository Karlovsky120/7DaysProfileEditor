using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysSaveManipulator.RegionData {
    public class ChunkCustomData {

        public string key;
        ulong expiresInWorldTime;
        bool isSavedToNetwork;

        byte[] data;

        public void Read(BinaryReader reader) {
            key = reader.ReadString();
            expiresInWorldTime = reader.ReadUInt64();
            isSavedToNetwork = reader.ReadBoolean();

            int dataLength = (int)reader.ReadUInt16();

            if (dataLength > 0) {
                data = reader.ReadBytes(dataLength);
            }
        }

        public void Write(BinaryWriter writer) {
            writer.Write(key);

            writer.Write(expiresInWorldTime);
            writer.Write(isSavedToNetwork);

            if (data != null) {
                writer.Write((ushort)data.Length);

                if (data.Length > 0) {
                    writer.Write(data);
                }
            }
        }
    }
}
