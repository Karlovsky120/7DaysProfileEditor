using System;
using System.IO;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    public class EntityCreationData {
        public enum EnumSpawnerSource {
            Unknown,
            Biome,
            StaticSpawner,
            Dynamic
        }

        //belongsPlayerId
        public Value<int> belongsPlayerId;

        //bodyDamage
        public BodyDamage bodyDamage;

        //blockValue
        public Value<uint> blockValue;

        //blockPos
        public Vector3D<int> blockPosition;

        //deathTime
        public Value<short> deathTime;

        //entityClass
        public Value<int> entityClass;

        //entityData
        public MemoryStream entityData;

        //entityName
        public Value<string> entityName;

        //fallTreeDir
        public Vector3D<float> fallTreeDir;

        //holdingItem
        public ItemValue holdingItem;

        //homePosition
        public Vector3D<int> homePosition;

        //id
        public Value<int> id;

        //traderData
        public TileEntityTrader traderData;

        //itemStack
        public ItemStack itemStack;

        //lifetime
        public Value<float> lifetime;

        //lootContainer
        public TileEntityLootContainer lootContainer;

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

        //spawnerSource
        public EnumSpawnerSource spawnerSource;

        //stats
        public EntityStats stats;

        //teamNumber
        public Value<byte> teamNumber;

        //textureFull
        public Value<long> textureFull;

        //homeRange
        public Value<short> homeRange;

        public EntityCreationData() {}

        internal EntityCreationData(TypedBinaryReader reader, AdditionalFileData xmlData) {
            Read(reader, xmlData);
        }

        internal void Read(TypedBinaryReader reader, AdditionalFileData xmlData) {
            Utils.VerifyVersion(reader.ReadByte(), SaveVersionConstants.ENTITY_CREATION_DATA, "EntityCreationData");

            entityClass = new Value<int>(reader);

            id = new Value<int>(reader);
            lifetime = new Value<float>(reader);

            pos = new Vector3D<float>(reader);
            rot = new Vector3D<float>(reader);

            onGround = new Value<bool>(reader);

            bodyDamage = new BodyDamage(reader);

            if (reader.ReadBoolean()) {
                stats = new EntityStats(reader);
            }

            deathTime = new Value<short>(reader);

            if (reader.ReadBoolean()) {
                lootContainer = (TileEntityLootContainer)TileEntity.Instantiate((TileEntity.TileEntityType)reader.ReadInt32(), reader, xmlData);
            }

            homePosition = new Vector3D<int>(reader);
            homeRange = new Value<short>(reader);

            spawnerSource = (EnumSpawnerSource)reader.ReadByte();

            if (entityClass.Get() == Utils.GetMonoHash("item")) {
                belongsPlayerId = new Value<int>(reader);
                itemStack = new ItemStack(reader, xmlData);

                reader.ReadSByte();
            }

            else if (entityClass.Get() == Utils.GetMonoHash("fallingBlock")) {
                blockValue = new Value<uint>(reader);
                textureFull = new Value<long>(reader);
            }

            else if (entityClass.Get() == Utils.GetMonoHash("fallingTree")) {
                blockPosition = new Vector3D<int>(reader);
                fallTreeDir = new Vector3D<float>(reader);
            }

            else if ((entityClass.Get() == Utils.GetMonoHash("playerMale")) || (entityClass.Get() == Utils.GetMonoHash("playerFemale"))) {
                holdingItem = new ItemValue(reader, xmlData);
                teamNumber = new Value<byte>(reader);
                entityName = new Value<string>(reader);
                skinTexture = new Value<string>(reader);

                if (reader.ReadBoolean()) {
                    playerProfile = new PlayerProfile(reader);
                }
            }

 
            ushort entityDataLength = reader.ReadUInt16();
            if (entityDataLength > 0) {
                entityData = new MemoryStream(reader.ReadBytes(entityDataLength));
            }

            if (reader.ReadBoolean()) {
                traderData = (TileEntityTrader)TileEntity.Instantiate((TileEntity.TileEntityType)reader.ReadInt32(), reader, xmlData);
            }

        }

        internal void Write(TypedBinaryWriter writer) {
            writer.Write(SaveVersionConstants.ENTITY_CREATION_DATA);
            entityClass.Write(writer);
            id.Write(writer);
            lifetime.Write(writer);


            pos.Write(writer);
            rot.Write(writer);
            onGround.Write(writer);

            bodyDamage.Write(writer);

            writer.Write(stats != null);
            if (stats != null) {
                stats.Write(writer);
            }

            deathTime.Write(writer);

            writer.Write(lootContainer != null);
            if (lootContainer != null) {
                writer.Write((int)lootContainer.GetTileEntityType());
                lootContainer.Write(writer);
            }

            homePosition.Write(writer);
            homeRange.Write(writer);
            writer.Write((byte)spawnerSource);

            if (entityClass.Get() == Utils.GetMonoHash("item")) {
                belongsPlayerId.Write(writer);
                itemStack.Write(writer);
                writer.Write((sbyte)0);
            }

            else if (entityClass.Get() == Utils.GetMonoHash("fallingBlock")) {
                blockValue.Write(writer);
                textureFull.Write(writer);
            }

            else if (entityClass.Get() == Utils.GetMonoHash("fallingTree")) {

                blockPosition.Write(writer);
                fallTreeDir.Write(writer);
            }

            else if ((entityClass.Get() == Utils.GetMonoHash("playerMale")) || (entityClass.Get() == Utils.GetMonoHash("playerFemale"))) {

                holdingItem.Write(writer);
                teamNumber.Write(writer);
                entityName.Write(writer);
                skinTexture.Write(writer);
                writer.Write(playerProfile != null);
                if (playerProfile != null) {
                    playerProfile.Write(writer);
                }
            }

            writer.Write((ushort)entityData.Length);
            if (entityData.Length > 0) {
                writer.Write(entityData.ToArray());
            }

            writer.Write(traderData != null);
            if (traderData != null) {
                writer.Write((int)traderData.GetTileEntityType());
                traderData.Write(writer);
            }
        }
    }
}