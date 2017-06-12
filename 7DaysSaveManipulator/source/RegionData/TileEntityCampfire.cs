using SevenDaysSaveManipulator.PlayerData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysSaveManipulator.RegionData {
    class TileEntityCampfire : TileEntity {

        Value<ulong> ib;

        ItemStack[] gb;
        ItemStack[] kb;

        ItemStack xb = new ItemStack();
        ItemStack mb = new ItemStack();

        Value<int> hb;
        ItemValue tb = new ItemValue();

        Value<bool> isCooking;
        Value<float> db;

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

            xb.Read(reader);
            mb.Read(reader);

            hb = new Value<int>(reader.ReadInt32());
            tb.Read(reader);
            isCooking = new Value<bool>(reader.ReadBoolean());
            db = new Value<float>(reader.ReadSingle());
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

            xb.Write(writer);
            mb.Write(writer);

            writer.Write(hb.Get());
            tb.Write(writer);
            writer.Write(isCooking.Get());
            writer.Write(db.Get());
        }
    }
}
