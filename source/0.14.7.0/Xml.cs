using SevenDaysProfileEditor.Inventory;
using SevenDaysProfileEditor.Skills;
using SevenDaysSaveManipulator;
using SevenDaysSaveManipulator.GameData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace SevenDaysProfileEditor
{
    class Xml
    {       
        public static void initialize(string gameRoot)
        {
            try
            {
                getBlocks(gameRoot);
                getItems(gameRoot);
                arrangeItemList();
                getSkills(gameRoot);
            }

            catch (Exception e)
            {
                Log.writeError(e);
                MessageBox.Show("Failed to load XML files. " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private static void getBlocks(string gameRoot)
        {
            List<DataItem> itemList = DataItem.itemList;
            XmlDocument document = new XmlDocument();
            document.Load(gameRoot + "\\Data\\Config\\blocks.xml");

            XmlNode blocks = document.DocumentElement;

            foreach (XmlNode block in blocks.ChildNodes)
            {
                if (block.Name.Equals("block"))
                {
                    XmlNode developer = getChildNodeByName(block, "IsDeveloper");

                    bool isDeveloper = false;
                    if (developer != null && developer.Attributes[1].Value.Equals("true"))
                    {
                        isDeveloper = true;
                    }

                    if (!isDeveloper)
                    {
                        DataItem dataItem = new DataItem();
                        dataItem.id = int.Parse(block.Attributes[0].Value);
                        dataItem.name = block.Attributes[1].Value;
                        dataItem.stackNumber = 500;

                        itemList.Add(dataItem);
                    }
                }
            }
        }

        private static void getItems(string gameRoot)
        {
            List<DataItem> itemList = DataItem.itemList;
            XmlDocument document = new XmlDocument();
            document.Load(gameRoot + "\\Data\\Config\\items.xml");

            XmlNode items = document.DocumentElement;

            foreach (XmlNode item in items.ChildNodes)
            {
                if (item.Name.Equals("item"))
                {
                    XmlNode developer = getChildNodeByName(item, "IsDeveloper");
                    XmlNode extends = getChildNodeByName(item, "Extends");
                    XmlNode stackNumber = getChildNodeByName(item, "Stacknumber");
                    XmlNode repairTools = getChildNodeByName(item, "RepairTools");
                    XmlNode attributes = getChildNodeByName(item, "Attributes");
                    XmlNode degradationMax = null;
                    if (attributes != null)
                    {
                        degradationMax = getChildNodeByName(attributes, "DegradationMax");
                    }

                    XmlNode partType = getChildNodeByName(item, "PartType");
                    XmlNode attachments = getChildNodeByName(item, "Attachments");
                    XmlNode parts = getChildNodeByName(item, "Parts");
                    XmlNode action0 = getChildNodeByName(item, "Action0");
                    XmlNode magazineSize = null;
                    if (action0 != null)
                    {
                        magazineSize = getChildNodeByName(action0, "Magazine_size");
                    }

                    DataItem itemData = new DataItem();

                    if (extends != null)
                    {
                        string parentItemName = extends.Attributes[1].Value;
                        itemData.copy(DataItem.getItemDataByName(parentItemName));
                    }

                    if (developer != null)
                    {
                        itemData.isDeveloper = developer.Attributes[1].Value.Equals("true");
                    }

                    else
                    {
                        itemData.isDeveloper = false;
                    }

                    itemData.id = int.Parse(item.Attributes[0].Value) + 4096;
                    itemData.name = item.Attributes[1].Value;

                    if (stackNumber != null)
                    {
                        itemData.stackNumber = int.Parse(stackNumber.Attributes[1].Value);
                    }

                    else if (parts != null || attributes != null)
                    {
                        itemData.stackNumber = 1;
                    }

                    else if (itemData.stackNumber == 0)
                    {
                        itemData.stackNumber = DataItem.DEFAULT_STACKNUMBER;
                    }

                    if (repairTools != null || partType != null)
                    {
                        string name = itemData.name;

                        itemData.hasQuality = true;

                        if (degradationMax != null)
                        {
                            string degradationS = degradationMax.Attributes[1].Value;

                            itemData.degradationMin = int.Parse(degradationS.Substring(0, degradationS.IndexOf(',')));
                            itemData.degradationMax = int.Parse(degradationS.Substring(degradationS.IndexOf(',') + 1));
                        }

                        if (partType != null)
                        {
                            itemData.partType = partType.Attributes[1].Value;
                        }

                        if (attachments != null)
                        {
                            itemData.attachmentNames = Util.pulverizeString(attachments.Attributes[1].Value);
                        }

                        if (parts != null)
                        {
                            itemData.partNames = new string[4];

                            itemData.partNames[0] = getChildNodeByName(parts, "Stock").Attributes[1].Value;
                            itemData.partNames[1] = getChildNodeByName(parts, "Receiver").Attributes[1].Value;
                            itemData.partNames[2] = getChildNodeByName(parts, "Pump").Attributes[1].Value;
                            itemData.partNames[3] = getChildNodeByName(parts, "Barrel").Attributes[1].Value;
                        }

                        if (magazineSize != null)
                        {
                            itemData.magazineSize = int.Parse(magazineSize.Attributes[1].Value);
                            itemData.magazineItems = Util.pulverizeString(getChildNodeByName(action0, "Magazine_items").Attributes[1].Value);
                        }
                    }

                    itemList.Add(itemData);
                }
            }
        }

        private static void arrangeItemList()
        {
            List<DataItem> itemList = DataItem.itemList;
            List<DataItem> devItems = DataItem.devItems;

            //REMOVE DEVELOPER ITEMS
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i].isDeveloper)
                {
                    devItems.Add(itemList[i]);
                    itemList.Remove(itemList[i]); 
                    i--;
                }
            }

            //CREATE NAME-ONLY LIST
            DataItem.nameList = new string[itemList.Count];

            int k = 0;

            foreach (DataItem itemData in itemList)
            {
                DataItem.nameList[k] = itemData.name;
                k++;
            }

            //SORT ITEMS
            Util.quicksort(DataItem.nameList, 0, itemList.Count);
        }  
        
        private static void getSkills(string gameRoot)
        {
            List<DataSkill> skillList = DataSkill.skillList;
            XmlDocument document = new XmlDocument();
            document.Load(gameRoot + "\\Data\\Config\\progression.xml");

            XmlNode progression = document.DocumentElement;

            XmlNode player = progression.ChildNodes[0];
            DataSkill.maxPlayerLevel = int.Parse(player.Attributes[0].Value);
            DataSkill.expToPlayerLevel = int.Parse(player.Attributes[1].Value);

            XmlNode skills = progression.ChildNodes[1];
            DataSkill.maxLevelDefault = int.Parse(skills.Attributes[0].Value);
            DataSkill.expToLevelDefault = int.Parse(skills.Attributes[1].Value);

            foreach (XmlNode skill in skills.ChildNodes)
            {
                DataSkill dataSkill = new DataSkill();

                if (skill.Name.Equals("player_skill"))
                {
                    dataSkill.type = SkillType.PlayerSkill;
                }

                else if (skill.Name.Equals("action_skill"))
                {
                    dataSkill.type = SkillType.ActionSkill;
                }

                else if (skill.Name.Equals("perk"))
                {
                    dataSkill.type = SkillType.Perk;
                }

                else if (skill.Name.Equals("crafting_skill"))
                {
                    dataSkill.type = SkillType.CraftingSkill;
                }

                else
                {
                    continue;
                }

                dataSkill.name = getAttribute(skill, "name").Value;

                XmlAttribute maxLevel = getAttribute(skill, "max_level");
                if (maxLevel != null)
                {
                    dataSkill.maxLevel = int.Parse(maxLevel.Value);
                }

                else
                {
                    dataSkill.maxLevel = DataSkill.maxLevelDefault;
                }

                XmlAttribute expToLevel = getAttribute(skill, "exp_to_level");
                if (expToLevel != null)
                {
                    dataSkill.expToLevel = int.Parse(expToLevel.Value);
                }

                else if (dataSkill.type == SkillType.Perk)
                {
                    dataSkill.expToLevel = 0;
                }

                else
                {
                    dataSkill.expToLevel = DataSkill.expToLevelDefault;
                }

                if (hasAttribute(skill, "requirement"))
                {
                    dataSkill.requirements = new List<Tuple<string, int>>();

                    for (int i = 1; i < dataSkill.maxLevel + 1; i++)
                    {
                        int counter = 0;
                        XmlNode requirement = null;

                        while (counter <= skill.ChildNodes.Count)
                        {
                            requirement = skill.ChildNodes[counter++];

                            if (requirement.Name.Equals("requirement") && int.Parse(requirement.Attributes[0].Value) == i)
                            {
                                break;
                            }
                        }

                        XmlAttribute requiredSkillName = getAttribute(requirement, "required_skill_name");
                        XmlAttribute requiredSkillLevel = getAttribute(requirement, "required_skill_level");
                        XmlAttribute requiredPlayerLevel = getAttribute(requirement, "required_player_level");

                        string name;
                        int level;

                        if (requiredSkillName != null)
                        {
                            name = requiredSkillName.Value;
                            level = int.Parse(requiredSkillLevel.Value);
                        }

                        else
                        {
                            name = "Player Level";
                            level = int.Parse(requiredPlayerLevel.Value);
                        }

                        dataSkill.requirements.Add(new Tuple<string, int>(name, level));
                    }
                }

                skillList.Add(dataSkill);
            }
        }  

        private static XmlNode getChildNodeByName(XmlNode node, string propertyName)
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

        private static XmlAttribute getAttribute(XmlNode node, string attributeName)
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

        private static bool hasAttribute(XmlNode node, string attribute)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name.Equals(attribute))
                {
                    return true;
                }
            }

            return false;
        }
    }
}