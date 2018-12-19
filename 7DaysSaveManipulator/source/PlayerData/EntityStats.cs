using SevenDaysSaveManipulator.source.PlayerData;
using System;
using System.Collections.Generic;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class EntityStats {

        //CoreTemp
        public Stat coreTemp;

        //Health
        public Stat health;

        //m_immunity
        public int[] immunity = new int[13];

        //Stamina
        public Stat stamina;

        //Water
        public Stat water;

        //m_seekWaterLevel
        public Value<float> seekWaterLevel;

        public EntityStats() {}

        internal EntityStats(BinaryReader reader) {
            Read(reader);
        }

        public void Read(BinaryReader reader) {
            //num
            Utils.VerifyVersion<int>(reader.ReadInt32(), SaveVersionConstants.ENTITY_STATS);

            //num2
            int immunityLength = reader.ReadInt32();
            for (int i = 0; i < immunityLength; ++i) {
                //num3
                immunity[i] = reader.ReadInt32();
            }

            health = new Stat(reader);
            stamina = new Stat(reader);
            coreTemp = new Stat(reader);
            water = new Stat(reader);

            seekWaterLevel = new Value<float>(reader.ReadSingle());
        }

        public void Write(BinaryWriter writer) {
            writer.Write(SaveVersionConstants.ENTITY_STATS);

            writer.Write(immunity.Length);
            for (int i = 0; i < immunity.Length; ++i) {
                writer.Write(immunity[i]);
            }

            health.Write(writer);
            stamina.Write(writer);
            coreTemp.Write(writer);
            water.Write(writer);

            writer.Write(seekWaterLevel.Get());
        }
    }
}