

using SevenDaysSaveManipulator.GameData;
using System.Collections.Generic;

namespace SevenDaysProfileEditor.Inventory
{
    class DataItem
    {
        public static List<DataItem> itemList = new List<DataItem>();
        public static List<DataItem> devItems = new List<DataItem>();
        public static string[] nameList;

        public const int MAX_QUALITY = 600;
        public const int DEFAULT_STACKNUMBER = 500;

        public int id;
        public string name;
        public int stackNumber = 0;

        public bool isBlock = false;
        public bool hasQuality = false;
        public int degradationMin = 0;
        public int degradationMax = 0;
        public int magazineSize = 0;
        public string partType = "";
        public string[] attachmentNames;
        public string[] partNames;
        public string[] magazineItems;

        public bool isDeveloper = false;

        public void copy(DataItem itemData)
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
        }

        public static DataItem getItemDataById(int id)
        {
            foreach (DataItem itemData in itemList)
            {
                if (itemData.id == id)
                {
                    return itemData;
                }
            }

            return null;
        }

        public static DataItem getItemDataByName(string name)
        {
            foreach (DataItem itemData in itemList)
            {
                if (itemData.name.Equals(name))
                {
                    return itemData;
                }
            }

            return null;
        }

        public static DataItem getItemDataByItemValue(ItemValue itemValue)
        {
            return getItemDataById(itemValue.type.get());
        }

        public static string[] getNameList()
        {
            string[] list = new string[itemList.Count];

            nameList.CopyTo(list, 0);

            return list;
        }

        public static DataItem getDevItemById(int id)
        {
            foreach (DataItem itemData in devItems)
            {
                if (itemData.id == id)
                {
                    return itemData;
                }
            }

            return null;
        }
    }
}
