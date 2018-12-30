using System;
using System.Collections.Generic;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    public class WaypointCollection {

        //List
        public List<Waypoint> waypoints;

        public WaypointCollection() {}

        internal WaypointCollection(TypedBinaryReader reader) {
            Read(reader);
        }

        internal void Read(TypedBinaryReader reader) {
            Utils.VerifyVersion(reader.ReadByte(), SaveVersionConstants.WAYPOINT_COLLECTION);

            ushort waypointListLength = reader.ReadUInt16();
            waypoints = new List<Waypoint>(waypointListLength);
            for (short i = 0; i < waypointListLength; ++i) {
                waypoints.Add(new Waypoint(reader));
            }
        }

        internal void Write(TypedBinaryWriter writer) {
            writer.Write(SaveVersionConstants.WAYPOINT_COLLECTION);

            writer.Write((ushort)waypoints.Count);
            foreach(Waypoint waypoint in waypoints) {
                waypoint.Write(writer);
            }
        }
    }
}