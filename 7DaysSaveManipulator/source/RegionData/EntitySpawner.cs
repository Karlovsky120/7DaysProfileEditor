using SevenDaysSaveManipulator.PlayerData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysSaveManipulator.RegionData {
    public class EntitySpawner {

        public byte version;
        public Vector3D<int> position;
        public Vector3D<int> size;
        public int triggerDiameter;
        public string entitySpawnerClassName;

        public int totalSpawnedThisWave;
        public float timeDelayToNextWave;
        public float timeDelayBetweenSpawns;

        public uint idVersion;

        public List<int> entitySpawnedIdList = new List<int>();

        public int currentWave;
        public int lastDaySpawnCalled;
        public int numberToSpawnThisWave;

        public ulong worldTimeNextWave;
        public bool bCaveSpawn;

        public void Read(BinaryReader reader) {

            version = reader.ReadByte();
            position = new Vector3D<int>();
            position.x = new Value<int>(reader.ReadInt32());
            position.y = new Value<int>(reader.ReadInt32());
            position.z = new Value<int>(reader.ReadInt32());

            size = new Vector3D<int>();
            size.x = new Value<int>((int)reader.ReadUInt16());
            size.y = new Value<int>((int)reader.ReadUInt16());
            size.z = new Value<int>((int)reader.ReadUInt16());

            triggerDiameter = (int)reader.ReadUInt16();
            entitySpawnerClassName = reader.ReadString();

            totalSpawnedThisWave = (int)reader.ReadInt16();
            timeDelayToNextWave = reader.ReadSingle();
            timeDelayBetweenSpawns = reader.ReadSingle();

            int entitySpawnedIdCount = (int)reader.ReadUInt16();
            idVersion = (uint)reader.ReadByte();

            for(int i = 0; i < entitySpawnedIdCount; ++i) {
                entitySpawnedIdList.Add(reader.ReadInt32());
            }

            currentWave = (int)reader.ReadInt16();
            lastDaySpawnCalled = reader.ReadInt32();
            numberToSpawnThisWave = reader.ReadInt32();

            worldTimeNextWave = reader.ReadUInt64();
            bCaveSpawn = reader.ReadBoolean();
        }

        public void Write(BinaryWriter writer) {
            writer.Write(version);

            writer.Write(position.x.Get());
            writer.Write(position.y.Get());
            writer.Write(position.z.Get());

            writer.Write((short)size.x.Get());
            writer.Write((short)size.y.Get());
            writer.Write((short)size.z.Get());

            writer.Write((ushort)triggerDiameter);
            writer.Write(entitySpawnerClassName);
            writer.Write((short)totalSpawnedThisWave);
            writer.Write(timeDelayToNextWave);
            writer.Write(timeDelayBetweenSpawns);

            writer.Write((ushort)entitySpawnedIdList.Count);
            writer.Write((byte)idVersion);

            for (int i = 0; i < entitySpawnedIdList.Count; ++i) {
                writer.Write(entitySpawnedIdList[i]);
            }

            writer.Write((short)currentWave);
            writer.Write(lastDaySpawnCalled);
            writer.Write(numberToSpawnThisWave);
            writer.Write(worldTimeNextWave);
            writer.Write(bCaveSpawn);
        }
    }
}
