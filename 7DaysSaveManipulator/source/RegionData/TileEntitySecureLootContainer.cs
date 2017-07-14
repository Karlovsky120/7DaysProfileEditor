using SevenDaysSaveManipulator.PlayerData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysSaveManipulator.RegionData {
    class TileEntitySecureLootContainer : TileEntityLootContainer {

        // 1
        Value<int> version;

        Value<bool> md;
        Value<bool> vd;
        Value<string> gd;
        Value<string> xd;

        List<string> kd = new List<string>();

        public override void Read(BinaryReader reader) {
            base.Read(reader);

            version = new Value<int>(reader.ReadInt32());

            md = new Value<bool>(reader.ReadBoolean());
            vd = new Value<bool>(reader.ReadBoolean());
            gd = new Value<string>(reader.ReadString());
            xd = new Value<string>(reader.ReadString());

            int kdCount = reader.ReadInt32();

            for (int i = 0; i < kdCount; ++i) {
                kd.Add(reader.ReadString());
            }
        }

        public override void Write(BinaryWriter writer) {
            base.Write(writer);

            writer.Write(version.Get());

            writer.Write(md.Get());
            writer.Write(vd.Get());
            writer.Write(gd.Get());
            writer.Write(xd.Get());

            writer.Write(kd.Count);

            for (int i = 0; i < kd.Count; ++i) {
                writer.Write(kd[i]);
            }
        }
    }
}
