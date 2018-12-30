using System;
using System.Collections.Generic;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    public class PlayerJournal {

        //List
        public List<JournalEntry> journalEntries;

        public PlayerJournal() {}

        internal PlayerJournal(TypedBinaryReader reader) {
            Read(reader);
        }

        internal void Read(TypedBinaryReader reader) {
            Utils.VerifyVersion(reader.ReadByte(), SaveVersionConstants.PLAYER_JOURNAL);

            short journalEntryCount = reader.ReadInt16();
            journalEntries = new List<JournalEntry>(journalEntryCount);
            for (int i = 0; i < journalEntryCount; ++i) {
                journalEntries.Add(JournalEntry.Instantiate((JournalEntry.JournalEntryType)reader.ReadByte(), reader));
            }
        }

        internal void Write(TypedBinaryWriter writer) {
            writer.Write(SaveVersionConstants.PLAYER_JOURNAL);

            writer.Write((ushort)journalEntries.Count);
            foreach (JournalEntry entry in journalEntries) {
                entry.Write(writer);
            }
        }
    }
}