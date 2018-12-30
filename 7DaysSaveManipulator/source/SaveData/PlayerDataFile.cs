using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    public class PlayerDataFile {

        //alreadyCraftedList
        public HashSet<string> alreadyCraftedList;

        //bag
        public List<ItemStack> bag;

        //bCrouchedLocked
        public Value<bool> bCrouchedLocked;

        //bDead
        public Value<bool> bDead;

        //bLoaded
        public Value<bool> bLoaded;

        //bModdedSaveGame
        public Value<bool> bModdedSaveGame;

        //craftingData
        public CraftingData craftingData;

        //currentLife
        public Value<float> currentLife;

        //deaths
        public Value<int> deaths;

        //deathUpdateTime
        public Value<int> deathUpdateTime;

        //distanceWalked
        public Value<float> distanceWalked;

        //droppedBackpackPosition
        public Vector3D<int> droppedBackpackPosition;

        //ecd
        public EntityCreationData ecd;

        //equipment
        public Equipment equipment;

        //favoriteEquipment
        public Equipment favoriteEquipment;

        //favoriteRecipeList
        public List<string> favoriteRecipeList;

        //id
        public Value<int> id;

        //inventory
        public List<ItemStack> inventory;

        //lastSpawnPosition
        public Vector4D<int, float> lastSpawnPosition;

        //longestLife
        public Value<float> longestLife;

        //markerPosition
        public Vector3D<int> markerPosition;

        //playerJournal
        public PlayerJournal playerJournal;

        //playerKills
        public Value<int> playerKills;

        //questJournal
        public QuestJournal questJournal;

        //rentalEndTime
        public Value<int> rentalEndTime;

        //rentedVMPosition
        public Vector3D<int> rentedVMPosition;

        //score
        public Value<int> score;

        //selectedInventorySlot
        public Value<byte> selectedInventorySlot;

        //selectedSpawnPointKey
        public Value<long> selectedSpawnPointKey;

        //spawnPoints
        public List<Vector3D<int>> spawnPoints;

        //totalItemsCrafted
        public Value<uint> totalItemsCrafted;

        //trackedFriendEntityIds
        public List<int> trackedFriendEntityIds;

        //unlockedRecipeList
        public List<string> unlockedRecipeList;

        //waypoints
        public WaypointCollection waypoints;

        //zombieKills
        public Value<int> zombieKills;

        //progressionData
        public MemoryStream progressionData;

        //buffData
        public MemoryStream buffData;

        //stealthData
        public MemoryStream stealthData;

        //favouriteCreativeStacks
        public List<ushort> favoriteCreativeStacks;

        //gameStageBornAtWorldTime
        public Value<ulong> gameStageBornAtWorldTime;

        internal XmlData xmlData;

        public PlayerDataFile(string playerSavePath, string blockMappingsPath, string itemMappingsPath, string blocksXmlPath, string itemsXmlPath, string itemModifiersXmlPath, string questsXmlPath, string tradersXmlPath) {
            xmlData = new XmlData(blockMappingsPath, itemMappingsPath, blocksXmlPath, itemsXmlPath, itemModifiersXmlPath, questsXmlPath, tradersXmlPath);
            Load(playerSavePath);
        }

        public PlayerDataFile DeepCopy() {
            using (Stream stream = new MemoryStream()) {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);

                stream.Seek(0, SeekOrigin.Begin);
                PlayerDataFile copy = (PlayerDataFile)formatter.Deserialize(stream);
                return copy;
            }
        }

        public void Load(string path) {
            using (TypedBinaryReader reader = new TypedBinaryReader(new FileStream(path, FileMode.Open))) {
                Read(reader);
            }
        }

        public void Save(string path) {
            using (TypedBinaryWriter writer = new TypedBinaryWriter(new FileStream(path, FileMode.Create))) {
                Write(writer);
            }
        }

        // Note for myself - in the original code the flag is false and the streamMode is persistency
        internal void Read(TypedBinaryReader reader) {

            if (reader.ReadChar() == 't' && reader.ReadChar() == 't' && reader.ReadChar() == 'p' &&
                reader.ReadChar() == '\0') {
                Utils.VerifyVersion(reader.ReadByte(), SaveVersionConstants.PLAYER_DATA_FILE);

                ecd = new EntityCreationData(reader, xmlData);

                ushort inventoryStackLength = reader.ReadUInt16();
                inventory = new List<ItemStack>(inventoryStackLength);
                for (ushort i = 0; i < inventoryStackLength; ++i) {
                    inventory.Add(new ItemStack(reader, xmlData));
                }

                selectedInventorySlot = new Value<byte>(reader);

                ushort bagStackLength = reader.ReadUInt16();
                bag = new List<ItemStack>(bagStackLength);
                for (ushort i = 0; i < bagStackLength; ++i) {
                    bag.Add(new ItemStack(reader, xmlData));
                }

                ushort alreadyCraftedListLength = reader.ReadUInt16();
                alreadyCraftedList = new HashSet<string>();
                for (ushort i = 0; i < alreadyCraftedListLength; ++i) {
                    alreadyCraftedList.Add(reader.ReadString());
                }

                byte spawnPointCount = reader.ReadByte();
                spawnPoints = new List<Vector3D<int>>(spawnPointCount);
                for (byte i = 0; i < spawnPointCount; ++i) {
                    spawnPoints.Add(new Vector3D<int>(reader));
                }

                selectedSpawnPointKey = new Value<long>(reader);

                reader.ReadBoolean();
                reader.ReadInt16();

                bLoaded = new Value<bool>(reader);

                lastSpawnPosition = new Vector4D<int, float>(reader);

                id = new Value<int>(reader);

                droppedBackpackPosition = new Vector3D<int>(reader);

                playerKills = new Value<int>(reader);
                zombieKills = new Value<int>(reader);
                deaths = new Value<int>(reader);
                score = new Value<int>(reader);
                equipment = new Equipment(reader, xmlData);

                ushort unlockedRecipeListLength = reader.ReadUInt16();
                unlockedRecipeList = new List<string>(unlockedRecipeListLength);
                for (ushort i = 0; i < unlockedRecipeListLength; ++i) {
                    unlockedRecipeList.Add(reader.ReadString());
                }

                reader.ReadUInt16();

                markerPosition = new Vector3D<int>(reader);

                favoriteEquipment = new Equipment(reader, xmlData);
                bCrouchedLocked = new Value<bool>(reader);
                craftingData = new CraftingData(reader, xmlData);

                ushort favoriteRecipeListLength = reader.ReadUInt16();
                favoriteRecipeList = new List<string>(favoriteRecipeListLength);
                for (ushort i = 0; i < favoriteRecipeListLength; ++i) {
                    favoriteRecipeList.Add(reader.ReadString());
                }

                totalItemsCrafted = new Value<uint>(reader);
                distanceWalked = new Value<float>(reader);
                longestLife = new Value<float>(reader);
                gameStageBornAtWorldTime = new Value<ulong>(reader);

                waypoints = new WaypointCollection(reader);
                questJournal = new QuestJournal(reader, xmlData);

                deathUpdateTime = new Value<int>(reader);
                currentLife = new Value<float>(reader);
                bDead = new Value<bool>(reader);

                reader.ReadByte();

                bModdedSaveGame = new Value<bool>(reader);

                playerJournal = new PlayerJournal(reader);

                rentedVMPosition = new Vector3D<int>(reader);

                rentalEndTime = new Value<int>(reader);

                ushort trackedFriendEntityIdsCount = reader.ReadUInt16();
                trackedFriendEntityIds = new List<int>(trackedFriendEntityIdsCount);
                for (ushort i = 0; i < trackedFriendEntityIdsCount; ++i) {
                    trackedFriendEntityIds.Add(reader.ReadInt32());
                }

                int progressionDataLength = reader.ReadInt32();
                progressionData = ((progressionDataLength > 0) ? new MemoryStream(reader.ReadBytes(progressionDataLength)) : new MemoryStream());

                int buffDataLength = reader.ReadInt32();
                buffData = ((buffDataLength > 0) ? new MemoryStream(reader.ReadBytes(buffDataLength)) : new MemoryStream());

                int stealthDataLength = reader.ReadInt32();
                stealthData = ((stealthDataLength > 0) ? new MemoryStream(reader.ReadBytes(stealthDataLength)) : new MemoryStream());

                ushort favoriteCreativeStacksCount = reader.ReadUInt16();
                favoriteCreativeStacks = new List<ushort>(favoriteCreativeStacksCount);
                for (ushort i = 0; i < favoriteCreativeStacksCount; ++i) {
                    favoriteCreativeStacks.Add(reader.ReadUInt16());
                }
            }
            else {
                throw new IOException("Save file corrupted!");
            }
        }

        internal void Write(TypedBinaryWriter writer) {
            writer.Write('t');
            writer.Write('t');
            writer.Write('p');
            writer.Write((byte)0);
            writer.Write(SaveVersionConstants.PLAYER_DATA_FILE);

            ecd.Write(writer);

            writer.Write((ushort)inventory.Count);
            foreach(ItemStack inventoryStack in inventory) {
                inventoryStack.Write(writer);
            }

            selectedInventorySlot.Write(writer);

            writer.Write((ushort)bag.Count);
            foreach (ItemStack bagStack in bag) {
                bagStack.Write(writer);
            }

            writer.Write((ushort)alreadyCraftedList.Count);
            foreach(string current in alreadyCraftedList) {
                writer.Write(current);
            }

            writer.Write((byte)spawnPoints.Count);
            foreach (Vector3D<int> spawnPoint in spawnPoints) {
                spawnPoint.Write(writer);
            }

            selectedSpawnPointKey.Write(writer);
            writer.Write(true);
            writer.Write((short)0);

            bLoaded.Write(writer);

            lastSpawnPosition.Write(writer);

            id.Write(writer);

            droppedBackpackPosition.Write(writer);

            playerKills.Write(writer);
            zombieKills.Write(writer);
            deaths.Write(writer);
            score.Write(writer);

            equipment.Write(writer);

            writer.Write((ushort)unlockedRecipeList.Count);
            foreach(string current in unlockedRecipeList) {
                writer.Write(current);
            }

            writer.Write((ushort)1);

            markerPosition.Write(writer);

            favoriteEquipment.Write(writer);

            bCrouchedLocked.Write(writer);
            craftingData.Write(writer);

            writer.Write((ushort)favoriteRecipeList.Count);
            foreach (string current in favoriteRecipeList) {
                writer.Write(current);
            }

            totalItemsCrafted.Write(writer);
            distanceWalked.Write(writer);
            longestLife.Write(writer);
            gameStageBornAtWorldTime.Write(writer);

            waypoints.Write(writer);
            questJournal.Write(writer);

            deathUpdateTime.Write(writer);
            currentLife.Write(writer);
            bDead.Write(writer);

            writer.Write((byte)88);

            bModdedSaveGame.Write(writer);

            playerJournal.Write(writer);

            rentedVMPosition.Write(writer);

            rentalEndTime.Write(writer);

            writer.Write((ushort)trackedFriendEntityIds.Count);
            foreach (int trackedFriendlyEntityId in trackedFriendEntityIds) {
                writer.Write(trackedFriendlyEntityId);
            }

            writer.Write((int)progressionData.Length);
            writer.Write(progressionData.ToArray());

            writer.Write((int)buffData.Length);
            writer.Write(buffData.ToArray());

            writer.Write((int)stealthData.Length);
            writer.Write(stealthData.ToArray());

            writer.Write((ushort)favoriteCreativeStacks.Count);
            foreach (ushort favoriteCreativeStack in favoriteCreativeStacks) {
                writer.Write(favoriteCreativeStack);
            }
        }
    }
}