using System;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class BodyDamage {

        //num = 2
        public Value<int> bodyDamageVersion;

        public Value<int> Chest;
        public Value<bool> CrippledLeftLeg;
        public Value<bool> CrippledRightLeg;
        public Value<bool> DismemberedHead;
        public Value<bool> DismemberedLeftLowerArm;
        public Value<bool> DismemberedLeftLowerLeg;
        public Value<bool> DismemberedLeftUpperArm;
        public Value<bool> DismemberedLeftUpperLeg;
        public Value<bool> DismemberedRightLowerArm;
        public Value<bool> DismemberedRightLowerLeg;
        public Value<bool> DismemberedRightUpperArm;
        public Value<bool> DismemberedRightUpperLeg;
        public Value<int> Head;
        public Value<int> LeftLowerArm;
        public Value<int> LeftLowerLeg;
        public Value<int> LeftUpperArm;
        public Value<int> LeftUpperLeg;
        public Value<int> RightLowerArm;
        public Value<int> RightLowerLeg;
        public Value<int> RightUpperArm;
        public Value<int> RightUpperLeg;

        public void Read(BinaryReader reader) {
            bodyDamageVersion = new Value<int>(reader.ReadInt32());

            LeftUpperLeg = new Value<int>((int)reader.ReadInt16());
            RightUpperLeg = new Value<int>((int)reader.ReadInt16());
            LeftUpperArm = new Value<int>((int)reader.ReadInt16());
            RightUpperArm = new Value<int>((int)reader.ReadInt16());
            Chest = new Value<int>((int)reader.ReadInt16());
            Head = new Value<int>((int)reader.ReadInt16());
            DismemberedLeftUpperArm = new Value<bool>(reader.ReadBoolean());
            DismemberedRightUpperArm = new Value<bool>(reader.ReadBoolean());
            DismemberedHead = new Value<bool>(reader.ReadBoolean());
            DismemberedRightUpperLeg = new Value<bool>(reader.ReadBoolean());
            CrippledRightLeg = new Value<bool>(reader.ReadBoolean());

            LeftLowerLeg = new Value<int>((int)reader.ReadInt16());
            RightLowerLeg = new Value<int>((int)reader.ReadInt16());
            LeftLowerArm = new Value<int>((int)reader.ReadInt16());
            RightLowerArm = new Value<int>((int)reader.ReadInt16());
            DismemberedLeftLowerArm = new Value<bool>(reader.ReadBoolean());
            DismemberedRightLowerArm = new Value<bool>(reader.ReadBoolean());
            DismemberedLeftLowerLeg = new Value<bool>(reader.ReadBoolean());
            DismemberedRightLowerLeg = new Value<bool>(reader.ReadBoolean());

            DismemberedLeftUpperLeg = new Value<bool>(reader.ReadBoolean());
            CrippledLeftLeg = new Value<bool>(reader.ReadBoolean());
        }

        public void Write(BinaryWriter writer) {
            writer.Write(bodyDamageVersion.Get());
            writer.Write((short)LeftUpperLeg.Get());
            writer.Write((short)RightUpperLeg.Get());
            writer.Write((short)LeftUpperArm.Get());
            writer.Write((short)RightUpperArm.Get());

            writer.Write((short)Chest.Get());
            writer.Write((short)Head.Get());
            writer.Write(DismemberedLeftUpperArm.Get());
            writer.Write(DismemberedRightUpperArm.Get());
            writer.Write(DismemberedHead.Get());
            writer.Write(DismemberedRightUpperLeg.Get());

            writer.Write(CrippledRightLeg.Get());
            writer.Write((short)LeftLowerLeg.Get());
            writer.Write((short)RightLowerLeg.Get());
            writer.Write((short)LeftLowerArm.Get());
            writer.Write((short)RightLowerArm.Get());
            writer.Write(DismemberedLeftLowerArm.Get());
            writer.Write(DismemberedRightLowerArm.Get());
            writer.Write(DismemberedLeftLowerLeg.Get());
            writer.Write(DismemberedRightLowerLeg.Get());
            writer.Write(DismemberedLeftUpperLeg.Get());
            writer.Write(CrippledLeftLeg.Get());
        }
    }
}