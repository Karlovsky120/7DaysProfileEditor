using System;
using System.IO;

namespace SevenDaysSaveManipulator.GameData {

    [Serializable]
    public class Equipment {

        //notSaved = 1
        public static Value<byte> equipmentVersion;

        //S
        public ItemValue[] slots = new ItemValue[32];

        public static Equipment Read(BinaryReader reader) {
            equipmentVersion = new Value<byte>(reader.ReadByte());

            Equipment equipment = new Equipment();
            for (int i = 0; i < equipment.slots.Length; i++) {
                equipment.slots[i] = new ItemValue();
                equipment.slots[i].Read(reader);
            }
            return equipment;
        }

        public void Write(BinaryWriter writer) {
            writer.Write(equipmentVersion.Get());
            for (int i = 0; i < slots.Length; i++) {
                slots[i].Write(writer);
            }
        }
    }
}