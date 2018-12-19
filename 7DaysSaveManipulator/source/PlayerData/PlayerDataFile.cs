using SevenDaysSaveManipulator.source.PlayerData;
using SevenDaysXMLParser.Quests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class PlayerDataFile {

        //alreadyCraftedList
        public HashSet<string> alreadyCraftedList;

        //bag
        public ItemStack[] bag;

        //bCrouchedLocked
        public Value<bool> bCrouchedLocked;

        //bDead
        public Value<bool> bDead;

        //bLoaded
        public Value<bool> bLoaded;

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
        public List<string> favoriteRecipeList = new List<string>();

        //id
        public Value<int> id;

        //inventory
        public ItemStack[] inventory;

        //lastSpawnPosition
        public Vector3D<int> lastSpawnPosition;

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
        public List<int> trackedFriendEntityIds = new List<int>();

        //unlockedRecipeList
        public List<string> unlockedRecipeList = new List<string>();

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
        public List<ushort> favoriteCreativeStacks = new List<ushort>();

        //gameStageBornAtWorldTime
        public Value<ulong> gameStageBornAtWorldTime;

        public PlayerDataFile(string path) {
            Load(path);
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

        public void Load(string path, QuestsXml questsXml) {
            using (BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open))) {
                Read(reader, questsXml);
            }
        }

        public void Save(string path) {
            using (BinaryWriter writer = new BinaryWriter(new FileStream(path, FileMode.Create))) {
                Write(writer);
            }
        }

        private void Read(BinaryReader reader, QuestsXml questsXml) {

            if (reader.ReadChar() == 't' && reader.ReadChar() == 't' && reader.ReadChar() == 'p' &&
                reader.ReadChar() == '\0') {
                Utils.VerifyVersion(reader.ReadByte(), SaveVersionConstants.PLAYER_DATA_FILE);

                ecd.Read(reader);
                inventory = ItemStack.ReadItemStack(reader);
                selectedInventorySlot = new Value<byte>(reader.ReadByte());
                bag = ItemStack.ReadItemStack(reader);

                //num
                ushort alreadyCraftedListLength = reader.ReadUInt16();
                for (ushort i = 0; i < alreadyCraftedListLength; ++i) {
                    alreadyCraftedList.Add(reader.ReadString());
                }

                //b
                byte spawnPointCount = reader.ReadByte();
                for (byte i = 0; i < spawnPointCount; ++i) {
                    Vector3D<int> spawnPoint = new Vector3D<int> {
                        x = new Value<int>(reader.ReadInt32()),
                        y = new Value<int>(reader.ReadInt32()),
                        z = new Value<int>(reader.ReadInt32())
                    };

                    spawnPoints.Add(spawnPoint);
                }

                selectedSpawnPointKey = new Value<long>(reader.ReadInt64());

                reader.ReadBoolean();
                reader.ReadInt16();

                bLoaded = new Value<bool>(reader.ReadBoolean());

                lastSpawnPosition = new Vector3D<int> {
                    x = new Value<int>(reader.ReadInt32()),
                    y = new Value<int>(reader.ReadInt32()),
                    z = new Value<int>(reader.ReadInt32()),
                    heading = new Value<float>(reader.ReadSingle())
                };

                id = new Value<int>(reader.ReadInt32());

                droppedBackpackPosition = new Vector3D<int> {
                    x = new Value<int>(reader.ReadInt32()),
                    y = new Value<int>(reader.ReadInt32()),
                    z = new Value<int>(reader.ReadInt32())
                };

                playerKills = new Value<int>(reader.ReadInt32());
                zombieKills = new Value<int>(reader.ReadInt32());
                deaths = new Value<int>(reader.ReadInt32());
                score = new Value<int>(reader.ReadInt32());
                equipment = new Equipment(reader);

                //num
                ushort unlockedRecipeListLength = reader.ReadUInt16();
                for (ushort i = 0; i < unlockedRecipeListLength; ++i) {
                    unlockedRecipeList.Add(reader.ReadString());
                }

                reader.ReadUInt16();

                markerPosition = new Vector3D<int> {
                    x = new Value<int>(reader.ReadInt32()),
                    y = new Value<int>(reader.ReadInt32()),
                    z = new Value<int>(reader.ReadInt32())
                };

                favoriteEquipment = new Equipment(reader);
                bCrouchedLocked = new Value<bool>(reader.ReadBoolean());
                craftingData = new CraftingData(reader);

                //num
                ushort favoriteRecipeListLength = reader.ReadUInt16();
                for (ushort i = 0; i < favoriteRecipeListLength; ++i) {
                    favoriteRecipeList.Add(reader.ReadString());
                }

                totalItemsCrafted = new Value<uint>(reader.ReadUInt32());
                distanceWalked = new Value<float>(reader.ReadSingle());
                longestLife = new Value<float>(reader.ReadSingle());

                waypoints = new WaypointCollection(reader);
                questJournal = new QuestJournal(reader, questXml);

                deathUpdateTime = new Value<int>(reader.ReadInt32());
                currentLife = new Value<float>(reader.ReadSingle());
                bDead = new Value<bool>(reader.ReadBoolean());

                reader.ReadByte();

                //bModdedSaveGame; I was asked to always save this variable as true
                reader.ReadBoolean();

                playerJournal = new PlayerJournal(reader);

                rentedVMPosition = new Vector3D<int> {
                    x = new Value<int>(reader.ReadInt32()),
                    y = new Value<int>(reader.ReadInt32()),
                    z = new Value<int>(reader.ReadInt32())
                };

                rentalEndTime = new Value<int>(reader.ReadInt32());

                ushort trackedFriendEntityIdsCount = reader.ReadUInt16();
                for (ushort i = 0; i < trackedFriendEntityIdsCount; ++i) {
                    trackedFriendEntityIds.Add(reader.ReadInt32());
                }

                //num2
                int progressionDataLength = reader.ReadInt32();
                progressionData = ((progressionDataLength > 0) ? new MemoryStream(reader.ReadBytes(progressionDataLength)) : new MemoryStream());

                //num2
                int buffDataLength = reader.ReadInt32();
                buffData = ((buffDataLength > 0) ? new MemoryStream(reader.ReadBytes(buffDataLength)) : new MemoryStream());

                //num2
                int stealthDataLength = reader.ReadInt32();
                stealthData = ((stealthDataLength > 0) ? new MemoryStream(reader.ReadBytes(stealthDataLength)) : new MemoryStream());

                ushort favoriteCreativeStacksCount = reader.ReadUInt16();
                for (ushort i = 0; i < favoriteCreativeStacksCount; ++i) {
                    favoriteCreativeStacks.Add(reader.ReadUInt16());
                }
            }
            else {
                throw new IOException("Save file corrupted!");
            }
        }

        private void Write(BinaryWriter writer) {
            writer.Write('t');
            writer.Write('t');
            writer.Write('p');
            writer.Write((byte)0);
            writer.Write(SaveVersionConstants.PLAYER_DATA_FILE);

            ecd.Write(writer);

            ItemStack.WriteItemStack(writer, inventory);
            writer.Write(selectedInventorySlot.Get());
            ItemStack.WriteItemStack(writer, bag);
            writer.Write((ushort)alreadyCraftedList.Count);

            foreach(string current in alreadyCraftedList) {
                writer.Write(current);
            }

            writer.Write((byte)spawnPoints.Count);
            for (int i = 0; i < spawnPoints.Count; ++i) {
                writer.Write(spawnPoints[i].x.Get());
                writer.Write(spawnPoints[i].y.Get());
                writer.Write(spawnPoints[i].z.Get());
            }

            writer.Write(selectedSpawnPointKey.Get());
            writer.Write(true);
            writer.Write((short)0);

            writer.Write(bLoaded.Get());

            writer.Write(lastSpawnPosition.x.Get());
            writer.Write(lastSpawnPosition.y.Get());
            writer.Write(lastSpawnPosition.z.Get());

            writer.Write(lastSpawnPosition.heading.Get());
            writer.Write(id.Get());

            writer.Write(droppedBackpackPosition.x.Get());
            writer.Write(droppedBackpackPosition.y.Get());
            writer.Write(droppedBackpackPosition.z.Get());

            writer.Write(playerKills.Get());
            writer.Write(zombieKills.Get());
            writer.Write(deaths.Get());
            writer.Write(score.Get());

            equipment.Write(writer);

            writer.Write((ushort)unlockedRecipeList.Count);
            foreach(string current in unlockedRecipeList) {
                writer.Write(current);
            }

            writer.Write((ushort)1);

            writer.Write(markerPosition.x.Get());
            writer.Write(markerPosition.y.Get());
            writer.Write(markerPosition.z.Get());

            favoriteEquipment.Write(writer);

            writer.Write(bCrouchedLocked.Get());
            craftingData.Write(writer);

            writer.Write((ushort)favoriteRecipeList.Count);
            foreach (string current in favoriteRecipeList) {
                writer.Write(current);
            }

            writer.Write(totalItemsCrafted.Get());
            writer.Write(distanceWalked.Get());
            writer.Write(longestLife.Get());
            writer.Write(gameStageBornAtWorldTime.Get());

            waypoints.Write(writer);
            questJournal.Write(writer);

            writer.Write(deathUpdateTime.Get());
            writer.Write(currentLife.Get());
            writer.Write(bDead.Get());

            writer.Write((byte)88);
            writer.Write(true);

            playerJournal.Write(writer);

            writer.Write(rentedVMPosition.x.Get());
            writer.Write(rentedVMPosition.y.Get());
            writer.Write(rentedVMPosition.z.Get());

            writer.Write(rentalEndTime.Get());

            writer.Write((ushort)trackedFriendEntityIds.Count);
            for (int i = 0; i < this.trackedFriendEntityIds.Count; ++i) {
                writer.Write(trackedFriendEntityIds[i]);
            }

            writer.Write((int)progressionData.Length);
            writer.Write(progressionData.ToArray());

            writer.Write((int)buffData.Length);
            writer.Write(buffData.ToArray());

            writer.Write((int)stealthData.Length);
            writer.Write(stealthData.ToArray());

            writer.Write((ushort)favoriteCreativeStacks.Count);
            for (int i = 0; i < favoriteCreativeStacks.Count; ++i) {
                writer.Write(favoriteCreativeStacks[i]);
            }
        }
    }
}