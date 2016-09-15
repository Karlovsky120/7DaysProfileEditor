using SevenDaysProfileEditor.Inventory;
using SevenDaysProfileEditor.Quests;
using SevenDaysProfileEditor.Skills;
using System.Collections.Generic;
using System.Xml;

namespace SevenDaysProfileEditor.Data
{
    class XmlData
    {
        public static void GetBlocks()
        {
            List<ItemData> itemList = ItemData.itemList;
            XmlDocument document = new XmlDocument();
            document.Load(Config.GetSetting("gameRoot") + "\\Data\\Config\\blocks.xml");

            XmlNode blocksNode = document.DocumentElement;

            foreach (XmlNode blockNode in blocksNode.ChildNodes)
            {
                if (blockNode.Name.Equals("block"))
                {
                    XmlNode extendsNode = GetChildNodeByName(blockNode, "Extends");
                    XmlNode developerNode = GetChildNodeByName(blockNode, "IsDeveloper");

                    ItemData itemData = new ItemData();

                    if (extendsNode != null)
                    {
                        string parentItemName = extendsNode.Attributes[1].Value;
                        ItemData parent = ItemData.GetItemDataByName(parentItemName);
                        itemData.Copy(parent);
                    }

                    if (developerNode != null)
                    {
                        itemData.isDeveloper = developerNode.Attributes[1].Value.Equals("true");
                    }

                    itemData.id = int.Parse(blockNode.Attributes[0].Value);
                    itemData.name = blockNode.Attributes[1].Value;
                    itemData.stackNumber = ItemData.DEFAULT_STACKNUMBER;

                    if (IconData.itemIconDictionary.ContainsKey(itemData.name))
                    {
                        IconData.itemIconDictionary.TryGetValue(itemData.name, out itemData.iconPixels);
                    }

                    itemList.Add(itemData);
                }
            }
        }

        public static void GetItems()
        {
            List<ItemData> itemList = ItemData.itemList;
            XmlDocument document = new XmlDocument();
            document.Load(Config.GetSetting("gameRoot") + "\\Data\\Config\\items.xml");

            XmlNode itemsNode = document.DocumentElement;

            foreach (XmlNode itemNode in itemsNode.ChildNodes)
            {
                if (itemNode.Name.Equals("item"))
                {
                    XmlNode extendsNode = GetChildNodeByName(itemNode, "Extends");
                    XmlNode developerNode = GetChildNodeByName(itemNode, "IsDeveloper");
                    XmlNode stackNumberNode = GetChildNodeByName(itemNode, "Stacknumber");
                    XmlNode repairToolsNode = GetChildNodeByName(itemNode, "RepairTools");
                    XmlNode attributesNode = GetChildNodeByName(itemNode, "Attributes");
                    XmlNode degradationMaxNode = null;
                    if (attributesNode != null)
                    {
                        degradationMaxNode = GetChildNodeByName(attributesNode, "DegradationMax");
                    }

                    XmlNode partTypeNode = GetChildNodeByName(itemNode, "PartType");
                    XmlNode attachmentsNode = GetChildNodeByName(itemNode, "Attachments");
                    XmlNode partsNode = GetChildNodeByName(itemNode, "Parts");
                    XmlNode action0Node = GetChildNodeByName(itemNode, "Action0");
                    XmlNode magazineSizeNode = null;
                    if (action0Node != null)
                    {
                        magazineSizeNode = GetChildNodeByName(action0Node, "Magazine_size");
                    }

                    XmlNode customIconNode = GetChildNodeByName(itemNode, "CustomIcon");

                    ItemData itemData = new ItemData();

                    if (extendsNode != null)
                    {
                        string parentItemName = extendsNode.Attributes[1].Value;
                        ItemData parent = ItemData.GetItemDataByName(parentItemName);
                        itemData.Copy(parent);
                    }

                    if (developerNode != null)
                    {
                        itemData.isDeveloper = developerNode.Attributes[1].Value.Equals("true");
                    }

                    else
                    {
                        itemData.isDeveloper = false;
                    }

                    itemData.id = int.Parse(itemNode.Attributes[0].Value) + 4096;
                    itemData.name = itemNode.Attributes[1].Value;

                    if (stackNumberNode != null)
                    {
                        itemData.stackNumber = int.Parse(stackNumberNode.Attributes[1].Value);
                    }

                    else if (partsNode != null || attributesNode != null)
                    {
                        itemData.stackNumber = 1;
                    }

                    else if (itemData.stackNumber == 0)
                    {
                        itemData.stackNumber = ItemData.DEFAULT_STACKNUMBER;
                    }

                    if (repairToolsNode != null || partTypeNode != null)
                    {
                        string name = itemData.name;

                        itemData.hasQuality = true;

                        if (degradationMaxNode != null)
                        {
                            string degradationS = degradationMaxNode.Attributes[1].Value;

                            itemData.degradationMin = int.Parse(degradationS.Substring(0, degradationS.IndexOf(',')));
                            itemData.degradationMax = int.Parse(degradationS.Substring(degradationS.IndexOf(',') + 1));
                        }

                        if (partTypeNode != null)
                        {
                            itemData.partType = partTypeNode.Attributes[1].Value;
                        }

                        if (attachmentsNode != null)
                        {

                            itemData.attachmentNames = attachmentsNode.Attributes[1].Value.Split(',');
                        }

                        if (partsNode != null)
                        {
                            itemData.partNames = new string[4];

                            itemData.partNames[0] = GetChildNodeByName(partsNode, "Stock").Attributes[1].Value;
                            itemData.partNames[1] = GetChildNodeByName(partsNode, "Receiver").Attributes[1].Value;
                            itemData.partNames[2] = GetChildNodeByName(partsNode, "Pump").Attributes[1].Value;
                            itemData.partNames[3] = GetChildNodeByName(partsNode, "Barrel").Attributes[1].Value;
                        }

                        if (magazineSizeNode != null)
                        {
                            itemData.magazineSize = int.Parse(magazineSizeNode.Attributes[1].Value);
                            itemData.magazineItems = GetChildNodeByName(action0Node, "Magazine_items").Attributes[1].Value.Split(',');
                        }
                    }

                    if (customIconNode != null)
                    {
                        IconData.itemIconDictionary.TryGetValue(customIconNode.Attributes[1].Value, out itemData.iconPixels);
                    }

                    else if (IconData.itemIconDictionary.ContainsKey(itemData.name))
                    {
                        IconData.itemIconDictionary.TryGetValue(itemData.name, out itemData.iconPixels);
                    }

                    itemList.Add(itemData);
                }
            }
        }

