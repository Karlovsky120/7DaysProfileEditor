using System;
using System.IO;

namespace SevenDaysSaveManipulator.GameData {

    [Serializable]
    public class EntityCreationData {

        //belongsPlayerId
        public Value<int> belongsPlayerId;

        //bodyDamage
        public BodyDamage bodyDamage;

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

        //P
        public EnumSpawnerSource spawnerSource;

        //stats
        public EntityStats stats;

        //teamNumber
        public Value<int> teamNumber;

        //type = 5
        public Value<int> type;

        //D = -1
        public Value<int> unknownD;

        public void Read(BinaryReader reader) {
            entityCreationDataVersion = new Value<byte>(reader.ReadByte());
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

            stats = new EntityStats();
            stats.Read(reader);

            deathTime = new Value<int>((int)reader.ReadInt16());

            //MAY BE USELESS
            bool tileEntityLootContainerNotNull = reader.ReadBoolean();
            if (tileEntityLootContainerNotNull) {
                type = new Value<int>(reader.ReadInt32());
                lootContainer = new TileEntityLootContainer();
                lootContainer.Read(reader);
            }
            //END USELESS

            homePosition = new Vector3D<int>();
            homePosition.x = new Value<int>(reader.ReadInt32());
            homePosition.y = new Value<int>(reader.ReadInt32());
            homePosition.z = new Value<int>(reader.ReadInt32());

            unknownD = new Value<int>((int)reader.ReadInt16());
            spawnerSource = (EnumSpawnerSource)reader.ReadByte();

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

            stats.Write(writer);

            writer.Write((short)deathTime.Get());
            writer.Write(lootContainer != null);
            if (lootContainer != null) {
                writer.Write(5);
                lootContainer.Write(writer);
            }

            writer.Write(homePosition.x.Get());
            writer.Write(homePosition.y.Get());
            writer.Write(homePosition.z.Get());
            writer.Write((short)unknownD.Get());
            writer.Write((byte)spawnerSource);

            holdingItem.Write(writer);
            writer.Write((byte)teamNumber.Get());
            writer.Write(entityName.Get());
            writer.Write(skinTexture.Get());
            writer.Write(playerProfile != null);
            if (playerProfile != null) {
                playerProfile.Write(writer);
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