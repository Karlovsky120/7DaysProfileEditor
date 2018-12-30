using System;
using System.Collections.Generic;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    public class EntitySpawner {

        //position
        public Vector3D<int> position;

        //size
        public Vector3D<short> size;

        //triggerDiameter
        public Value<uint> triggerDiameter;

        //entitySpawnerClassName
        public Value<string> entitySpawnerClassName;

        //totalSpawnedThisWave
        public Value<short> totalSpawnedThisWave;

        //timeDelayToNextWave
        public Value<float> timeDelayToNextWave;

        //timeDelayBetweenSpawns
        public Value<float> timeDelayBetweenSpawns;

        //entityIdSpawner
        public List<int> entityIdSpawned;

        //currentWave
        public Value<short> currentWave;

        //lastDaySpawnCalled
        public Value<int> lastDaySpawnCalled;

        //numberToSpawnThisWave
        public Value<int> numberToSpawnThisWave;

        //worldTimeNextWave
        public Value<ulong> worldTimeNextWave;

        //bCaveSpawn
        public Value<bool> bCaveSpawn;

        public EntitySpawner() {}

        public EntitySpawner(TypedBinaryReader reader) {
            Read(reader);
        }

        internal void Read(TypedBinaryReader reader) {
            Utils.VerifyVersion(reader.ReadByte(), SaveVersionConstants.ENTITY_SPAWNER);
            
            position = new Vector3D<int>(reader);
            size = new Vector3D<short>(reader);

            triggerDiameter = new Value<uint>(reader);
            entitySpawnerClassName = new Value<string>(reader);

            totalSpawnedThisWave = new Value<short>(reader);
            timeDelayToNextWave = new Value<float>(reader);
            timeDelayBetweenSpawns = new Value<float>(reader);

            ushort entityIdSpawnedCount = reader.ReadUInt16();
            Utils.VerifyVersion(reader.ReadByte(), SaveVersionConstants.ENTITY_SPAWNER_SECOND);
            entityIdSpawned = new List<int>(entityIdSpawnedCount);
            for(int i = 0; i < entityIdSpawnedCount; ++i) {
                entityIdSpawned.Add(reader.ReadInt32());
            }

            currentWave = new Value<short>(reader);
            lastDaySpawnCalled = new Value<int>(reader);
            numberToSpawnThisWave = new Value<int>(reader);

            worldTimeNextWave = new Value<ulong>(reader);
            bCaveSpawn = new Value<bool>(reader);
        }

        internal void Write(TypedBinaryWriter writer) {
            writer.Write(SaveVersionConstants.ENTITY_SPAWNER);

            position.Write(writer);
            size.Write(writer);

            triggerDiameter.Write(writer);
            entitySpawnerClassName.Write(writer);

            totalSpawnedThisWave.Write(writer);
            timeDelayToNextWave.Write(writer);
            timeDelayBetweenSpawns.Write(writer);

            writer.Write(entityIdSpawned.Count);
            writer.Write(SaveVersionConstants.ENTITY_SPAWNER_SECOND);
            foreach (int entityID in entityIdSpawned) {
                writer.Write(entityID);
            }

            currentWave.Write(writer);
            lastDaySpawnCalled.Write(writer);
            numberToSpawnThisWave.Write(writer);
            worldTimeNextWave.Write(writer);
            bCaveSpawn.Write(writer);
        }
    }
}
