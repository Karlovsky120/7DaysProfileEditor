using SevenDaysSaveManipulator.GameData;
using System.Collections.Generic;

namespace SevenDaysProfileEditor.Inventory
{
    public class ItemData
    {
        public const int DEFAULT_STACKNUMBER = 500;
        public const int MAX_QUALITY = 600;
        public static List<ItemData> devItems = new List<ItemData>();
        public static List<ItemData> itemList = new List<ItemData>(); //TODO dictionary
        public static string[] nameList;

        public static List<string> schematicRecipeList = new List<string>();
        public string[] attachmentNames;
        public int degradationMax = 0;
        public int degradationMin = 0;
        public bool hasQuality = false;
        public byte[] iconPixels;
        public int id;
        public bool isBlock = false;
        public bool isDeveloper = false;
        public string[] magazineItems;
        public int magazineSize = 0;
        public string name;
        public string[] partNames;
        public string partType = "";
        public int stackNumber = 0;

        public static ItemData GetDevItemById(int id)
        {
            foreach (ItemData itemData in devItems)
            {
                if (itemData.id == id)
                {
                    return itemData;
                }
            }

            return null;
        }

        public static ItemData GetItemDataById(int id)
        {
            foreach (ItemData itemData in itemList)
            {
                if (itemData.id == id)
                {
                    return itemData;
                }
            }

            return null;
        }

        public static ItemData GetItemDataByItemValue(ItemValue itemValue)
        {
            return GetItemDataById(itemValue.type.Get());
        }

        public static ItemData GetItemDataByName(string name)
        {
            foreach (ItemData itemData in itemList)
            {
                if (itemData.name.Equals(name))
                {
                    return itemData;
                }
            }

            return null;
        }

        public static string[] GetNameList()
        {
            string[] list = new string[itemList.Count];

            nameList.CopyTo(list, 0);

            return list;
        }

        public void Copy(ItemData itemData)
        {
            id = itemData.id;
            name = itemData.name;
            stackNumber = itemData.stackNumber;

            isBlock = itemData.isBlock;
            hasQuality = itemData.hasQuality;
            degradationMin = itemData.degradationMin;
            degradationMax = itemData.degradationMax;
            magazineSize = itemData.magazineSize;
            partType = itemData.partType;
            attachmentNames = itemData.attachmentNames;
            partNames = itemData.partNames;
            magazineItems = itemData.magazineItems;
            iconPixels = itemData.iconPixels;
        }
    }
}