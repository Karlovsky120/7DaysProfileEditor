using System;
using System.Collections.Generic;
using System.Reflection;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    public class JournalEntry {

        public enum JournalEntryType {
            Invalid,
            Tip,
            Death,
            Level,
            Friend
        }

        //EntryType
        public Value<JournalEntryType> entryType;

        private readonly static Dictionary<JournalEntryType, Type> instantiationBindings = new Dictionary<JournalEntryType, Type>() {
            {JournalEntryType.Invalid, typeof(JournalEntryOther)},
            {JournalEntryType.Tip, typeof(JournalEntryTip)},
            {JournalEntryType.Death, typeof(JournalEntryOther)},
            {JournalEntryType.Level, typeof(JournalEntryOther)},
            {JournalEntryType.Friend, typeof(JournalEntryOther)}
        };

        public JournalEntry() {}

        internal JournalEntry(TypedBinaryReader reader) {
            Read(reader);
        }

        internal virtual void Read(TypedBinaryReader reader) {
            Utils.VerifyVersion(reader.ReadByte(), SaveVersionConstants.JOURNAL_ENTRY, "JournalEntry");
        }

        internal virtual void Write(TypedBinaryWriter writer) {
            writer.Write((byte)entryType.Get());

            writer.Write(SaveVersionConstants.JOURNAL_ENTRY);           
        }

        internal static JournalEntry Instantiate(JournalEntryType type, TypedBinaryReader reader) {
            JournalEntry instance = (JournalEntry)instantiationBindings[type].GetTypeInfo().GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)[0].Invoke(new[] {reader});
            instance.entryType = new Value<JournalEntryType>(type);
            return instance;
        }
    }
}