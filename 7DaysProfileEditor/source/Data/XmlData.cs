using SevenDaysProfileEditor.Inventory;
using SevenDaysProfileEditor.Skills;
using SevenDaysProfileEditor.StatsAndGeneral;
using System.Collections.Generic;
using System.Xml;

namespace SevenDaysProfileEditor.Data {

    /// <summary>
    /// Handles all the XML loading in the program.
    /// </summary>
    internal class XmlData {

        /// <summary>
        /// Removes all the dev items from the main list and adds it to designated one. Also creates a separate list holding only item names.
        /// </summary>
        public static void ArrangeItemList() {
            List<ItemData> itemList = ItemData.itemList;
            List<ItemData> devItems = ItemData.devItems;

            for (int i = 0; i < itemList.Count; i++) {
                if (itemList[i].isDeveloper) {
                    devItems.Add(itemList[i]);
                    itemList.Remove(itemList[i]);
                    i--;
                }
            }

            ItemData.nameList = new string[itemList.Count];

            int k = 0;
            foreach (ItemData itemData in itemList) {
                ItemData.nameList[k] = itemData.name;
                k++;
            }

            Util.Quicksort(ItemData.nameList);
        }

        /// <summary>
        /// Deals with the blocks.xml.
        /// </summary>
        public static void GetBlocks() {
            List<ItemData> itemList = ItemData.itemList;
            XmlDocument document = new XmlDocument();
            document.Load(Config.GetSetting("gameRoot") + "\\Data\\Config\\blocks.xml");

            XmlNode blocksNode = document.DocumentElement;

            foreach (XmlNode blockNode in blocksNode.ChildNodes) {
                if (blockNode.Name.Equals("block")) {
                    XmlNode extendsNode = GetChildNodeByName(blockNode, "Extends");
                    XmlNode developerNode = GetChildNodeByName(blockNode, "IsDeveloper");

                    ItemData itemData = new ItemData();

                    if (extendsNode != null) {
                        string parentItemName = extendsNode.Attributes[1].Value;
                        ItemData parent = ItemData.GetItemDataByName(parentItemName);
                        itemData.Copy(parent);
                    }

                    if (developerNode != null) {
                        itemData.isDeveloper = developerNode.Attributes[1].Value.Equals("true");
                    }

                    itemData.id = int.Parse(blockNode.Attributes[0].Value);
                    itemData.name = blockNode.Attributes[1].Value;
                    itemData.stackNumber = ItemData.DEFAULT_STACKNUMBER;

                    if ((IconData.itemIconDictionary != null) && (IconData.itemIconDictionary.ContainsKey(itemData.name))) {
                        IconData.itemIconDictionary.TryGetValue(itemData.name, out itemData.iconPixels);
                    }

                    itemList.Add(itemData);
                }
            }
        }

