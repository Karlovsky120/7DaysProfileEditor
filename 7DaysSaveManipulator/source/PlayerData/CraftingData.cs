using System;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class CraftingData {

        //RecipeQueueItems
        public RecipeQueueItem[] recipeQueueItems;

        public CraftingData() {}

        internal CraftingData(BinaryReader reader) {
            Read(reader);
        }

        internal void Read(BinaryReader reader) {
            byte recipeQueueItemsLength = reader.ReadByte();
            recipeQueueItems = new RecipeQueueItem[recipeQueueItemsLength];
            for (byte i = 0; i < recipeQueueItemsLength; ++i) {
                recipeQueueItems[i] = new RecipeQueueItem();
                recipeQueueItems[i].Read(reader);
            }
        }

        internal void Write(BinaryWriter writer) {
            int num = recipeQueueItems.Length;
            writer.Write((byte)num);
            for (int i = 0; i < num; ++i) {
                if (recipeQueueItems[i] == null) {
                    recipeQueueItems[i] = new RecipeQueueItem();
                }
                recipeQueueItems[i].Write(writer);
            }
        }
    }
}