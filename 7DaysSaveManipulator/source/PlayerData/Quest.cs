using System;
using System.Collections.Generic;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public enum QuestState {
        InProgress,
        Completed,
        Failed
    }

    [Serializable]
    public class Quest {

        //CurrentFileVersion
        public Value<byte> currentFileVersion;

        //CurrentQuestVersion
        public Value<byte> currentQuestVersion;

        //CurrentState
        public Value<QuestState> currentState;

        //DataVariables
        public Dictionary<string, string> dataVariables = new Dictionary<string, string>();

        //FinishTime
        public Value<ulong> finishTime;

        //ID
        public string id;

        //Q
        public Value<bool> isTracked;

        //Objectives
        public List<BaseObjective> objectives = new List<BaseObjective>();

        //OwnerJournal
        public QuestJournal ownerJournal;

        public void Read(BinaryReader reader) {
            currentFileVersion = new Value<byte>(reader.ReadByte());
            currentState = new Value<QuestState>((QuestState)reader.ReadByte());
            isTracked = new Value<bool>(reader.ReadBoolean());
            finishTime = new Value<ulong>(reader.ReadUInt64());

            //num
            int objectiveCount = reader.ReadByte();
            for (int i = 0; i < objectiveCount; i++) {
                BaseObjective baseObjective = new BaseObjective();
                baseObjective.Read(reader);
                objectives.Add(baseObjective);
            }

            //num2
            int dataVariableCount = reader.ReadByte();
            for (int j = 0; j < dataVariableCount; j++) {
                string key = reader.ReadString();
                string value = reader.ReadString();
                dataVariables.Add(key, value);
            }
        }

        public void Write(BinaryWriter writer) {
            writer.Write(id);
            writer.Write(currentQuestVersion.Get());
            writer.Write(currentFileVersion.Get());
            writer.Write((byte)currentState.Get());
            writer.Write(isTracked.Get());
            writer.Write(finishTime.Get());
            writer.Write((byte)objectives.Count);
            for (int i = 0; i < objectives.Count; i++) {
                objectives[i].Write(writer);
            }

            writer.Write((byte)dataVariables.Count);
            foreach (string current in dataVariables.Keys) {
                writer.Write(current);
                writer.Write(dataVariables[current]);
            }
        }
    }
}