        /// <summary>
        /// Deals with items.xml.
        /// </summary>
        public static void GetItems() {
            List<ItemData> itemList = ItemData.itemList;
            XmlDocument document = new XmlDocument();
            document.Load(Config.GetSetting("gameRoot") + "\\Data\\Config\\items.xml");

            XmlNode itemsNode = document.DocumentElement;

            foreach (XmlNode itemNode in itemsNode.ChildNodes) {
                if (itemNode.Name.Equals("item")) {
                    XmlNode extendsNode = GetChildNodeByName(itemNode, "Extends");
                    XmlNode developerNode = GetChildNodeByName(itemNode, "IsDeveloper");
                    XmlNode stackNumberNode = GetChildNodeByName(itemNode, "Stacknumber");
                    XmlNode repairToolsNode = GetChildNodeByName(itemNode, "RepairTools");
                    XmlNode attributesNode = GetChildNodeByName(itemNode, "Attributes");
                    XmlNode degradationMaxNode = null;
                    if (attributesNode != null) {
                        degradationMaxNode = GetChildNodeByName(attributesNode, "DegradationMax");
                    }

                    XmlNode partTypeNode = GetChildNodeByName(itemNode, "PartType");
                    XmlNode attachmentsNode = GetChildNodeByName(itemNode, "Attachments");
                    XmlNode partsNode = GetChildNodeByName(itemNode, "Parts");
                    XmlNode action0Node = GetChildNodeByName(itemNode, "Action0");
                    XmlNode magazineSizeNode = null;
                    if (action0Node != null) {
                        magazineSizeNode = GetChildNodeByName(action0Node, "Magazine_size");
                    }

                    XmlNode customIconNode = GetChildNodeByName(itemNode, "CustomIcon");

                    ItemData itemData = new ItemData();

                    if (extendsNode != null) {
                        string parentItemName = extendsNode.Attributes[1].Value;
                        ItemData parent = ItemData.GetItemDataByName(parentItemName);
                        itemData.Copy(parent);
                    }

                    if (developerNode != null) {
                        itemData.isDeveloper = developerNode.Attributes[1].Value.Equals("true");
                    }
                    else {
                        itemData.isDeveloper = false;
                    }

                    itemData.id = int.Parse(itemNode.Attributes[0].Value) + 4096;
                    itemData.name = itemNode.Attributes[1].Value;

                    if (stackNumberNode != null) {
                        itemData.stackNumber = int.Parse(stackNumberNode.Attributes[1].Value);
                    }
                    else if (partsNode != null || attributesNode != null) {
                        itemData.stackNumber = 1;
                    }
                    else if (itemData.stackNumber == 0) {
                        itemData.stackNumber = ItemData.DEFAULT_STACKNUMBER;
                    }

                    if (repairToolsNode != null || partTypeNode != null) {
                        string name = itemData.name;

                        itemData.hasQuality = true;

                        if (degradationMaxNode != null) {
                            string degradationS = degradationMaxNode.Attributes[1].Value;

                            itemData.degradationMin = int.Parse(degradationS.Substring(0, degradationS.IndexOf(',')));
                            itemData.degradationMax = int.Parse(degradationS.Substring(degradationS.IndexOf(',') + 1));
                        }

                        if (partTypeNode != null) {
                            itemData.partType = partTypeNode.Attributes[1].Value;
                        }

                        if (attachmentsNode != null) {
                            itemData.attachmentNames = attachmentsNode.Attributes[1].Value.Split(',');
                        }

                        if (partsNode != null) {
                            itemData.partNames = new string[4];

                            itemData.partNames[0] = GetChildNodeByName(partsNode, "Stock").Attributes[1].Value;
                            itemData.partNames[1] = GetChildNodeByName(partsNode, "Receiver").Attributes[1].Value;
                            itemData.partNames[2] = GetChildNodeByName(partsNode, "Pump").Attributes[1].Value;
                            itemData.partNames[3] = GetChildNodeByName(partsNode, "Barrel").Attributes[1].Value;
                        }

                        if (magazineSizeNode != null) {
                            itemData.magazineSize = int.Parse(magazineSizeNode.Attributes[1].Value);
                            itemData.magazineItems = GetChildNodeByName(action0Node, "Magazine_items").Attributes[1].Value.Split(',');
                        }
                    }

                    if ((customIconNode != null) && (IconData.itemIconDictionary != null)) {
                        IconData.itemIconDictionary.TryGetValue(customIconNode.Attributes[1].Value, out itemData.iconPixels);
                    }
                    else if ((IconData.itemIconDictionary != null) && (IconData.itemIconDictionary.ContainsKey(itemData.name))) {
                        IconData.itemIconDictionary.TryGetValue(itemData.name, out itemData.iconPixels);
                    }

                    if (!itemData.isDeveloper && extendsNode != null && extendsNode.Attributes[1].Value.Equals("schematicMaster")) {
                        XmlNode action1Node = GetChildNodeByName(itemNode, "Action1");
                        XmlNode recipesToLearnNode = GetChildNodeByName(action1Node, "Recipes_to_learn");

                        if (recipesToLearnNode != null) {
                            string recipesToLearn = recipesToLearnNode.Attributes[1].Value;
                            string[] recipes = recipesToLearn.Split(',');

                            foreach (string recipe in recipes) {
                                RecipeData.recipeList.Add(new RecipeData(recipe, true));
                            }
                        }
                    }

                    itemList.Add(itemData);
                }
            }
        }

