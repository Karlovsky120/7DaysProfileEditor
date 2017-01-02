using System;
using System.Collections.Generic;
using System.IO;

namespace SevenDaysSaveManipulator.GameData {

    [Serializable]
    public class WaypointCollection {

        //notSaved = 1
        public Value<byte> waypointCollectionVersion;

        //List
        public List<Waypoint> waypointList;

        public void Read(BinaryReader reader) {
            waypointCollectionVersion = new Value<byte>(reader.ReadByte());
            //num
            int listCount = (int)reader.ReadInt16();
            waypointList = new List<Waypoint>();
            for (int i = 0; i < listCount; i++) {
                Waypoint waypoint = new Waypoint();
                waypoint.Read(reader);
                waypointList.Add(waypoint);
            }
        }

        public void Write(BinaryWriter writer) {
            writer.Write(waypointCollectionVersion.Get());
            writer.Write((ushort)waypointList.Count);
            for (int i = 0; i < waypointList.Count; i++) {
                waypointList[i].Write(writer);
            }
        }
    }
}