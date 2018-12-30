using SevenDaysXMLParser.source;
using SevenDaysXMLParser.source.Quests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SevenDaysXMLParser.Quests
{
    public class QuestsXml
    {
        public Dictionary<string, Quest> quests = new Dictionary<string, Quest>();
        public Dictionary<string, QuestList> questLists = new Dictionary<string, QuestList>();
        public QuestItems questItems;
        public Dictionary<int, QuestTierReward> questTierRewards = new Dictionary<int, QuestTierReward>();

        public QuestsXml(string path) {
            Load(path);
        }

        public void Load(string path) {
            Read(Utils.LoadXml(path));
        }

        private void Read(XmlDocument questsXml) {
            if (questsXml.ChildNodes.Count < 1) {
                throw new XmlException("Quests.xml root node not found!");
            }

            foreach(XmlElement node in questsXml.ChildNodes) {
                try {
                    switch (node.Name) {
                        case "quest":
                            Quest quest = new Quest(node);
                            quests[quest.id] = quest;
                            break;
                        case "quest_list":
                            QuestList questList = new QuestList(node);
                            questLists[questList.id] = questList;
                            break;
                        case "quest_items":
                            questItems = new QuestItems(node);
                            break;
                        case "quest_tier_rewards":
                            foreach (XmlElement child in node.ChildNodes) {
                                int tier = (int)Utils.GetAttribute<int>(child, "quest_tier_reward", 0);
                                questTierRewards[tier] = new QuestTierReward(child);
                            }
                            break;
                        default:
                            break;
                    }
                } catch (XmlException) {}
            }
        }

    }
}
