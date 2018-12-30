using System.Collections.Generic;

namespace SevenDaysSaveManipulator.SaveData {
    class TileEntityVendingMachine : TileEntityTrader {

        //isLocked
        Value<bool> isLocked;

        //ownerID
        Value<string> ownerID;

        //allowedUserIds
        List<string> allowedUserIds;

        //password
        Value<string> password;

        //rentalEndDay
        Value<int> rentalEndDay;

        //nextAutoBuy
        Value<ulong> nextAutoBuy;

        //TraderData
        TraderData traderData;

        private XmlData xmlData;

        public TileEntityVendingMachine() {}

        internal TileEntityVendingMachine(TypedBinaryReader reader, XmlData xmlData) {
            this.xmlData = xmlData;
            Read(reader);
        }

        internal override void Read(TypedBinaryReader reader, XmlData xmlData = null) {
            base.Read(reader, xmlData);

            Utils.VerifyVersion(reader.ReadInt32(), SaveVersionConstants.TILE_ENTITY_VENDING_MACHINE);

            isLocked = new Value<bool>(reader);
            ownerID = new Value<string>(reader);
            password = new Value<string>(reader);

            int allowedUserIdsCount = reader.ReadInt32();
            allowedUserIds = new List<string>(allowedUserIdsCount);
            for (int i = 0; i < allowedUserIdsCount; ++i) {
                allowedUserIds.Add(reader.ReadString());
            }

            rentalEndDay = new Value<int>(reader);

            traderData = new TraderData(reader, xmlData);

            if (xmlData.IsRentable(traderData.traderID.Get())) {
                nextAutoBuy = new Value<ulong>(reader);
            }
        }

        internal override void Write(TypedBinaryWriter writer) {
            base.Write(writer);

            writer.Write(SaveVersionConstants.TILE_ENTITY_VENDING_MACHINE);

            isLocked.Write(writer);
            ownerID.Write(writer);
            password.Write(writer);

            writer.Write(allowedUserIds.Count);
            foreach (string allowedUserId in allowedUserIds) {
                writer.Write(allowedUserId);
            }

            rentalEndDay.Write(writer);
            traderData.Write(writer);

            if (xmlData.IsRentable(traderData.traderID.Get())) {
                nextAutoBuy.Write(writer);
            }
        }
    }
}
