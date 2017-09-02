using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysSaveManipulator.RegionData {
    public class ChunkBlockLayer {

        byte[] oh;
        byte jh;

        byte[] fh;

        public void Read(BinaryReader reader) {
            bool flag = reader.ReadBoolean();

            if (flag) {
                oh = new byte[1024];
                reader.Read(oh, 0, 1024);
            }

            else {
                jh = reader.ReadByte();
            }

            bool flag2 = reader.ReadBoolean();

            if (flag2) {
                fh = new byte[3072];
                reader.Read(fh, 0, 3072);
            }
        }

        public void Write(BinaryWriter writer) {
            writer.Write(oh != null);

            if (oh != null) {
                writer.Write(oh, 0, 1024);
            }

            else {
                writer.Write(jh);
            }

            writer.Write(fh != null);

            if (fh != null) {
                writer.Write(fh, 0, 3072);
            }
        }
    }
}
