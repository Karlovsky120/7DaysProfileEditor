using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysSaveManipulator.RegionData {
    public class ChunkBlockChannel {

        CBCLayer[] cbcLayers = new CBCLayer[64];
        byte[] jj = new byte[64];

        public void Read(BinaryReader reader) {
            for (int i = 0; i < 64; ++i) {
                bool flag = reader.ReadBoolean();

                if (!flag) {

                    cbcLayers[i] = new CBCLayer();
                    reader.Read(cbcLayers[i].data, 0, 1024);
                }

                else {
                    jj[i] = reader.ReadByte();
                }
            }
        }

        public void Write(BinaryWriter writer) {
            for (int i = 0; i < 64; ++i) {

                writer.Write(cbcLayers[i] == null);

                if (cbcLayers[i] != null) {
                    writer.Write(cbcLayers[i].data, 0, 1024);
                }

                else {
                    writer.Write(jj[i]);
                }
            }
        }
    }
}
