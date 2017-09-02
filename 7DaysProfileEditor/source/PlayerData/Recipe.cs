using System;
using System.Collections.Generic;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class Recipe {

        //count
        public Value<int> count;

        //craftingArea
        public Value<string> craftingArea;

        //craftingExpGain
        public Value<int> craftingExpGain;

        //craftingTime
        public Value<float> craftingTime;

        //text
        public Value<string> craftingToolTypeName;

        //ingredients, might be obsolete
        public List<ItemStack> ingredients;

        //GetName()
        public Value<string> itemName;

        //materialBasedRecipe
        public Value<bool> materialBasedRecipe;

        //custom, replaces ingredients
        public List<Tuple<string, int>> nameStackSizeList;

        //Q = 2
        public Value<byte> recipeVersion;

        //scrapable
        public Value<bool> scrapable;

        //tooltip
        public Value<string> tooltip;

        //wildcardCampfireCategory
        public Value<bool> wildcardCampfireCategory;

        //wildcardForgeCategory
        public Value<bool> wildcardForgeCategory;

        public void Read(BinaryReader reader) {
            recipeVersion = new Value<byte>(reader.ReadByte());

            itemName = new Value<string>(reader.ReadString());
            count = new Value<int>(reader.ReadInt32());
            scrapable = new Value<bool>(reader.ReadBoolean());
            wildcardForgeCategory = new Value<bool>(reader.ReadBoolean());
            wildcardCampfireCategory = new Value<bool>(reader.ReadBoolean());

            craftingToolTypeName = new Value<string>(reader.ReadString());
            craftingTime = new Value<float>(reader.ReadSingle());
            craftingArea = new Value<string>(reader.ReadString());
            tooltip = new Value<string>(reader.ReadString());

            //num
            int ingredientCount = reader.ReadInt32();
            ingredients = new List<ItemStack>();
            nameStackSizeList = new List<Tuple<string, int>>();
            for (int i = 0; i < ingredientCount; i++) {
                string name = reader.ReadString();
                int stackSize = reader.ReadInt32();

                int zero1 = reader.ReadInt32();//0
                int zero2 = reader.ReadInt32();//0

                nameStackSizeList.Add(new Tuple<string, int>(name, stackSize));
            }

            materialBasedRecipe = new Value<bool>(reader.ReadBoolean());
            craftingExpGain = new Value<int>(reader.ReadInt32());
        }

        public void Write(BinaryWriter writer) {
            writer.Write(recipeVersion.Get());
            writer.Write(itemName.Get());
            writer.Write(count.Get());
            writer.Write(scrapable.Get());
            writer.Write(wildcardForgeCategory.Get());
            writer.Write(wildcardCampfireCategory.Get());

            writer.Write(craftingToolTypeName.Get());
            writer.Write(craftingTime.Get());
            writer.Write(craftingArea.Get());
            writer.Write(tooltip.Get());

            writer.Write(nameStackSizeList.Count);
            foreach (Tuple<string, int> stack in nameStackSizeList) {
                writer.Write(stack.Item1);
                writer.Write(stack.Item2);

                writer.Write(0);
                writer.Write(0);
            }

            writer.Write(materialBasedRecipe.Get());
            writer.Write(craftingExpGain.Get());
        }
    }
}