using System;
using System.Collections.Generic;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class Skills {

        //Q
        public Dictionary<int, Skill> skillDictionary;

        public void Read(BinaryReader reader) {
            //num
            int skillNumber = reader.ReadInt32();
            skillDictionary = new Dictionary<int, Skill>();
            for (int i = 0; i < skillNumber; i++) {
                Skill skill = new Skill();
                int key = reader.ReadInt32();

                skill.Read(reader);
                skill.parent = this;

                if (skillDictionary.ContainsKey(key)) {
                    skillDictionary[key] = skill;
                }
                else {
                    skillDictionary.Add(key, skill);
                }
            }
        }

        public void Write(BinaryWriter writer) {
            int[] array = new int[skillDictionary.Count];
            skillDictionary.Keys.CopyTo(array, 0);
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            writer.Write(array.Length);

            for (int i = 0; i < array.Length; i++) {
                binaryWriter.Write(array[i]);
                skillDictionary[array[i]].Write(binaryWriter);
            }

            writer.Write(memoryStream.GetBuffer());
        }
    }
}