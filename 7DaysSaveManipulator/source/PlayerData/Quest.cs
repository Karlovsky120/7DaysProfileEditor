using SevenDaysSaveManipulator.source.PlayerData;
using SevenDaysXMLParser.Quests;
using System;
using System.Collections.Generic;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class Quest {

        //CurrentState
        public Value<QuestState> currentState;

        //DataVariables
        public Dictionary<string, string> dataVariables = new Dictionary<string, string>();

        //FinishTime
        public Value<ulong> finishTime;

        //ID
        public string id;

        //tracked
        public Value<bool> tracked;

        //Objectives
        public List<BaseObjective> objectives = new List<BaseObjective>();

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
        public Dictionary<PositionDataTypes, Vector3D<float>> positionData = new Dictionary<PositionDataTypes, Vector3D<float>>();

        //RallyMarkerActivated
        public Value<bool> rallyMarkerActivated;

        //Rewards
        public List<byte> rewardIndexes;

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

        public Quest() {}

        internal Quest(BinaryReader reader, QuestsXml questsXml) {
            Read(reader, questsXml);
        }

        internal void Read(BinaryReader reader, QuestsXml questsXml) {
            id = reader.ReadString();
            Utils.VerifyVersion(reader.ReadByte(), SaveVersionConstants.QUEST_QUEST);
            Utils.VerifyVersion(reader.ReadByte(), SaveVersionConstants.QUEST_FILE);

            currentState = new Value<QuestState>((QuestState)reader.ReadByte());
            sharedOwnerId = new Value<int>(reader.ReadInt32());
            questGiverId = new Value<int>(reader.ReadInt32());

            if (currentState.Get() == QuestState.InProgress) {
                tracked = new Value<bool>(reader.ReadBoolean());
                currentPhase = new Value<byte>(reader.ReadByte());
                questCode = new Value<int>(reader.ReadInt32());
            }

            int objectiveCount = questsXml.quests[id].objectives.Count;
            for (int i = 0; i < objectiveCount; ++i) {
                objectives.Add(new BaseObjective(reader));
            }

            //num2
            byte dataVariableCount = reader.ReadByte();
            for (byte i = 0; i < dataVariableCount; i++) {
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
                for (int i = 0; i < positionDataLength; ++i) {
                    PositionDataTypes key = (PositionDataTypes)reader.ReadByte();
                    Vector3D<float> value = new Vector3D<float> {
                        x = new Value<float>(reader.ReadSingle()),
                        y = new Value<float>(reader.ReadSingle()),
                        z = new Value<float>(reader.ReadSingle())
                    };

                    if (!positionData.ContainsKey(key)) {
                        positionData.Add(key, value);
                    } else {
                        positionData[key] = value;
                    }
                }

                rallyMarkerActivated = new Value<bool>(reader.ReadBoolean());
            } else {
                finishTime = new Value<ulong>(reader.ReadUInt64());
            }

            if (currentState.Get() == QuestState.InProgress || currentState.Get() == QuestState.ReadyForTurnIn) {
                int rewardsCount = questsXml.quests[id].rewards.Count;
                for (int i = 0; i < rewardsCount; ++i) {
                    rewardIndexes.Add(reader.ReadByte());
                }
            }
        }

        internal void Write(BinaryWriter writer) {
            writer.Write(id);
            writer.Write(SaveVersionConstants.QUEST_QUEST); //???
            writer.Write(SaveVersionConstants.QUEST_FILE);
            writer.Write((byte)currentState.Get());
            writer.Write(sharedOwnerId.Get());
            writer.Write(questGiverId.Get());

            if (currentState.Get() == QuestState.InProgress) {
                writer.Write(tracked.Get());
                writer.Write(currentPhase.Get());
                writer.Write(questCode.Get());
            }

            for (int i = 0; i < objectives.Count; ++i) {
                objectives[i].Write(writer);
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
                    writer.Write(current.Value.x.Get());
                    writer.Write(current.Value.y.Get());
                    writer.Write(current.Value.z.Get());
                }
                writer.Write(rallyMarkerActivated.Get());
            } else {
                writer.Write(finishTime.Get());
            }

            if (currentState.Get() == QuestState.InProgress || currentState.Get() == QuestState.ReadyForTurnIn) {
                for (int i = 0; i < rewardIndexes.Count; ++i) {
                    writer.Write(rewardIndexes[i]);
                }
            }
        }
    }
}