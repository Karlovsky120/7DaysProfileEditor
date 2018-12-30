using System;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    class JournalEntryTip : JournalEntry {

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
            meleeToolWireToolTip,
            generatorBankTip,
            solarBankTip,
            batteryBankTip,
            paintingTip,
            cameraTip,
            triggerDelayDurationTip,
            landClaimTip,
            passthroughTriggeringTip,
            quest_WhiteRiverCitizen1_description,
            onFireTip,
            tableSawTip,
            questTip,
            powerAttackTip,
            survivalTip
        }

        //Stored in ID as string
        public Value<JournalTipEntry> tipType;

        //read
        public Value<bool> read;

        //timestamp
        public Value<ulong> timestamp;

        public JournalEntryTip() {}

        internal JournalEntryTip(TypedBinaryReader reader) {
            Read(reader);
        }

        internal override void Read(TypedBinaryReader reader) {
            base.Read(reader);

            tipType = new Value<JournalTipEntry>((JournalTipEntry)reader.ReadByte());
            read = new Value<bool>(reader);
            timestamp = new Value<ulong>(reader);
        }

        internal override void Write(TypedBinaryWriter writer) {
            base.Write(writer);

            writer.Write((byte)tipType.Get());
            read.Write(writer);
            timestamp.Write(writer);
        }
    }
}
