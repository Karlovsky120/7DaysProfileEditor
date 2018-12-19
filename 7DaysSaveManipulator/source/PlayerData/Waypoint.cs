using System;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

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

        //ownerId
        public Value<int> ownerId;

        //entityId
        public Value<int> entityId;

        public Waypoint() {}

        internal Waypoint(BinaryReader reader) {
            Read(reader);
        }

        internal void Read(BinaryReader reader) {
            pos = new Vector3D<int> {
                x = new Value<int>(reader.ReadInt32()),
                y = new Value<int>(reader.ReadInt32()),
                z = new Value<int>(reader.ReadInt32())
            };

            icon = new Value<string>(reader.ReadString());
            name = new Value<string>(reader.ReadString());
            bTracked = new Value<bool>(reader.ReadBoolean());

            ownerId = new Value<int>(reader.ReadInt32());
            entityId = new Value<int>(reader.ReadInt32());
        }

        internal void Write(BinaryWriter writer) {
            writer.Write(pos.x.Get());
            writer.Write(pos.y.Get());
            writer.Write(pos.z.Get());

            writer.Write(icon.Get());
            writer.Write(name.Get());
            writer.Write(bTracked.Get());

            writer.Write(ownerId.Get());
            writer.Write(entityId.Get());
        }
    }
}