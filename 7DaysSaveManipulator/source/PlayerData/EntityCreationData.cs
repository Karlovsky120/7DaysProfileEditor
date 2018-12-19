using SevenDaysSaveManipulator.RegionData;
using SevenDaysSaveManipulator.source.PlayerData;
using System;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class EntityCreationData { 

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

        internal EntityCreationData(BinaryReader reader) {
            Read(reader);
        }

        internal void Read(BinaryReader reader) {
            Utils.VerifyVersion(reader.ReadByte(), SaveVersionConstants.ENTITY_CREATION_DATA);

            entityClass = new Value<int>(reader.ReadInt32());

            id = new Value<int>(reader.ReadInt32());
            lifetime = new Value<float>(reader.ReadSingle());

            pos = new Vector3D<float> {
                x = new Value<float>(reader.ReadSingle()),
                y = new Value<float>(reader.ReadSingle()),
                z = new Value<float>(reader.ReadSingle())
            };

            rot = new Vector3D<float> {
                x = new Value<float>(reader.ReadSingle()),
                y = new Value<float>(reader.ReadSingle()),
                z = new Value<float>(reader.ReadSingle())
            };

            onGround = new Value<bool>(reader.ReadBoolean());

            bodyDamage = new BodyDamage(reader);

            if (reader.ReadBoolean()) {
                stats = new EntityStats(reader);
            }

            deathTime = new Value<short>(reader.ReadInt16());

            if (reader.ReadBoolean()) {
                int tileEntityType = reader.ReadInt32();
                lootContainer = TileEntity.Instantiate((TileEntityType)(tileEntityType));
                lootContainer.Read(reader);
            }

            homePosition = new Vector3D<int> {
                x = new Value<int>(reader.ReadInt32()),
                y = new Value<int>(reader.ReadInt32()),
                z = new Value<int>(reader.ReadInt32())
            };
            homeRange = new Value<short>(reader.ReadInt16());

            spawnerSource = (EnumSpawnerSource)reader.ReadByte();

            if (entityClass.Get() == Utils.GetMonoHash("item")) {
                belongsPlayerId = new Value<int>(reader.ReadInt32());
                itemStack.Read(reader);

                reader.ReadSByte();
            }

            else if (entityClass.Get() == Utils.GetMonoHash("fallingBlock")) {
                blockValue = new Value<uint>(reader.ReadUInt32());
                textureFull = new Value<long>(reader.ReadInt64());
            }

            else if (entityClass.Get() == Utils.GetMonoHash("fallingTree")) {
                blockPosition = new Vector3D<int> {
                    x = new Value<int>(reader.ReadInt32()),
                    y = new Value<int>(reader.ReadInt32()),
                    z = new Value<int>(reader.ReadInt32())
                };

                fallTreeDir = new Vector3D<float> {
                    x = new Value<float>(reader.ReadSingle()),
                    y = new Value<float>(reader.ReadSingle()),
                    z = new Value<float>(reader.ReadSingle())
                };
            }

            else if ((entityClass.Get() == Utils.GetMonoHash("playerMale")) || (entityClass.Get() == Utils.GetMonoHash("playerFemale"))) {

                holdingItem = new ItemValue(reader);
                teamNumber = new Value<byte>(reader.ReadByte());
                entityName = new Value<string>(reader.ReadString());
                skinTexture = new Value<string>(reader.ReadString());

                if (reader.ReadBoolean()) {
                    playerProfile = PlayerProfile.Read(reader);
                }
                else {
                    playerProfile = null;
                }
            }

            //num2
            ushort entityDataLength = reader.ReadUInt16();
            if (entityDataLength > 0) {
                byte[] buffer = reader.ReadBytes(entityDataLength);
                entityData = new MemoryStream(buffer);
            }

            if (reader.ReadBoolean() {
                int tileEntityType = reader.ReadInt32();
                traderData = (TileEntityTrader)TileEntity.Instantiate((TileEntityType)(tileEntityType));
                traderData.Read(reader);
            }

        }

        internal void Write(BinaryWriter writer) {
            writer.Write(SaveVersionConstants.ENTITY_CREATION_DATA);
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

            writer.Write(deathTime.Get());

            writer.Write(lootContainer != null);
            if (lootContainer != null) {
                writer.Write((int)lootContainer.GetTileEntityType());
                lootContainer.Write(writer);
            }

            writer.Write(homePosition.x.Get());
            writer.Write(homePosition.y.Get());
            writer.Write(homePosition.z.Get());
            writer.Write(homeRange.Get());
            writer.Write((byte)spawnerSource);

            if (entityClass.Get() == Utils.GetMonoHash("item")) {

                writer.Write(belongsPlayerId.Get());
                itemStack.Write(writer);
                writer.Write((sbyte)0);
            }

            else if (entityClass.Get() == Utils.GetMonoHash("fallingBlock")) {
                writer.Write(blockValue.Get());
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

            writer.Write(traderData != null);
            if (traderData != null) {
                writer.Write((int)traderData.GetTileEntityType());
                traderData.Write(writer);
            }
        }
    }
}