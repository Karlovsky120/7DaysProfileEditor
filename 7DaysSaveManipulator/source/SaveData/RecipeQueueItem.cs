using System;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    public class RecipeQueueItem {

        //AmountToRepair
        public Value<ushort> amountToRepair;

        //CraftingTimeLeft
        public Value<float> craftingTimeLeft;

        //IsCrafting
        public Value<bool> isCrafting;

        //Multiplier
        public Value<short> multiplier;

        //Quality
        public Value<byte> quality;

        //recipeHashCode
        public Value<int> recipeHashCode;

        //RepairItem
        public ItemValue repairItem;

        //StartingEntityId
        public Value<int> startingEntityId;

        public RecipeQueueItem() {}

        internal RecipeQueueItem(TypedBinaryReader reader, XmlData xmlData) {
            Read(reader, xmlData);
        }

        internal void Read(TypedBinaryReader reader, XmlData xmlData) {
            recipeHashCode = new Value<int>(reader);
            multiplier = new Value<short>(reader);
            isCrafting = new Value<bool>(reader);
            craftingTimeLeft = new Value<float>(reader);

            if (reader.ReadBoolean()) {
                repairItem = new ItemValue(reader, xmlData);
                amountToRepair = new Value<ushort>(reader);
            }

            quality = new Value<byte>(reader);
            startingEntityId = new Value<int>(reader);
        }

        internal void Write(TypedBinaryWriter writer) {
            recipeHashCode.Write(writer);
            multiplier.Write(writer);
            isCrafting.Write(writer);
            craftingTimeLeft.Write(writer);

            writer.Write(repairItem != null);
            if (repairItem != null) {
                repairItem.Write(writer);
                amountToRepair.Write(writer);
            }

            quality.Write(writer);
            startingEntityId.Write(writer);
        }
    }
}