using System;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    class JournalEntryOther : JournalEntry {

        //ID
        public Value<string> id;

        //read
        public Value<bool> read;

        //timestamp
        public Value<ulong> timestamp;

        public JournalEntryOther() {}

        internal JournalEntryOther(TypedBinaryReader reader) {
            Read(reader);
        }

        internal override void Read(TypedBinaryReader reader) {
            base.Read(reader);

            id = new Value<string>(reader);
            read = new Value<bool>(reader);
            timestamp = new Value<ulong>(reader);
        }

        internal override void Write(TypedBinaryWriter writer) {
            base.Write(writer);

            id.Write(writer);
            read.Write(writer);
            timestamp.Write(writer);
        }
    }
}
