using System;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    public class TileEntityTrader : TileEntity {

        //TreaderData
        TraderData traderData;

        public TileEntityTrader() {}

        internal TileEntityTrader(TypedBinaryReader reader, XmlData xmlData) {
            Read(reader, xmlData);
        }

        internal override void Read(TypedBinaryReader reader, XmlData xmlData) {
            base.Read(reader);

            Utils.VerifyVersion(reader.ReadInt32(), SaveVersionConstants.TILE_ENTITY_TRADER);

            traderData = new TraderData(reader, xmlData);
        }

        internal override void Write(TypedBinaryWriter writer) {
            base.Write(writer);

            writer.Write(SaveVersionConstants.TILE_ENTITY_TRADER);

            traderData.Write(writer);
        }
    }
}
