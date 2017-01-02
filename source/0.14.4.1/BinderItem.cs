using SevenDaysSaveManipulator.GameData;
using System.Collections.Generic;

namespace SevenDaysProfileEditor.Inventory
{
    class BinderItem
    {
        public ItemStack itemStack;
        public ItemValue itemValue;
        public DataItem itemData;

        public Value<int> count;

        public Value<int> type;
        public Value<int> quality;
        public Value<int> useTimes;
        public Value<int> meta;
        public Value<byte> selectedAmmoTypeIndex;
        public Value<bool> activated;
        public ItemValue[] parts;
        public List<ItemValue> attachments;

        public string name;
        public int id;
        public int stackNumber;

        public bool isBlock;
        public bool hasQuality;
        public int degradationMin;
        public int degradationMax;
        public int magazineSize;
        public string partType;
        public string[] attachmentNames;
        public string[] partNames;
        public string[] magazineItems;

        public bool isDeveloper;

        private ItemStack createItemStack(DataItem itemData, ItemValue itemValue)
        {
            ItemStack itemStack = new ItemStack();
            itemStack.count = new Value<int>(itemData.stackNumber);
            itemStack.itemValue = itemValue;

            return itemStack;
        }

        private void populateItemValue(ItemValue itemValue, DataItem itemData)
        {
            itemValue.itemValueVersion = new Value<byte>(3);
            itemValue.type = new Value<int>(itemData.id);
            itemValue.useTimes = new Value<int>(0);
            itemValue.quality = new Value<int>(0);
            itemValue.meta = new Value<int>(0);
            itemValue.selectedAmmoTypeIndex = new Value<byte>(0);
            itemValue.activated = new Value<bool>(false);
            itemValue.parts = new ItemValue[0];
            itemValue.attachments = new List<ItemValue>();

            if (itemData.name != "air")
            {
                if (itemData.hasQuality)
                {
                    itemValue.quality.set(DataItem.MAX_QUALITY);

                    if (itemData.partNames != null)
                    {
                        itemValue.parts = new ItemValue[4];

                        int quality = 0;

                        for (int i = 0; i < 4; i++)
                        {
                            itemValue.parts[i] = new ItemValue();
                            populateItemValue(itemValue.parts[i], DataItem.getItemDataByName(itemData.partNames[i]));
                            quality += itemValue.parts[i].quality.get();
                        }

                        itemValue.quality.set(quality / 4);
                    }
                }
            }
        }

        private void bindItemStack(ItemStack itemStack)
        {
            this.itemStack = itemStack;

            count = itemStack.count;
        }

        private void bindItemValue(ItemValue itemValue)
        {
            this.itemValue = itemValue;

            type = itemValue.type;
            quality = itemValue.quality;
            useTimes = itemValue.useTimes;
            meta = itemValue.meta;
            selectedAmmoTypeIndex = itemValue.selectedAmmoTypeIndex;
            activated = itemValue.activated;
            parts = itemValue.parts;
            attachments = itemValue.attachments;
        }

        private void bindItemData(DataItem itemData)
        {
            this.itemData = itemData;

            name = itemData.name;
            id = itemData.id;
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
            isDeveloper = itemData.isDeveloper;
        }

        public bool hasAllParts()
        {
            if (parts.Length > 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (parts[i].type.get() == 0)
                    {
                        return false;
                    }
                }

                return true; 
            }

            return false;
        }

        public int getQuality()
        {
            if (hasQuality)
            {
                if (parts.Length > 0)
                {
                    if (hasAllParts())
                    {
                        int itemQuality = 0;

                        for (int i = 0; i < 4; i++)
                        {                            
                            itemQuality += parts[i].quality.get();
                        }

                        return itemQuality / 4;
                    }

                    else
                    {
                        return 0;
                    }
                }

                else
                {
                    return quality.get();
                }
            }

            return 0;
        }

        public int getDegradation()
        {
            return (int)(((degradationMax - degradationMin) / (float)DataItem.MAX_QUALITY) * quality.get() + degradationMin);
        }

        public void update(int id)
        {
            DataItem itemData = DataItem.getItemDataById(id);

            bindItemData(itemData);
            populateItemValue(itemValue, itemData);
            bindItemValue(itemValue);
            bindItemStack(createItemStack(itemData, itemValue));           
        }

        public BinderItem(ItemStack itemStack)
        {
            bindItemData(DataItem.getItemDataByItemValue(itemStack.itemValue));
            bindItemStack(itemStack);
            bindItemValue(itemStack.itemValue);    
        }

        public BinderItem(ItemValue itemValue)
        {
            bindItemData(DataItem.getItemDataByItemValue(itemValue));

            itemStack = createItemStack(DataItem.getItemDataByItemValue(itemValue), itemValue);
            bindItemValue(itemValue);            
        }      

        public BinderItem(DataItem itemData)
        {
            bindItemData(itemData);

            itemValue = new ItemValue();
            populateItemValue(itemValue, itemData);
            itemStack = createItemStack(itemData, itemValue);
        }
    }
}
