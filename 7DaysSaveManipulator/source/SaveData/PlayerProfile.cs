using System;
using System.Collections.Generic;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    public class PlayerProfile {

        //Archetype
        public Value<string> archetype;

        //BeardName
        public Value<string> beardName;

        //Dna.Values
        public List<float> dnaValues;

        //EyeColor
        public Vector3D<float> eyeColor;

        //EyebrowColor
        public Vector3D<float> eyebrowColor;

        //EyebrowName
        public Value<string> eyebrowName;

        //HairColor
        public Vector3D<float> hairColor;

        //HairName
        public Value<string> hairName;

        //IsMale
        public Value<bool> isMale;

        //SkinColor
        public Vector3D<float> skinColor;

        //Hardcoded in the game code
        private readonly int dnaCount = 46;

        public PlayerProfile() {}

        internal PlayerProfile(TypedBinaryReader reader) {
            Read(reader);
        }

        internal void Read(TypedBinaryReader reader) {
            Utils.VerifyVersion(reader.ReadInt32(), SaveVersionConstants.PLAYER_PROFILE);

            archetype = new Value<string>(reader);
            isMale = new Value<bool>(reader);
            hairColor = new Vector3D<float>(reader);
            skinColor = new Vector3D<float>(reader);
            eyeColor = new Vector3D<float>(reader);
            eyebrowColor = new Vector3D<float>(reader);

            hairName = new Value<string>(reader);
            eyebrowName = new Value<string>(reader);
            beardName = new Value<string>(reader);

            dnaValues = new List<float>(dnaCount);
            for (int i = 0; i < dnaCount; ++i) {
                dnaValues.Add(reader.ReadSingle());
            }
        }

        internal void Write(TypedBinaryWriter writer) {
            writer.Write(SaveVersionConstants.PLAYER_PROFILE);

            archetype.Write(writer);
            isMale.Write(writer);

            hairColor.Write(writer);
            skinColor.Write(writer);
            eyeColor.Write(writer);
            eyebrowColor.Write(writer);

            eyebrowName.Write(writer);
            hairName.Write(writer);
            beardName.Write(writer);

            foreach (float dnaValue in dnaValues) {
                writer.Write(dnaValue);
            }
        }
    }
}