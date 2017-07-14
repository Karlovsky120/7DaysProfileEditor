using System;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class JournalEntry {

        //CurrentFileVersion
        public Value<byte> currentFileVersion;

        //EntryType
        public Value<JournalEntryTypes> entryType;

        //ID
        public string id;

        //read
        public Value<bool> read;

        //timestamp
        public Value<ulong> timeStamp;

        public enum JournalEntryTypes {
            Invalid,
            Tip,
            Death,
            Level,
            Friend
        }

        public enum JournalTipEntry {
            bedrollTip,
            harvestTip,
            waterTip,
            airDropTip,
            cementMixerTip,
            workbenchTip,
            campfireTip,
            forgeTip,
            chemistryStationTip,
            tutorialTipQuest01,
            tutorialTipQuest02,
            skillPointTip,
            farmingTip,
            firstAidTip,
            augmentGunsTip,
            gunAssemblyTip,
            alternateAmmoTip,
            hotWeatherTip,
            coldWeatherTip,
            traderTip,
            treasureMapTip,
            vendingMachineTip,
            miningTip,
            wireToolTip,
            generatorBankTip,
            solarBankTip,
            batteryBankTip,
            paintingTip,
            cameraTip,
            triggerDelayDurationTip,
            landClaimTip,
            passthroughTriggeringTip
        }

        public void Read(BinaryReader reader) {
            entryType = new Value<JournalEntryTypes>((JournalEntryTypes)reader.ReadByte());
            currentFileVersion = new Value<byte>(reader.ReadByte());
            
            if (entryType.Get() == JournalEntryTypes.Tip) {
                if (currentFileVersion.Get() == 1)
                    throw new Exception();

                byte b = reader.ReadByte();
                id = ((JournalTipEntry)b).ToString();
            }
            else {
                id = reader.ReadString();
            }
            read = new Value<bool>(reader.ReadBoolean());
            timeStamp = new Value<ulong>(reader.ReadUInt64());
        }

        public void Write(BinaryWriter writer) {
            writer.Write((byte)entryType.Get());
            writer.Write(currentFileVersion.Get());

            if (entryType.Get() == JournalEntryTypes.Tip) {
                JournalTipEntry journalTipEntry = (JournalTipEntry)((int)Enum.Parse(typeof(JournalTipEntry), id));
                writer.Write((byte)journalTipEntry);
            }
            else {
                writer.Write(id);
            }
            writer.Write(read.Get());
            writer.Write(timeStamp.Get());
        }
    }
}