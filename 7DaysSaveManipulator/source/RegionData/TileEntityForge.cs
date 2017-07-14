using SevenDaysSaveManipulator.PlayerData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysSaveManipulator.RegionData {
    class TileEntityForge : TileEntity {

        Value<ulong> ib;

        ItemStack[] gb;
        ItemStack[] kb;

        ItemStack fd = new ItemStack();
        ItemStack mb = new ItemStack();

        Value<int> hb;
        Value<int> qd;
        Value<int> yd;
        ItemValue tb = new ItemValue();

        public override void Read(BinaryReader reader) {
            base.Read(reader);

            ib = new Value<ulong>(reader.ReadUInt64());

            int itemStackLengthA = (int)reader.ReadByte();
            gb = new ItemStack[itemStackLengthA];
            for (int i = 0; i < itemStackLengthA; ++i) {
                gb[i].Read(reader);
            }

            int itemStackLengthB = (int)reader.ReadByte();
            kb = new ItemStack[itemStackLengthB];
            for (int j = 0; j < itemStackLengthB; ++j) {
                kb[j].Read(reader);
            }

            fd.Read(reader);
            mb.Read(reader);

            hb = new Value<int>(reader.ReadInt32());
            qd = new Value<int>((int)reader.ReadInt16());
            yd = new Value<int>((int)reader.ReadInt16());
            tb.Read(reader);   
        }


        public override void Write(BinaryWriter writer) {
            base.Write(writer);

            writer.Write(ib.Get());

            writer.Write((byte)gb.Length);
            for (int i = 0; i < gb.Length; ++i) {
                gb[i].Write(writer);
            }

            writer.Write((byte)kb.Length);
            for (int j = 0; j < kb.Length; ++j) {
                kb[j].Write(writer);
            }

            fd.Write(writer);
            mb.Write(writer);

            writer.Write(hb.Get());
            writer.Write(qd.Get());
            writer.Write(yd.Get());
            tb.Write(writer);
        }
    }
}
