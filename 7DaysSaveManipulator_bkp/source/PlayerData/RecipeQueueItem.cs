using System;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class RecipeQueueItem {

        //AmountToRepair
        public Value<int> amountToRepair = new Value<int>(0);

        //CraftingTimeLeft
        public Value<float> craftingTimeLeft = new Value<float>(0);

        //IsCrafting
        public Value<bool> isCrafting;

        //Multiplier
        public Value<int> multiplier;

        //Quality
        public Value<int> quality = new Value<int>(0);

        //Recipe
        public Recipe recipe;

        //RepairItem
        public ItemValue repairItem;

        //StartingEntityID
        public Value<int> startingEntityId = new Value<int>(-1);

        public void Read(BinaryReader reader) {
            //flag
            bool isRecipeNotNull = reader.ReadBoolean();
            if (isRecipeNotNull) {
                recipe = new Recipe();
                recipe.Read(reader);
            }

            craftingTimeLeft = new Value<float>(reader.ReadSingle());
            multiplier = new Value<int>(reader.ReadInt32());
            isCrafting = new Value<bool>(reader.ReadBoolean());

            //flag2
            bool isRepairItemNotNull = reader.ReadBoolean();
            if (isRepairItemNotNull) {
                repairItem = new ItemValue();
                repairItem.Read(reader);
                amountToRepair = new Value<int>(reader.ReadInt32());
            }

            quality = new Value<int>(reader.ReadInt32());
            startingEntityId = new Value<int>(reader.ReadInt32());
        }

        public void Write(BinaryWriter writer) {
            bool recipeNotNull = recipe != null;
            writer.Write(recipeNotNull);
            if (recipeNotNull) {
                recipe.Write(writer);
            }

            writer.Write(craftingTimeLeft.Get());
            writer.Write(multiplier.Get());
            writer.Write(isCrafting.Get());

            bool repairItemNotNull = repairItem != null;
            writer.Write(repairItemNotNull);
            if (repairItemNotNull) {
                repairItem.Write(writer);
                writer.Write(amountToRepair.Get());
            }

            writer.Write(quality.Get());
            writer.Write(startingEntityId.Get());
        }
    }
}