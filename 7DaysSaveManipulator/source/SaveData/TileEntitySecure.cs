using System;
using System.Collections.Generic;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    class TileEntitySecure : TileEntityLootContainer {

        //bPlayerPlaced
        Value<bool> bPlayerPlaced;

        //isLocked
        Value<bool> isLocked;

        //ownerID
        Value<string> ownerID;

        //allowedUserIds
        List<string> allowedUserIds;

        //password
        Value<string> password;

        public TileEntitySecure() {}

        internal TileEntitySecure(TypedBinaryReader reader, AdditionalFileData xmlData) {
            Read(reader, xmlData);
        }

        internal override void Read(TypedBinaryReader reader, AdditionalFileData xmlData) {
            base.Read(reader, xmlData);

            Utils.VerifyVersion(reader.ReadInt32(), SaveVersionConstants.TILE_ENTITY_SECURE, "TileEntitySecure");

            bPlayerPlaced = new Value<bool>(reader);
            isLocked = new Value<bool>(reader);
            ownerID = new Value<string>(reader);

            int allowedUserIdsCount = reader.ReadInt32();
            allowedUserIds = new List<string>();
            for (int i = 0; i < allowedUserIdsCount; ++i) {
                allowedUserIds.Add(reader.ReadString());
            }

            password = new Value<string>(reader);
        }

        internal override void Write(TypedBinaryWriter writer) {
            base.Write(writer);

            writer.Write(SaveVersionConstants.TILE_ENTITY_SECURE);

            bPlayerPlaced.Write(writer);
            isLocked.Write(writer);
            ownerID.Write(writer);

            writer.Write(allowedUserIds.Count);
            foreach (string allowedUserId in allowedUserIds) {
                writer.Write(allowedUserId);
            }

            password.Write(writer);
        }
    }
}
