using SevenDaysSaveManipulator.source.PlayerData;
using System;
using System.Collections.Generic;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class WaypointCollection {

        //List
        public List<Waypoint> waypoints;

        public WaypointCollection() {}

        internal WaypointCollection(BinaryReader reader) {
            Read(reader);
        }

        internal void Read(BinaryReader reader) {
            Utils.VerifyVersion(reader.ReadByte(), SaveVersionConstants.WAYPOINT_COLLECTION);

            waypoints = new List<Waypoint>();
            ushort waypointListLength = reader.ReadUInt16();
            for (short i = 0; i < waypointListLength; ++i) {
                waypoints.Add(new Waypoint(reader));
            }
        }

        internal void Write(BinaryWriter writer) {
            writer.Write(SaveVersionConstants.WAYPOINT_COLLECTION);
            writer.Write((ushort)waypoints.Count);
            foreach(Waypoint waypoint in waypoints) {
                waypoint.Write(writer);
            }
        }
    }
}