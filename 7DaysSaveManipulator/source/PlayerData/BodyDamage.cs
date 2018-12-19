using SevenDaysSaveManipulator.source.PlayerData;
using System;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class BodyDamage {

        public Value<short> Chest;
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
        public Value<short> Head;
        public Value<short> LeftLowerArm;
        public Value<short> LeftLowerLeg;
        public Value<short> LeftUpperArm;
        public Value<short> LeftUpperLeg;
        public Value<short> RightLowerArm;
        public Value<short> RightLowerLeg;
        public Value<short> RightUpperArm;
        public Value<short> RightUpperLeg;

        public BodyDamage() {}

        internal BodyDamage(BinaryReader reader) {
            Read(reader);
        }

        internal void Read(BinaryReader reader) {
            Utils.VerifyVersion(reader.ReadInt32(), SaveVersionConstants.BODY_DAMAGE);

            LeftUpperLeg = new Value<short>(reader.ReadInt16());
            RightUpperLeg = new Value<short>(reader.ReadInt16());
            LeftUpperArm = new Value<short>(reader.ReadInt16());
            RightUpperArm = new Value<short>(reader.ReadInt16());
            Chest = new Value<short>(reader.ReadInt16());
            Head = new Value<short>(reader.ReadInt16());
            DismemberedLeftUpperArm = new Value<bool>(reader.ReadBoolean());
            DismemberedRightUpperArm = new Value<bool>(reader.ReadBoolean());
            DismemberedHead = new Value<bool>(reader.ReadBoolean());
            DismemberedRightUpperLeg = new Value<bool>(reader.ReadBoolean());
            CrippledRightLeg = new Value<bool>(reader.ReadBoolean());

            LeftLowerLeg = new Value<short>(reader.ReadInt16());
            RightLowerLeg = new Value<short>(reader.ReadInt16());
            LeftLowerArm = new Value<short>(reader.ReadInt16());
            RightLowerArm = new Value<short>(reader.ReadInt16());
            DismemberedLeftLowerArm = new Value<bool>(reader.ReadBoolean());
            DismemberedRightLowerArm = new Value<bool>(reader.ReadBoolean());
            DismemberedLeftLowerLeg = new Value<bool>(reader.ReadBoolean());
            DismemberedRightLowerLeg = new Value<bool>(reader.ReadBoolean());

            DismemberedLeftUpperLeg = new Value<bool>(reader.ReadBoolean());
            CrippledLeftLeg = new Value<bool>(reader.ReadBoolean());
        }

        internal void Write(BinaryWriter writer) {
            writer.Write(SaveVersionConstants.BODY_DAMAGE);
            writer.Write(LeftUpperLeg.Get());
            writer.Write(RightUpperLeg.Get());
            writer.Write(LeftUpperArm.Get());
            writer.Write(RightUpperArm.Get());

            writer.Write(Chest.Get());
            writer.Write(Head.Get());
            writer.Write(DismemberedLeftUpperArm.Get());
            writer.Write(DismemberedRightUpperArm.Get());
            writer.Write(DismemberedHead.Get());
            writer.Write(DismemberedRightUpperLeg.Get());

            writer.Write(CrippledRightLeg.Get());
            writer.Write(LeftLowerLeg.Get());
            writer.Write(RightLowerLeg.Get());
            writer.Write(LeftLowerArm.Get());
            writer.Write(RightLowerArm.Get());
            writer.Write(DismemberedLeftLowerArm.Get());
            writer.Write(DismemberedRightLowerArm.Get());
            writer.Write(DismemberedLeftLowerLeg.Get());
            writer.Write(DismemberedRightLowerLeg.Get());
            writer.Write(DismemberedLeftUpperLeg.Get());
            writer.Write(CrippledLeftLeg.Get());
        }
    }
}