        /// <summary>
        /// Deals with progression.xml.
        /// </summary>
        public static void GetSkills() {
            List<SkillData> skillList = SkillData.skillList;
            XmlDocument document = new XmlDocument();
            document.Load(Config.GetSetting("gameRoot") + "\\Data\\Config\\progression.xml");

            XmlNode progressionNode = document.DocumentElement;

            XmlNode playerNode = progressionNode.SelectSingleNode("player");
            SkillData.maxPlayerLevel = int.Parse(playerNode.Attributes["max_level"].Value);
            SkillData.expToPlayerLevel = int.Parse(playerNode.Attributes["exp_to_level"].Value);
            
            XmlNode skillsNode = progressionNode.SelectSingleNode("skills");
            SkillData.maxLevelDefault = int.Parse(skillsNode.Attributes["max_level"].Value);
            SkillData.expToLevelDefault = int.Parse(skillsNode.Attributes["exp_to_level"].Value);

            foreach (XmlNode skillNode in skillsNode.ChildNodes) {
                SkillData skillData = new SkillData();

                if (skillNode.Name.Equals("player_skill")) {
                    skillData.type = SkillType.PlayerSkill;
                }
                else if (skillNode.Name.Equals("action_skill")) {
                    skillData.type = SkillType.ActionSkill;
                }
                else if (skillNode.Name.Equals("perk")) {
                    skillData.type = SkillType.Perk;
                }
                else if (skillNode.Name.Equals("crafting_skill")) {
                    skillData.type = SkillType.CraftingSkill;
                }
                else {
                    continue;
                }

                skillData.name = skillNode.Attributes["name"].Value;

                XmlAttribute iconAttr = skillNode.Attributes["icon"];
                if (iconAttr != null) {
                    string iconName = "ui_game_symbol_" + iconAttr.Value;
                    if (IconData.itemIconDictionary != null) {
                        IconData.uiIconDictionary.TryGetValue(iconName, out skillData.iconData);
                    }
                }

                XmlAttribute maxLevelAttr = skillNode.Attributes["max_level"];
                if (maxLevelAttr != null) {
                    skillData.maxLevel = int.Parse(maxLevelAttr.Value);
                }
                else {
                    skillData.maxLevel = SkillData.maxLevelDefault;
                }

                XmlAttribute expToLevelAttr = skillNode.Attributes["exp_to_level"];
                if (expToLevelAttr != null) {
                    skillData.expToLevel = int.Parse(expToLevelAttr.Value);
                }
                else if (skillData.type == SkillType.Perk) {
                    skillData.expToLevel = 0;
                }
                else {
                    skillData.expToLevel = SkillData.expToLevelDefault;
                }

                skillData.requirements = new List<Requirement>();
                skillData.recipes = new List<RecipeData>();

                for (int i = 0; i < skillNode.ChildNodes.Count; i++) {
                    if (skillNode.ChildNodes[i].Name.Equals("requirement")) {
                        XmlNode requirementNode = skillNode.ChildNodes[i];

                        XmlAttribute thisPerkLevelAttr = requirementNode.Attributes["perk_level"];
                        XmlAttribute requiredSkillNameAttr = requirementNode.Attributes["required_skill_name"];
                        XmlAttribute requiredSkillLevelAttr = requirementNode.Attributes["required_skill_level"];
                        XmlAttribute requiredPlayerLevelAttr = requirementNode.Attributes["required_player_level"];

                        if (thisPerkLevelAttr == null) {
                            thisPerkLevelAttr = requirementNode.Attributes["skill_level"];
                        }

                        int perkLevel = int.Parse(thisPerkLevelAttr.Value);
                        string requiredSkillName;
                        int requiredSkillLevel;

                        if (requiredSkillNameAttr != null) {
                            requiredSkillName = requiredSkillNameAttr.Value;
                            requiredSkillLevel = int.Parse(requiredSkillLevelAttr.Value);
                        }
                        else {
                            requiredSkillName = "Player Level";
                            requiredSkillLevel = int.Parse(requiredPlayerLevelAttr.Value);
                        }

                        skillData.requirements.Add(new Requirement(perkLevel, requiredSkillName, requiredSkillLevel));
                    }
                    else if (skillNode.ChildNodes[i].Name.Equals("recipe")) {
                        XmlNode recipeNode = skillNode.ChildNodes[i];

                        XmlAttribute recipeNameAttr = recipeNode.Attributes["name"];
                        XmlAttribute recipeUnlockLevelAttr = recipeNode.Attributes["unlock_level"];

                        RecipeData skillsRecipe = RecipeData.GetRecipeDataByName(recipeNameAttr.Value);
                        if (skillsRecipe == null) {
                            skillsRecipe = new RecipeData(recipeNameAttr.Value, false);
                            RecipeData.recipeList.Add(skillsRecipe);
                        }

                        if (skillsRecipe.skillUnlockConditions == null) {
                            skillsRecipe.skillUnlockConditions = new Dictionary<SkillData, int>();
                        }

                        if (!skillsRecipe.skillUnlockConditions.ContainsKey(skillData))
                        {
                            skillsRecipe.skillUnlockConditions.Add(skillData, int.Parse(recipeUnlockLevelAttr.Value));
                        }

                        skillData.recipes.Add(skillsRecipe);
                    }
                }

                skillList.Add(skillData);
            }
        }

        /// <summary>
        /// Gets first attribute of specified name of a node.
        /// </summary>
        /// <param name="node">Node to look for attribute in</param>
        /// <param name="attributeName">Name of the attribute</param>
        /// <returns>Attribute if found, else null</returns>
        [System.Obsolete("GetAttribute is deprecated, please use node.Attribute['name'] instead.")]
        private static XmlAttribute GetAttribute(XmlNode node, string attributeName) {
            return node.Attributes[attributeName];
            /*
            foreach (XmlAttribute attribute in node.Attributes) {
                if (attribute.Name == attributeName) {
                    return attribute;
                }
            }

            return null;
            */
        }

        /// <summary>
        /// Gets first child with leading attribute as speficied.
        /// </summary>
        /// <param name="node">Parent node</param>
        /// <param name="propertyName">Name of the leading attribute</param>
        /// <returns>Node if found, else null</returns>
        private static XmlNode GetChildNodeByName(XmlNode node, string propertyName) {
            //TODO: Remove function.
            //Faster way by using XPath, but need to figure out exact syntax.
            //something like...
            //return node.SelectSingleNode("\[@" + propertyName + "]");
       
            foreach (XmlNode child in node.ChildNodes) {
                if (child.Attributes != null && child.Attributes[0].Value == propertyName) {
                    return child;
                }
            }

            return null;
        }
    }
}