using SevenDaysSaveManipulator.PlayerData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysSaveManipulator.RegionData {
    class TileEntityTrader : TileEntity {

        Value<int> version;

        TraderData traderData = new TraderData();

        public override void Read(BinaryReader reader) {
            base.Read(reader);

            version = new Value<int>(reader.ReadInt32());

            traderData.Read(reader);
        }

        public override void Write(BinaryWriter writer) {
            base.Write(writer);

            writer.Write(version.Get());

            traderData.Write(writer);
        }
    }
}
