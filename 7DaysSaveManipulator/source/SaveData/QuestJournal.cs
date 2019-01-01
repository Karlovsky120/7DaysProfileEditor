using System;
using System.Collections.Generic;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    public class QuestJournal {

        //quests
        public List<Quest> quests = new List<Quest>();

        public QuestJournal() {}

        internal QuestJournal(TypedBinaryReader reader, AdditionalFileData xmlData) {
            Read(reader, xmlData);
        }

        internal void Read(TypedBinaryReader reader, AdditionalFileData xmlData) {
            Utils.VerifyVersion(reader.ReadByte(), SaveVersionConstants.QUEST_JOURNAL, "QuestJournal");

            short questNumber = reader.ReadInt16();
            for (int i = 0; i < questNumber; ++i) {
                quests.Add(new Quest(reader, xmlData) {
                    ownerJournal = this
                });
            }
        }

        internal void Write(TypedBinaryWriter writer) {
            writer.Write(SaveVersionConstants.QUEST_JOURNAL);

            ushort count = (ushort)quests.Count;
            writer.Write(count);
            foreach (Quest quest in quests) {
                quest.Write(writer);
            }
        }
    }
}