using System;
using System.Collections.Generic;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    class TileEntitySign : TileEntity {

        //isLocked
        Value<bool> isLocked;

        //ownerID
        Value<string> ownerID;

        //password
        Value<string> password;

        //allowedUserIds
        List<string> allowedUserIds;

        //signText
        Value<string> signText;

        public TileEntitySign() { }

        internal TileEntitySign(TypedBinaryReader reader) {
            Read(reader);
        }

        internal override void Read(TypedBinaryReader reader, AdditionalFileData xmlData = null) {
            base.Read(reader);

            Utils.VerifyVersion(reader.ReadInt32(), SaveVersionConstants.TILE_ENTITY_SIGN, "TileEntitySign");

            isLocked = new Value<bool>(reader);
            ownerID = new Value<string>(reader);
            password = new Value<string>(reader);

            int allowedUserIdsCount = reader.ReadInt32();
            allowedUserIds = new List<string>();
            for (int i = 0; i < allowedUserIdsCount; ++i) {
                allowedUserIds.Add(reader.ReadString());
            }

            signText = new Value<string>(reader);
        }

        internal override void Write(TypedBinaryWriter writer) {
            base.Write(writer);

            writer.Write(SaveVersionConstants.TILE_ENTITY_SIGN);

            isLocked.Write(writer);
            ownerID.Write(writer);
            password.Write(writer);

            writer.Write(allowedUserIds.Count);
            foreach (string allowedUserId in allowedUserIds) {
                writer.Write(allowedUserId);
            }

            signText.Write(writer);
        }
    }
}
