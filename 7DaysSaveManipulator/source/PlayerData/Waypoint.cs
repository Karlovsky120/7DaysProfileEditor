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

        public Value<int> ownerId;
        public Value<int> entityId;

        public void Read(BinaryReader reader, Value<byte> version) {
            pos = new Vector3D<int>();
            pos.x = new Value<int>(reader.ReadInt32());
            pos.y = new Value<int>(reader.ReadInt32());
            pos.z = new Value<int>(reader.ReadInt32());

            icon = new Value<string>(reader.ReadString());
            name = new Value<string>(reader.ReadString());
            bTracked = new Value<bool>(reader.ReadBoolean());

            //Version 2
            if (version.Get() >= 2) {
                ownerId = new Value<int>(reader.ReadInt32());
                entityId = new Value<int>(reader.ReadInt32());
            }
        }

        public void Write(BinaryWriter writer, Value<byte> version) {

            writer.Write(pos.x.Get());
            writer.Write(pos.y.Get());
            writer.Write(pos.z.Get());

            writer.Write(icon.Get());
            writer.Write(name.Get());
            writer.Write(bTracked.Get());

            //We ahve to preserve their format, if they are Version 1 save as Version 1
            //Adding Version 2 variables to their TTP will break it.
            //Version 2
            if (version.Get() >= 2) {
                writer.Write(ownerId.Get());
                writer.Write(entityId.Get());
            }
        }
    }
}