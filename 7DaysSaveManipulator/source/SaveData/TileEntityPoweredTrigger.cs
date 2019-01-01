namespace SevenDaysSaveManipulator.SaveData {

    class TileEntityPoweredTrigger : TileEntityPowered {

        public enum TriggerType : byte {
            Switch,
            PressurePlate,
            TimerRelay,
            Motion,
            TripWire
        }

        //ownerID
        public Value<string> ownerID;

        //TriggerType
        public Value<TriggerType> triggerType;

        public TileEntityPoweredTrigger() {}

        internal TileEntityPoweredTrigger(TypedBinaryReader reader) {
            Read(reader);
        }

        internal override void Read(TypedBinaryReader reader, AdditionalFileData xmlData = null) {
            base.Read(reader);

            triggerType = new Value<TriggerType>((TriggerType)reader.ReadByte());

            if (triggerType.Get() == TriggerType.Motion) {
                ownerID = new Value<string>(reader);
            }
        }

        internal override void Write(TypedBinaryWriter writer) {
            base.Write(writer);

            writer.Write((byte)triggerType.Get());

            if (triggerType.Get() == TriggerType.Motion) {
                ownerID.Write(writer);
            }
        }
    }
}
