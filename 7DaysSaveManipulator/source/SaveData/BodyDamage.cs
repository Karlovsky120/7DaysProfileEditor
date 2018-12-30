using System;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    public class BodyDamage {

        //Chest  
        public Value<short> chest;

        //CrippledLeftLeg
        public Value<bool> crippledLeftLeg;

        //CrippledRightLeg
        public Value<bool> crippledRightLeg;

        //DismemberedHead
        public Value<bool> dismemberedHead;

        //DismemberedLeftLowerArm
        public Value<bool> dismemberedLeftLowerArm;

        //DismemberedLeftLowerLeg
        public Value<bool> dismemberedLeftLowerLeg;

        //DismemberedLeftUpperArm
        public Value<bool> dismemberedLeftUpperArm;

        //DismemberedLeftUpperLeg
        public Value<bool> dismemberedLeftUpperLeg;

        //DismemberedRightLowerArm
        public Value<bool> dismemberedRightLowerArm;

        //DismemberedRightLowerLeg
        public Value<bool> dismemberedRightLowerLeg;

        //DismemberedRightUpperArm
        public Value<bool> dismemberedRightUpperArm;

        //DismemberedRightUpperLeg
        public Value<bool> dismemberedRightUpperLeg;

        //Head
        public Value<short> head;

        //LeftLowerArm
        public Value<short> leftLowerArm;

        //LeftLowerLeg
        public Value<short> leftLowerLeg;

        //LeftUpperArm
        public Value<short> leftUpperArm;

        //LeftUpperLeg
        public Value<short> leftUpperLeg;

        //RightLowerArm
        public Value<short> rightLowerArm;

        //RightLowerLeg
        public Value<short> rightLowerLeg;

        //RightUpperArm
        public Value<short> rightUpperArm;

        //RightUpperLeg
        public Value<short> rightUpperLeg;
 
        public BodyDamage() {}

        internal BodyDamage(TypedBinaryReader reader) {
            Read(reader);
        }

        internal void Read(TypedBinaryReader reader) {
            Utils.VerifyVersion(reader.ReadInt32(), SaveVersionConstants.BODY_DAMAGE);

            leftUpperLeg = new Value<short>(reader);
            rightUpperLeg = new Value<short>(reader);
            leftUpperArm = new Value<short>(reader);
            rightUpperArm = new Value<short>(reader);
            chest = new Value<short>(reader);
            head = new Value<short>(reader);
            dismemberedLeftUpperArm = new Value<bool>(reader);
            dismemberedRightUpperArm = new Value<bool>(reader);
            dismemberedHead = new Value<bool>(reader);
            dismemberedRightUpperLeg = new Value<bool>(reader);
            crippledRightLeg = new Value<bool>(reader);

            leftLowerLeg = new Value<short>(reader);
            rightLowerLeg = new Value<short>(reader);
            leftLowerArm = new Value<short>(reader);
            rightLowerArm = new Value<short>(reader);
            dismemberedLeftLowerArm = new Value<bool>(reader);
            dismemberedRightLowerArm = new Value<bool>(reader);
            dismemberedLeftLowerLeg = new Value<bool>(reader);
            dismemberedRightLowerLeg = new Value<bool>(reader);

            dismemberedLeftUpperLeg = new Value<bool>(reader);
            crippledLeftLeg = new Value<bool>(reader);
        }

        internal void Write(TypedBinaryWriter writer) {
            writer.Write(SaveVersionConstants.BODY_DAMAGE);
            leftUpperLeg.Write(writer);
            rightUpperLeg.Write(writer);
            leftUpperArm.Write(writer);
            rightUpperArm.Write(writer);

            chest.Write(writer);
            head.Write(writer);
            dismemberedLeftUpperArm.Write(writer);
            dismemberedRightUpperArm.Write(writer);
            dismemberedHead.Write(writer);
            dismemberedRightUpperLeg.Write(writer);

            crippledRightLeg.Write(writer);
            leftLowerLeg.Write(writer);
            rightLowerLeg.Write(writer);
            leftLowerArm.Write(writer);
            rightLowerArm.Write(writer);
            dismemberedLeftLowerArm.Write(writer);
            dismemberedRightLowerArm.Write(writer);
            dismemberedLeftLowerLeg.Write(writer);
            dismemberedRightLowerLeg.Write(writer);
            dismemberedLeftUpperLeg.Write(writer);
            crippledLeftLeg.Write(writer);
        }
    }
}