using System;
using System.Collections.Generic;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    class TileEntitySecureLootContainer : TileEntityLootContainer {

        //bPlayerPlaced
        Value<bool> bPlayerPlaced;

        //isLocked
        Value<bool> isLocked;

        //ownerID
        Value<string> ownerID;

        //password
        Value<string> password;

        //allowedUserIds
        List<string> allowedUserIds;

        public TileEntitySecureLootContainer() {}

        internal TileEntitySecureLootContainer(TypedBinaryReader reader, AdditionalFileData xmlData) {
            Read(reader, xmlData);
        }

        internal override void Read(TypedBinaryReader reader, AdditionalFileData xmlData) {
            base.Read(reader, xmlData);

            Utils.VerifyVersion(reader.ReadInt32(), SaveVersionConstants.TILE_ENTITY_SECURE_LOOT_CONTAINER, "TileEntitySecureLootContainer");

            bPlayerPlaced = new Value<bool>(reader);
            isLocked = new Value<bool>(reader);
            ownerID = new Value<string>(reader);
            password = new Value<string>(reader);

            int allowedUserIdsCount = reader.ReadInt32();
            allowedUserIds = new List<string>();
            for (int i = 0; i < allowedUserIdsCount; ++i) {
                allowedUserIds.Add(reader.ReadString());
            }
        }

        internal override void Write(TypedBinaryWriter writer) {
            base.Write(writer);

            writer.Write(SaveVersionConstants.TILE_ENTITY_SECURE_LOOT_CONTAINER);

            bPlayerPlaced.Write(writer);
            isLocked.Write(writer);
            ownerID.Write(writer);
            password.Write(writer);

            writer.Write(allowedUserIds.Count);
            foreach (string allowedUserId in allowedUserIds) {
                writer.Write(allowedUserId);
            }
        }
    }
}