        public static void ArrangeItemList()
        {
            List<ItemData> itemList = ItemData.itemList;
            List<ItemData> devItems = ItemData.devItems;

            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i].isDeveloper)
                {
                    devItems.Add(itemList[i]);
                    itemList.Remove(itemList[i]);
                    i--;
                }
            }

            ItemData.nameList = new string[itemList.Count];

            int k = 0;
            foreach (ItemData itemData in itemList)
            {
                ItemData.nameList[k] = itemData.name;
                k++;
            }

            Util.Quicksort(ItemData.nameList, 0, itemList.Count);
        }

        public static void GetSkills()
        {
            List<SkillData> skillList = SkillData.skillList;
            XmlDocument document = new XmlDocument();
            document.Load(Config.GetSetting("gameRoot") + "\\Data\\Config\\progression.xml");

            XmlNode progressionNode = document.DocumentElement;

            XmlNode playerNode = progressionNode.ChildNodes[0];
            SkillData.maxPlayerLevel = int.Parse(playerNode.Attributes[0].Value);
            SkillData.expToPlayerLevel = int.Parse(playerNode.Attributes[1].Value);

            XmlNode skillsNode = progressionNode.ChildNodes[1];
            SkillData.maxLevelDefault = int.Parse(skillsNode.Attributes[0].Value);
            SkillData.expToLevelDefault = int.Parse(skillsNode.Attributes[1].Value);

            foreach (XmlNode skillNode in skillsNode.ChildNodes)
            {
                SkillData skillData = new SkillData();

                if (skillNode.Name.Equals("player_skill"))
                {
                    skillData.type = SkillType.PlayerSkill;
                }

                else if (skillNode.Name.Equals("action_skill"))
                {
                    skillData.type = SkillType.ActionSkill;
                }

                else if (skillNode.Name.Equals("perk"))
                {
                    skillData.type = SkillType.Perk;
                }

                else if (skillNode.Name.Equals("crafting_skill"))
                {
                    skillData.type = SkillType.CraftingSkill;
                }

                else
                {
                    continue;
                }

                skillData.name = GetAttribute(skillNode, "name").Value;

                XmlAttribute iconAttr = GetAttribute(skillNode, "icon");
                if (iconAttr != null)
                {
                    string iconName = "ui_game_symbol_" + iconAttr.Value;

                    IconData.uiIconDictionary.TryGetValue(iconName, out skillData.iconData);
                }

                XmlAttribute maxLevelAttr = GetAttribute(skillNode, "max_level");
                if (maxLevelAttr != null)
                {
                    skillData.maxLevel = int.Parse(maxLevelAttr.Value);
                }

                else
                {
                    skillData.maxLevel = SkillData.maxLevelDefault;
                }

                XmlAttribute expToLevelAttr = GetAttribute(skillNode, "exp_to_level");
                if (expToLevelAttr != null)
                {
                    skillData.expToLevel = int.Parse(expToLevelAttr.Value);
                }

                else if (skillData.type == SkillType.Perk)
                {
                    skillData.expToLevel = 0;
                }

                else
                {
                    skillData.expToLevel = SkillData.expToLevelDefault;
                }

                skillData.requirements = new List<Requirement>();
                skillData.recipes = new Dictionary<string, int>();

                for (int i = 0; i < skillNode.ChildNodes.Count; i++)
                {
                    if (skillNode.ChildNodes[i].Name.Equals("requirement"))
                    {
                        XmlNode requirementNode = skillNode.ChildNodes[i];

                        XmlAttribute thisPerkLevelAttr = GetAttribute(requirementNode, "perk_level");
                        XmlAttribute requiredSkillNameAttr = GetAttribute(requirementNode, "required_skill_name");
                        XmlAttribute requiredSkillLevelAttr = GetAttribute(requirementNode, "required_skill_level");
                        XmlAttribute requiredPlayerLevelAttr = GetAttribute(requirementNode, "required_player_level");

                        if (thisPerkLevelAttr == null)
                        {
                            thisPerkLevelAttr = GetAttribute(requirementNode, "skill_level");
                        }

                        int perkLevel = int.Parse(thisPerkLevelAttr.Value);
                        string requiredSkillName;
                        int requiredSkillLevel;

                        if (requiredSkillNameAttr != null)
                        {
                            requiredSkillName = requiredSkillNameAttr.Value;
                            requiredSkillLevel = int.Parse(requiredSkillLevelAttr.Value);
                        }

                        else
                        {
                            requiredSkillName = "Player Level";
                            requiredSkillLevel = int.Parse(requiredPlayerLevelAttr.Value);
                        }

                        skillData.requirements.Add(new Requirement(perkLevel, requiredSkillName, requiredSkillLevel));
                    }

                    else if (skillNode.ChildNodes[i].Name.Equals("recipe"))
                    {
                        XmlNode recipeNode = skillNode.ChildNodes[i];

                        XmlAttribute recipeNameAttr = GetAttribute(recipeNode, "name");
                        XmlAttribute recipeUnlockLevelAttr = GetAttribute(recipeNode, "unlock_level");

                        skillData.recipes.Add(recipeNameAttr.Value, int.Parse(recipeUnlockLevelAttr.Value));
                    }
                }

                skillList.Add(skillData);
            }
        }

        public static void GetQuests()
        {
            List<QuestData> questList = QuestData.questList;
            XmlDocument document = new XmlDocument();
            document.Load(Config.GetSetting("gameRoot") + "\\Data\\Config\\quests.xml");

            int count = 0;
            XmlNode questsNode;

            do
            {
                questsNode = document.ChildNodes[count];
                count++;
            } while (!questsNode.Name.Equals("quests"));

            foreach (XmlNode questNode in questsNode.ChildNodes)
            {
                if (questNode.Name.Equals("quest"))
                {
                    QuestData questData = new QuestData();

                    questData.id = GetAttribute(questNode, "id").Value;
                    questData.categoryKey = GetAttribute(questNode, "category_key").Value;
                    questData.objectives = new List<ObjectiveData>();

                    foreach (XmlNode objectiveNode in questNode.ChildNodes)
                    {
                        if (objectiveNode.Name.Equals("objective"))
                        {
                            ObjectiveData objectiveData = new ObjectiveData();

                            string type = GetAttribute(objectiveNode, "type").Value;

                            XmlAttribute idAttr = GetAttribute(objectiveNode, "id");
                            string id = "";
                            if (idAttr != null)
                            {
                                id = " " + idAttr.Value;
                            }

                            objectiveData.name = type + id;

                            //if single value, stored in maxValue
                            XmlAttribute valueAttr = GetAttribute(objectiveNode, "value");
                            objectiveData.minValue = 0;
                            objectiveData.maxValue = 0;
                            if (valueAttr != null)
                            {
                                if (!int.TryParse(valueAttr.Value, out objectiveData.maxValue))
                                {
                                    string[] values = valueAttr.Value.Split('-');
                                    objectiveData.minValue = int.Parse(values[0]);
                                    objectiveData.maxValue = int.Parse(values[1]);
                                }
                            }

                            questData.objectives.Add(objectiveData);
                        }
                    }

                    questList.Add(questData);
                }
            }
        }

        private static XmlNode GetChildNodeByName(XmlNode node, string propertyName)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Attributes != null && child.Attributes[0].Value == propertyName)
                {
                    return child;
                }
            }

            return null;
        }

        private static XmlAttribute GetAttribute(XmlNode node, string attributeName)
        {
            foreach (XmlAttribute attribute in node.Attributes)
            {
                if (attribute.Name == attributeName)
                {
                    return attribute;
                }
            }

            return null;
        }
    }
}