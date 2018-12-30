using System;
using System.Collections.Generic;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    public class Quest {

        public enum PositionDataTypes {
            QuestGiver,
            Location,
            POIPosition,
            POISize,
            TreasurePoint,
            FetchContainer,
            HiddenCache,
            Activate
        }

        public enum QuestState {
            NotStarted,
            InProgress,
            ReadyForTurnIn,
            Completed,
            Failed
        }

        //CurrentState
        public Value<QuestState> currentState;

        //DataVariables
        public Dictionary<string, string> dataVariables;

        //FinishTime
        public Value<ulong> finishTime;

        //ID
        public Value<string> id;

        //tracked
        public Value<bool> tracked;

        //Objectives
        public List<BaseObjective> objectives;

        //OwnerJournal
        public QuestJournal ownerJournal;

        //SharedOwnerID
        public Value<int> sharedOwnerId;

        //QuestGiverID
        public Value<int> questGiverId;

        //CurrentPhase
        public Value<byte> currentPhase;

        //QuestCode
        public Value<int> questCode;

        //PositionData
        public Dictionary<PositionDataTypes, Vector3D<float>> positionData;

        //RallyMarkerActivated
        public Value<bool> rallyMarkerActivated;

        //Rewards
        public List<byte> rewardIndexes;

        private XmlData xmlData;

        public Quest() {}

        internal Quest(TypedBinaryReader reader, XmlData xmlData) {
            this.xmlData = xmlData;
            Read(reader);
        }

        internal void Read(TypedBinaryReader reader) {
            id = new Value<string>(reader);
            Utils.VerifyVersion(reader.ReadByte(), xmlData.GetCurrentQuestVersion(id.Get()));
            Utils.VerifyVersion(reader.ReadByte(), SaveVersionConstants.QUEST);

            currentState = new Value<QuestState>((QuestState)reader.ReadByte());
            sharedOwnerId = new Value<int>(reader);
            questGiverId = new Value<int>(reader);

            if (currentState.Get() == QuestState.InProgress) {
                tracked = new Value<bool>(reader);
                currentPhase = new Value<byte>(reader);
                questCode = new Value<int>(reader);
            }

            int objectiveCount = xmlData.GetObjectiveCount(id.Get());
            objectives = new List<BaseObjective>(objectiveCount);
            for (int i = 0; i < objectiveCount; ++i) {
                objectives.Add(new BaseObjective(reader));
            }

            byte dataVariableCount = reader.ReadByte();
            dataVariables = new Dictionary<string, string>();
            for (byte i = 0; i < dataVariableCount; ++i) {
                string key = reader.ReadString();
                string value = reader.ReadString();

                if (!dataVariables.ContainsKey(key)) {
                    dataVariables.Add(key, value);
                } else {
                    dataVariables[key] = value;
                }
            }

            if (currentState.Get() == QuestState.InProgress) {
                byte positionDataLength = reader.ReadByte();
                positionData = new Dictionary<PositionDataTypes, Vector3D<float>>();
                for (int i = 0; i < positionDataLength; ++i) {

                    PositionDataTypes key = (PositionDataTypes)reader.ReadByte();
                    Vector3D<float> value = new Vector3D<float>(reader);

                    if (!positionData.ContainsKey(key)) {
                        positionData.Add(key, value);
                    } else {
                        positionData[key] = value;
                    }
                }

                rallyMarkerActivated = new Value<bool>(reader);
            } else {
                finishTime = new Value<ulong>(reader);
            }

            if (currentState.Get() == QuestState.InProgress || currentState.Get() == QuestState.ReadyForTurnIn) {
                int rewardsCount = xmlData.GetRewardCount(id.Get());
                rewardIndexes = new List<byte>(rewardsCount);
                for (int i = 0; i < rewardsCount; ++i) {
                    rewardIndexes.Add(reader.ReadByte());
                }
            }
        }

        internal void Write(TypedBinaryWriter writer) {
            id.Write(writer);
            writer.Write((byte)xmlData.GetCurrentQuestVersion(id.Get()));
            writer.Write(SaveVersionConstants.QUEST);

            writer.Write((byte)currentState.Get());
            sharedOwnerId.Write(writer);
            questGiverId.Write(writer);

            if (currentState.Get() == QuestState.InProgress) {
                tracked.Write(writer);
                currentPhase.Write(writer);
                questCode.Write(writer);
            }

            foreach (BaseObjective objective in objectives) {
                objective.Write(writer);
            }

            writer.Write((byte)dataVariables.Count);
            foreach (KeyValuePair<string, string> current in dataVariables) {
                writer.Write(current.Key);
                writer.Write(current.Value);
            }

            if (currentState.Get() == QuestState.InProgress) {
                writer.Write((byte)positionData.Count);
                foreach(KeyValuePair<PositionDataTypes, Vector3D<float>> current in positionData) {
                    writer.Write((byte)current.Key);
                    current.Value.Write(writer);
                }

                rallyMarkerActivated.Write(writer);
            } else {
                finishTime.Write(writer);
            }

            if (currentState.Get() == QuestState.InProgress || currentState.Get() == QuestState.ReadyForTurnIn) {
                foreach (byte rewardIndex in rewardIndexes) {
                    writer.Write(rewardIndex);
                }
            }
        }
    }
}