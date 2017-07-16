using System;
using System.Collections.Generic;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class QuestJournal {

        //notSaved = 1
        public Value<byte> questJournalVersion;

        //quests
        public List<Quest> quests = new List<Quest>();

        public void Read(BinaryReader reader) {
            questJournalVersion = new Value<byte>(reader.ReadByte());

            if (questJournalVersion.Get() > 1)
                throw new Exception("Unknown QuestJournal version! " + questJournalVersion.Get());

            //num
            int questNumber = reader.ReadInt16();

            for (int i = 0; i < questNumber; i++) {
                //iD
                string id = reader.ReadString();
                //b
                byte currentQuestVersion = reader.ReadByte();

                Quest quest = new Quest();
                quest.id = id;
                quest.currentQuestVersion = new Value<byte>(currentQuestVersion);
                quest.Read(reader);
                quest.ownerJournal = this;

                quests.Add(quest);
            }
        }

        public void Write(BinaryWriter writer) {
            writer.Write(questJournalVersion.Get());
            int count = quests.Count;
            writer.Write((ushort)count);
            for (int i = 0; i < count; i++) {
                quests[i].Write(writer);
            }
        }
    }
}