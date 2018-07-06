using SevenDaysSaveManipulator.RegionData;
using System;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class EntityCreationData {

        //belongsPlayerId
        public Value<int> belongsPlayerId;

        //bodyDamage
        public BodyDamage bodyDamage;

        //blockValue.rawData
        public Value<uint> blockValueRawData;

        //blockPos
        public Vector3D<int> blockPosition;

        //deathTime
        public Value<int> deathTime;

        //entityClass
        public Value<int> entityClass;

        //readFileVersion = 22
        public Value<byte> entityCreationDataVersion;

        //entityData
        public MemoryStream entityData;

        //entityName
        public Value<string> entityName;

        //fallTreeDir
        public Vector3D<float> fallTreeDir;

        //holdingItem
        public ItemValue holdingItem;

        //Q
        public Vector3D<int> homePosition;

        //id
        public Value<int> id;

        //isTraderEntity = false for save files
        public Value<bool> isTraderEntity;

        //itemStack
        public ItemStack itemStack;

        //lifetime
        public Value<float> lifetime;

        //lootContainer
        public TileEntity lootContainer;

        //onGround
        public Value<bool> onGround;

        //playerProfile
        public PlayerProfile playerProfile;

        //pos
        public Vector3D<float> pos;

        //rot
        public Vector3D<float> rot;

        //skinTexture
        public Value<string> skinTexture;

        //P
        public EnumSpawnerSource spawnerSource;

        //stats
        public EntityStats stats;

        //teamNumber
        public Value<int> teamNumber;

        //type
        public Value<int> type;

        //D = -1
        public Value<int> unknownD;

        public void Read(BinaryReader reader) {
            entityCreationDataVersion = new Value<byte>(reader.ReadByte());

            //Adding version checks to the segments. This will make the app blowup
            //where an unknown version has been introduced.
            if (entityCreationDataVersion.Get() > 25) //Last known version is 25.
                throw new Exception("Unknown EntityCreationData version! " + entityCreationDataVersion.Get());

            entityClass = new Value<int>(reader.ReadInt32());

            id = new Value<int>(reader.ReadInt32());
            lifetime = new Value<float>(reader.ReadSingle());

            pos = new Vector3D<float>();
            pos.x = new Value<float>(reader.ReadSingle());
            pos.y = new Value<float>(reader.ReadSingle());
            pos.z = new Value<float>(reader.ReadSingle());

            rot = new Vector3D<float>();
            rot.x = new Value<float>(reader.ReadSingle());
            rot.y = new Value<float>(reader.ReadSingle());
            rot.z = new Value<float>(reader.ReadSingle());

            onGround = new Value<bool>(reader.ReadBoolean());

            bodyDamage = new BodyDamage();
            bodyDamage.Read(reader);

            bool isStatsNotNull = reader.ReadBoolean();

            if (isStatsNotNull) {
                stats = new EntityStats();
                stats.Read(reader);
            }

            deathTime = new Value<int>((int)reader.ReadInt16());

            
            bool tileEntityNotNull = reader.ReadBoolean();
            if (tileEntityNotNull) {
                type = new Value<int>(reader.ReadInt32());
                lootContainer = TileEntity.Instantiate((TileEntityType)(type.Get()));
                lootContainer.Read(reader);
            }

            homePosition = new Vector3D<int>();
            homePosition.x = new Value<int>(reader.ReadInt32());
            homePosition.y = new Value<int>(reader.ReadInt32());
            homePosition.z = new Value<int>(reader.ReadInt32());

            unknownD = new Value<int>((int)reader.ReadInt16());
            spawnerSource = (EnumSpawnerSource)reader.ReadByte();

            if (entityClass.Get() == Utils.GetMonoHash("item")) {
                belongsPlayerId = new Value<int>(reader.ReadInt32());
                itemStack.Read(reader);

                reader.ReadSByte();
            }

            else if (entityClass.Get() == Utils.GetMonoHash("fallingBlock")) {
                blockValueRawData = new Value<uint>(reader.ReadUInt32());
            }

            else if (entityClass.Get() == Utils.GetMonoHash("fallingTree")) {
                blockPosition = new Vector3D<int>();
                blockPosition.x = new Value<int>(reader.ReadInt32());
                blockPosition.y = new Value<int>(reader.ReadInt32());
                blockPosition.z = new Value<int>(reader.ReadInt32());

                fallTreeDir = new Vector3D<float>();
                fallTreeDir.x = new Value<float>(reader.ReadSingle());
                fallTreeDir.y = new Value<float>(reader.ReadSingle());
                fallTreeDir.z = new Value<float>(reader.ReadSingle());
            }

            else if ((entityClass.Get() == Utils.GetMonoHash("playerMale")) || (entityClass.Get() == Utils.GetMonoHash("playerFemale"))) {

                holdingItem = new ItemValue();
                holdingItem.Read(reader);
                teamNumber = new Value<int>((int)reader.ReadByte());
                entityName = new Value<string>(reader.ReadString());
                skinTexture = new Value<string>(reader.ReadString());

                bool isPlayerProfileNotNull = reader.ReadBoolean();
                if (isPlayerProfileNotNull) {
                    playerProfile = PlayerProfile.Read(reader);
                }
                else {
                    playerProfile = null;
                }
            }

            //num2
            int entityDataLength = (int)reader.ReadUInt16();
            if (entityDataLength > 0) {
                byte[] buffer = reader.ReadBytes(entityDataLength);
                entityData = new MemoryStream(buffer);
            }

            isTraderEntity = new Value<bool>(reader.ReadBoolean());
        }

        public void Write(BinaryWriter writer) {
            writer.Write(entityCreationDataVersion.Get());
            writer.Write(entityClass.Get());
            writer.Write(id.Get());
            writer.Write(lifetime.Get());
            writer.Write(pos.x.Get());

            writer.Write(pos.y.Get());
            writer.Write(pos.z.Get());
            writer.Write(rot.x.Get());
            writer.Write(rot.y.Get());
            writer.Write(rot.z.Get());
            writer.Write(onGround.Get());

            bodyDamage.Write(writer);
            writer.Write(stats != null);

            if (stats != null) {
                stats.Write(writer);
            }

            writer.Write((short)deathTime.Get());

            writer.Write(lootContainer != null);
            if (lootContainer != null) {
                writer.Write(type.Get());
                lootContainer.Write(writer);
            }

            writer.Write(homePosition.x.Get());
            writer.Write(homePosition.y.Get());
            writer.Write(homePosition.z.Get());
            writer.Write((short)unknownD.Get());
            writer.Write((byte)spawnerSource);

            if (entityClass.Get() == Utils.GetMonoHash("item")) {

                writer.Write(belongsPlayerId.Get());
                itemStack.Write(writer);
                writer.Write((sbyte)0);
            }

            else if (entityClass.Get() == Utils.GetMonoHash("fallingBlock")) {
                writer.Write(blockValueRawData.Get());
            }

            else if (entityClass.Get() == Utils.GetMonoHash("fallingTree")) {

                writer.Write(blockPosition.x.Get());
                writer.Write(blockPosition.y.Get());
                writer.Write(blockPosition.z.Get());

                writer.Write(fallTreeDir.x.Get());
                writer.Write(fallTreeDir.y.Get());
                writer.Write(fallTreeDir.z.Get());
            }

            else if ((entityClass.Get() == Utils.GetMonoHash("playerMale")) || (entityClass.Get() == Utils.GetMonoHash("playerFemale"))) {

                holdingItem.Write(writer);
                writer.Write((byte)teamNumber.Get());
                writer.Write(entityName.Get());
                writer.Write(skinTexture.Get());
                writer.Write(playerProfile != null);
                if (playerProfile != null) {
                    playerProfile.Write(writer);
                }
            }

            int num = (int)entityData.Length;
            writer.Write((ushort)num);
            if (num > 0) {
                writer.Write(entityData.ToArray());
            }

            writer.Write(isTraderEntity.Get());
        }
    }
}