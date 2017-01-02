using System;
using System.IO;

namespace SevenDaysSaveManipulator.GameData {

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

        public void Read(BinaryReader reader) {
            pos = new Vector3D<int>();
            pos.x = new Value<int>(reader.ReadInt32());
            pos.y = new Value<int>(reader.ReadInt32());
            pos.z = new Value<int>(reader.ReadInt32());

            icon = new Value<string>(reader.ReadString());
            name = new Value<string>(reader.ReadString());
            bTracked = new Value<bool>(reader.ReadBoolean());
        }

        public void Write(BinaryWriter writer) {
            writer.Write(pos.x.Get());
            writer.Write(pos.y.Get());
            writer.Write(pos.z.Get());

            writer.Write(icon.Get());
            writer.Write(name.Get());
            writer.Write(bTracked.Get());
        }
    }
}