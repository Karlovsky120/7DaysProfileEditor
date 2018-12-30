using System;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    public class Waypoint {

        //bTracked
        public Value<bool> bTracked;

        //icon
        public Value<string> icon;

        //name
        public Value<string> name;

        //pos
        public Vector3D<int> pos;

        //ownerID
        public Value<int> ownerID;

        //entityId
        public Value<int> entityId;

        public Waypoint() {}

        internal Waypoint(TypedBinaryReader reader) {
            Read(reader);
        }

        internal void Read(TypedBinaryReader reader) {
            pos = new Vector3D<int>(reader);

            icon = new Value<string>(reader);
            name = new Value<string>(reader);
            bTracked = new Value<bool>(reader);

            ownerID = new Value<int>(reader);
            entityId = new Value<int>(reader);
        }

        internal void Write(TypedBinaryWriter writer) {
            pos.Write(writer);

            icon.Write(writer);
            name.Write(writer);
            bTracked.Write(writer);

            ownerID.Write(writer);
            entityId.Write(writer);
        }
    }
}