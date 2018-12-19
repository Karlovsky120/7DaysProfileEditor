using SevenDaysSaveManipulator.source.PlayerData;
using SevenDaysXMLParser.Quests;
using System;
using System.Collections.Generic;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class QuestJournal {

        //quests
        public List<Quest> quests = new List<Quest>();

        public QuestJournal() {}

        internal QuestJournal(BinaryReader reader, QuestsXml questsXml) {
            Read(reader);
        }

        internal void Read(BinaryReader reader) {
            Utils.VerifyVersion(reader.ReadByte(), SaveVersionConstants.QUEST_JOURNAL);

            //num
            short questNumber = reader.ReadInt16();
            for (int i = 0; i < questNumber; ++i) {
                quests.Add(new Quest(reader) {
                    ownerJournal = this
                });
            }
        }

        internal void Write(BinaryWriter writer) {
            writer.Write(SaveVersionConstants.QUEST_JOURNAL);
            ushort count = (ushort)quests.Count;
            writer.Write(count);
            for (ushort i = 0; i < count; ++i) {
                quests[i].Write(writer);
            }
        }
    }
}