using System;
using System.Collections.Generic;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    public class EntityStats {

        //CoreTemp
        public Stat coreTemp;

        //Health
        public Stat health;

        //m_immunity
        public List<int> immunity;

        //Stamina
        public Stat stamina;

        //Water
        public Stat water;

        //m_seekWaterLevel
        public Value<float> seekWaterLevel;

        public EntityStats() {}

        internal EntityStats(TypedBinaryReader reader) {
            Read(reader);
        }

        internal void Read(TypedBinaryReader reader) {
            Utils.VerifyVersion<int>(reader.ReadInt32(), SaveVersionConstants.ENTITY_STATS, "EntityStats");

            int immunityLength = reader.ReadInt32();
            immunity = new List<int>(immunityLength);
            for (int i = 0; i < immunityLength; ++i) {
                immunity[i] = reader.ReadInt32();
            }

            health = new Stat(reader);
            stamina = new Stat(reader);
            coreTemp = new Stat(reader);
            water = new Stat(reader);

            seekWaterLevel = new Value<float>(reader);
        }

        internal void Write(TypedBinaryWriter writer) {
            writer.Write(SaveVersionConstants.ENTITY_STATS);

            writer.Write(immunity.Count);
            foreach (int immunityItem in immunity) {
                writer.Write(immunityItem);
            }

            health.Write(writer);
            stamina.Write(writer);
            coreTemp.Write(writer);
            water.Write(writer);

            seekWaterLevel.Write(writer);
        }
    }
}