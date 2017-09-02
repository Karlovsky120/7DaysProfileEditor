using SevenDaysSaveManipulator.PlayerData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysSaveManipulator.RegionData {
    class TileEntityVendingMachine : TileEntityTrader {

        Value<int> version;

        Value<bool> vd;
        Value<string> gd;
        Value<string> xd;

        List<string> kd = new List<string>();

        Value<ulong> bd;

        TraderData traderData = new TraderData();

        Value<ulong> dd;

        public override void Read(BinaryReader reader) {
            base.Read(reader);

            version = new Value<int>(reader.ReadInt32());

            vd = new Value<bool>(reader.ReadBoolean());
            gd = new Value<string>(reader.ReadString());
            xd = new Value<string>(reader.ReadString());

            int num = reader.ReadInt32();

            for (int i = 0; i < num; ++i) {
                kd.Add(reader.ReadString());
            }

            bd = new Value<ulong>(reader.ReadUInt64());

            traderData.Read(reader);

            // Is the machine rentable? This will only work on vanilla, custom rentable machines will fail here and won't be able to load the chunk.
            if (traderData.traderID.Get() == 5) {
                dd = new Value<ulong>(reader.ReadUInt64());
            }
        }

        public override void Write(BinaryWriter writer) {
            base.Write(writer);

            writer.Write(version.Get());

            writer.Write(vd.Get());
            writer.Write(gd.Get());
            writer.Write(xd.Get());

            writer.Write(kd.Count);
            for (int i = 0; i < kd.Count; ++i) {
                writer.Write(kd[i]);
            }

            writer.Write(bd.Get());

            traderData.Write(writer);

            // Is the machine rentable? This will only work on vanilla, custom rentable machines will fail here and won't be able to save the chunk.
            if (traderData.traderID.Get() == 5) {
                writer.Write(dd.Get());
            }
        }
    }
}
