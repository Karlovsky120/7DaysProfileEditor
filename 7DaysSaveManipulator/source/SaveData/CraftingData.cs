using System;
using System.Collections.Generic;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    public class CraftingData {

        //RecipeQueueItems
        public List<RecipeQueueItem> recipeQueueItems;

        public CraftingData() {}

        internal CraftingData(TypedBinaryReader reader, XmlData xmlData) {
            Read(reader, xmlData);
        }

        internal void Read(TypedBinaryReader reader, XmlData xmlData) {
            byte recipeQueueItemsLength = reader.ReadByte();
            recipeQueueItems = new List<RecipeQueueItem>(recipeQueueItemsLength);
            for (byte i = 0; i < recipeQueueItemsLength; ++i) {
                recipeQueueItems.Add(new RecipeQueueItem(reader, xmlData));
            }
        }

        internal void Write(TypedBinaryWriter writer) {
            byte recipeQueueItemsLength = (byte)recipeQueueItems.Count;
            writer.Write(recipeQueueItemsLength);
            for (byte i = 0; i < recipeQueueItemsLength; ++i) {
                if (recipeQueueItems[i] == null) {
                    recipeQueueItems[i] = new RecipeQueueItem();
                }
                recipeQueueItems[i].Write(writer);
            }
        }
    }
}