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

        //drink
        public LiveStats drink;

        //droppedBackpackPosition
        public Vector3D<int> droppedBackpackPosition;

        //ecd
        public EntityCreationData ecd;

        //equipment
        public Equipment equipment;

        //experience
        public Value<uint> experience;

        //favoriteEquipment
        public Equipment favoriteEquipment;

        //favoriteRecipeList
        public List<string> favoriteRecipeList;

        //food
        public LiveStats food;

        //id
        public Value<int> id;

        //inventory
        public ItemStack[] inventory;

        //lastSpawnPosition
        public Vector3D<int> lastSpawnPosition;

        //level
        public Value<int> level;

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

        //notSaved = true
        public Value<bool> randomBoolean;

        //notSaved = 0
        public Value<short> randomShort;

        //notSaved = 1
        public Value<ushort> randomUShort;

        //rentalEndTime
        public Value<ulong> rentalEndTime;

        //rentedVMPosition
        public Vector3D<int> rentedVMPosition;

        //version = 24
        public Value<uint> saveFileVersion;

        //score
        public Value<int> score;

        //selectedInventorySlot
        public Value<int> selectedInventorySlot;

        //selectedSpawnPointKey
        public Value<long> selectedSpawnPointKey;

        //skillPoints
        public Value<int> skillPoints;

        //custom, doesn't exist
        public Skills skills;

        //J
        public MemoryStream skillStream;

        //spawnPoints
        public List<Vector3D<int>> spawnPoints;

        //totalItemsCrafted
        public Value<uint> totalItemsCrafted;

        //trackedFriendEntityIds
        public List<int> trackedFriendEntityIds;

        //unlockedRecipeList
        public List<string> unlockedRecipeList;

        //added in Version 36.
        public Value<ulong> gameStageLifetimeTicks;

        //waypoints
        public WaypointCollection waypoints;

        //zombieKills
        public Value<int> zombieKills;

        private PlayerDataFile() { }

        public PlayerDataFile(string path) {
            Read(path);
        }

        public PlayerDataFile Clone() {
            /*Stream stream = new FileStream("PlayerDataFile", FileMode.Create, FileAccess.Write, FileShare.None);

            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            stream.Close();

            stream = new FileStream("PlayerDataFile", FileMode.Open, FileAccess.Read, FileShare.None);
            PlayerDataFile clone = (PlayerDataFile)formatter.Deserialize(stream);
            stream.Close();

            File.Delete("PlayerDataFile");*/

            Stream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);

            stream.Seek(0, SeekOrigin.Begin);
            PlayerDataFile clone = (PlayerDataFile)formatter.Deserialize(stream);
            stream.Close();

            return clone;
        }

        public void Read(string path) {
            BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open));

            if (reader.ReadChar() == 't' && reader.ReadChar() == 't' && reader.ReadChar() == 'p' &&
                reader.ReadChar() == '\0') {
                saveFileVersion = new Value<uint>((uint)reader.ReadByte());

                //Adding version checks to the segments. This will make the app blowup
                //where an unknown version has been introduced.
                if (saveFileVersion.Get() > 36) //Last known version is 35.
                    throw new Exception("Unknown save file version! " + saveFileVersion);

                ecd = new EntityCreationData();
                ecd.Read(reader);
                food = new LiveStats();
                food.Read(reader);
                drink = new LiveStats();
                drink.Read(reader);

                inventory = ItemStack.ReadItemStack(reader);
                selectedInventorySlot = new Value<int>((int)reader.ReadByte());
                bag = ItemStack.ReadItemStack(reader);

                //num
                int alreadyCraftedListLength = (int)reader.ReadUInt16();
                alreadyCraftedList = new HashSet<string>();
                for (int i = 0; i < alreadyCraftedListLength; i++) {
                    alreadyCraftedList.Add(reader.ReadString());
                }

                //b
                byte spawnPointsCount = reader.ReadByte();
                spawnPoints = new List<Vector3D<int>>();
                for (int i = 0; i < (int)spawnPointsCount; i++) {
                    Vector3D<int> spawnPoint = new Vector3D<int>();
                    spawnPoint.x = new Value<int>(reader.ReadInt32());
                    spawnPoint.y = new Value<int>(reader.ReadInt32());
                    spawnPoint.z = new Value<int>(reader.ReadInt32());

                    spawnPoints.Add(spawnPoint);
                }

                selectedSpawnPointKey = new Value<long>(reader.ReadInt64());

                randomBoolean = new Value<bool>(reader.ReadBoolean());
                randomShort = new Value<short>(reader.ReadInt16());

                bLoaded = new Value<bool>(reader.ReadBoolean());

                lastSpawnPosition = new Vector3D<int>();
                lastSpawnPosition.x = new Value<int>(reader.ReadInt32());
                lastSpawnPosition.y = new Value<int>(reader.ReadInt32());
                lastSpawnPosition.z = new Value<int>(reader.ReadInt32());
                lastSpawnPosition.heading = new Value<float>(reader.ReadSingle());

                id = new Value<int>(reader.ReadInt32());

                droppedBackpackPosition = new Vector3D<int>();
                droppedBackpackPosition.x = new Value<int>(reader.ReadInt32());
                droppedBackpackPosition.y = new Value<int>(reader.ReadInt32());
                droppedBackpackPosition.z = new Value<int>(reader.ReadInt32());

                playerKills = new Value<int>(reader.ReadInt32());
                zombieKills = new Value<int>(reader.ReadInt32());
                deaths = new Value<int>(reader.ReadInt32());
                score = new Value<int>(reader.ReadInt32());
                equipment = Equipment.Read(reader);

                //num
                int recipeCount = (int)reader.ReadUInt16();
                unlockedRecipeList = new List<string>();
                for (int i = 0; i < recipeCount; i++) {
                    unlockedRecipeList.Add(reader.ReadString());
                }

                randomUShort = new Value<ushort>(reader.ReadUInt16());

                markerPosition = new Vector3D<int>();
                markerPosition.x = new Value<int>(reader.ReadInt32());
                markerPosition.y = new Value<int>(reader.ReadInt32());
                markerPosition.z = new Value<int>(reader.ReadInt32());

                favoriteEquipment = Equipment.Read(reader);
                experience = new Value<uint>(reader.ReadUInt32());
                level = new Value<int>(reader.ReadInt32());

                bCrouchedLocked = new Value<bool>(reader.ReadBoolean());
                craftingData = new CraftingData();
                craftingData.Read(reader);

                //num
                int favoriteRecipeListSize = (int)reader.ReadUInt16();
                favoriteRecipeList = new List<string>();
                for (int i = 0; i < favoriteRecipeListSize; i++) {
                    favoriteRecipeList.Add(reader.ReadString());
                }

                //num2
                int memoryStreamSize = (int)reader.ReadUInt32();

                skills = new Skills();
                if (memoryStreamSize > 0) {
                    skillStream = new MemoryStream(reader.ReadBytes(memoryStreamSize));
                    skills.Read(new BinaryReader(skillStream));
                }

                totalItemsCrafted = new Value<uint>(reader.ReadUInt32());
                distanceWalked = new Value<float>(reader.ReadSingle());
                longestLife = new Value<float>(reader.ReadSingle());

                if (saveFileVersion.Get() > 35)
                    gameStageLifetimeTicks = new Value<ulong>(reader.ReadUInt64());

                waypoints = new WaypointCollection();
                waypoints.Read(reader);

                skillPoints = new Value<int>(reader.ReadInt32());

                questJournal = new QuestJournal();
                questJournal.Read(reader);

                deathUpdateTime = new Value<int>(reader.ReadInt32());
                currentLife = new Value<float>(reader.ReadSingle());
                bDead = new Value<bool>(reader.ReadBoolean());

                //irelevant byte
                reader.ReadByte();

                //My own special varible!
                reader.ReadBoolean();

                playerJournal = new PlayerJournal();
                playerJournal.Read(reader);

                rentedVMPosition = new Vector3D<int>();
                rentedVMPosition.x = new Value<int>(reader.ReadInt32());
                rentedVMPosition.y = new Value<int>(reader.ReadInt32());
                rentedVMPosition.z = new Value<int>(reader.ReadInt32());

                rentalEndTime = new Value<ulong>(reader.ReadUInt64());

                int trackedFriendEntityIdsSize = reader.ReadUInt16();
                trackedFriendEntityIds = new List<int>();
                for (int i = 0; i < trackedFriendEntityIdsSize; ++i) {
                    trackedFriendEntityIds.Add(reader.ReadInt32());
                }

                reader.Close();
            }
            else {
                throw new IOException("Save file corrupted!");
            }
        }

        public void Write(string path) {
            BinaryWriter writer = new BinaryWriter(new FileStream(path, FileMode.Create));

            writer.Write('t');
            writer.Write('t');
            writer.Write('p');
            writer.Write((byte)0);
            writer.Write((byte)saveFileVersion.Get());

            ecd.Write(writer);
            food.Write(writer);
            drink.Write(writer);

            ItemStack.WriteItemStack(writer, inventory);
            writer.Write((byte)selectedInventorySlot.Get());
            ItemStack.WriteItemStack(writer, bag);

            writer.Write((ushort)alreadyCraftedList.Count);
            HashSet<string>.Enumerator enumerator = alreadyCraftedList.GetEnumerator();
            while (enumerator.MoveNext()) {
                writer.Write(enumerator.Current);
            }

            writer.Write((byte)spawnPoints.Count);
            for (int i = 0; i < spawnPoints.Count; i++) {
                writer.Write(spawnPoints[i].x.Get());
                writer.Write(spawnPoints[i].y.Get());
                writer.Write(spawnPoints[i].z.Get());
            }

            writer.Write(selectedSpawnPointKey.Get());
            writer.Write(randomBoolean.Get());
            writer.Write(randomShort.Get());
            writer.Write(bLoaded.Get());
            writer.Write((int)lastSpawnPosition.x.Get());
            writer.Write((int)lastSpawnPosition.y.Get());
            writer.Write((int)lastSpawnPosition.z.Get());
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
            List<string>.Enumerator enumerator2 = unlockedRecipeList.GetEnumerator();
            while (enumerator2.MoveNext()) {
                writer.Write(enumerator2.Current);
            }

            writer.Write(randomUShort.Get());
            writer.Write(markerPosition.x.Get());
            writer.Write(markerPosition.y.Get());
            writer.Write(markerPosition.z.Get());
            favoriteEquipment.Write(writer);
            writer.Write(experience.Get());
            writer.Write(level.Get());
            writer.Write(bCrouchedLocked.Get());
            craftingData.Write(writer);

            writer.Write((ushort)favoriteRecipeList.Count);
            List<string>.Enumerator enumerator3 = favoriteRecipeList.GetEnumerator();
            while (enumerator3.MoveNext()) {
                writer.Write(enumerator3.Current);
            }

            skillStream = new MemoryStream();
            skills.Write(new BinaryWriter(skillStream));
            byte[] array = skillStream.ToArray();
            writer.Write((uint)array.Length);
            if (array.Length > 0) {
                writer.Write(array);
            }

            writer.Write(totalItemsCrafted.Get());
            writer.Write(distanceWalked.Get());
            writer.Write(longestLife.Get());

            if (saveFileVersion.Get() > 35)
                writer.Write(gameStageLifetimeTicks.Get());

            waypoints.Write(writer);
            writer.Write(skillPoints.Get());

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

            writer.Close();
        }
    }
}