using SevenDaysSaveManipulator.PlayerData;
using System;
using System.Collections.Generic;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class PlayerJournal {

        //List
        public List<JournalEntry> journalEntries;

        //b
        public Value<byte> playerJournalVersion;

        public void Read(BinaryReader reader) {
            playerJournalVersion = new Value<byte>(reader.ReadByte());

            if (playerJournalVersion.Get() > 116)
                throw new Exception("Unknown PlayerJournal version!");

            journalEntries = new List<JournalEntry>();
            int journalEntryCount = reader.ReadInt16();
            for (int i = 0; i < journalEntryCount; ++i) {


                JournalEntry journalEntry = new JournalEntry();
                journalEntry.Read(reader);
                journalEntries.Add(journalEntry);


            }
        }

        public void Write(BinaryWriter writer) {
            writer.Write(playerJournalVersion.Get());
            writer.Write((ushort)journalEntries.Count);
            for (int i = 0; i < journalEntries.Count; ++i) {
                journalEntries[i].Write(writer);
            }
        }
    }
}