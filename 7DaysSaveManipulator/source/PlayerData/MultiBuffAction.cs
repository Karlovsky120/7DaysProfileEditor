﻿using System;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class MultiBuffAction {

        //num = 3
        public static Value<int> multiBuffActionVersion;

        //G
        public EnumBuffCategoryFlags categoryFlags;

        //Q
        public EnumCommand command;

        //E
        public Value<string> context;

        //C
        public Value<float> unknownC;

        //F
        public Value<bool> unknownF;

        //I
        public Value<bool> unknownI;

        //J
        public BuffTimer unknownJ;

        //O
        public Value<string> unknownO;

        //S
        public BuffTimer unknownS;

        //V
        public Value<int> unknownV;

        //W
        public Value<string> unknownW;

        public enum EnumCommand {
            Damage,
            Kill,
            Debuff,
            AddImmunity,
            SetStatValue,
            ResetStat,
            AttachPrefab,
            Increment,
            Min,
            Max,
            SetVar,
            Buff
        }

        public static MultiBuffAction Read(BinaryReader reader) {
            multiBuffActionVersion = new Value<int>(reader.ReadInt32());

            MultiBuffAction multiBuffAction = new MultiBuffAction {
                command = (EnumCommand)reader.ReadByte(),
                unknownC = new Value<float>(reader.ReadSingle()),
                categoryFlags = (EnumBuffCategoryFlags)reader.ReadInt32(),
                unknownV = new Value<int>(reader.ReadInt32()),
                unknownO = new Value<string>(reader.ReadString()),
                unknownW = new Value<string>(reader.ReadString()),
                context = new Value<string>(reader.ReadString()),

                unknownJ = BuffTimer.Read(reader),
                unknownS = BuffTimer.Read(reader),

                unknownF = new Value<bool>(reader.ReadBoolean()),
                unknownI = new Value<bool>(reader.ReadBoolean())
            };

            return multiBuffAction;
        }

        public void Write(BinaryWriter writer) {
            writer.Write(multiBuffActionVersion.Get());
            writer.Write((byte)command);
            writer.Write(unknownC.Get());
            writer.Write((int)categoryFlags);
            writer.Write(unknownV.Get());
            writer.Write(unknownO.Get());
            writer.Write(unknownW.Get());
            writer.Write(context.Get());

            unknownJ.Write(writer);
            unknownS.Write(writer);

            writer.Write(unknownF.Get());
            writer.Write(unknownI.Get());
        }
    }
}