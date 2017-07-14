using System;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class CraftingData {

        //RecipeQueueItems
        public RecipeQueueItem[] recipeQueueItems;

        public void Read(BinaryReader reader) {
            //num
            int recipeQueueItemsLength = (int)reader.ReadByte();
            recipeQueueItems = new RecipeQueueItem[recipeQueueItemsLength];
            for (int i = 0; i < recipeQueueItemsLength; i++) {
                recipeQueueItems[i] = new RecipeQueueItem();
                recipeQueueItems[i].Read(reader);
            }
        }

        public void Write(BinaryWriter writer) {
            int num = recipeQueueItems.Length;
            writer.Write((byte)num);
            for (int i = 0; i < num; i++) {
                if (recipeQueueItems[i] == null) {
                    recipeQueueItems[i] = new RecipeQueueItem();
                }
                recipeQueueItems[i].Write(writer);
            }
        }
    }
}