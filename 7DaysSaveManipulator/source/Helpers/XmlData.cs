using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace SevenDaysSaveManipulator {

    public class XmlData {

        private XmlDocument blocks;
        private XmlDocument items;
        private XmlDocument itemModifiers;
        private XmlDocument quests;
        private XmlDocument traders;

        private Dictionary<string, int> questObjectiveCount = new Dictionary<string, int>();
        private Dictionary<string, int> questRewardCount = new Dictionary<string, int>();
        private Dictionary<string, int> questVersions = new Dictionary<string, int>();
        private Dictionary<string, string> blockShape = new Dictionary<string, string>();
        private Dictionary<int, bool> isItemModifier = new Dictionary<int, bool>();
        private Dictionary<int, bool> isRentable = new Dictionary<int, bool>();

        private readonly NameIdMapping nameIdMapping;

        internal XmlData(string blockMappingsPath, string itemMappingsPath, string blocksXmlPath, string itemsXmlPath, string itemModifiersXmlPath, string questXmlPath, string tradersXmlPath) {
            //try {
            quests = new XmlDocument();
            quests.Load(questXmlPath);

            blocks = new XmlDocument();
            blocks.Load(blocksXmlPath);

            items = new XmlDocument();
            items.Load(itemsXmlPath);

            itemModifiers = new XmlDocument();
            itemModifiers.Load(itemModifiersXmlPath);

            traders = new XmlDocument();
            traders.Load(tradersXmlPath);

            nameIdMapping = new NameIdMapping(blockMappingsPath, itemMappingsPath, blocksXmlPath, itemsXmlPath, itemModifiersXmlPath, this);
            /*} catch (Exception) {
                //TODO write an exeption
                throw new Exception("Failed to read the xml files necessary for parsing the save file!");
            }*/
        }

        private string GetAncestor(XmlDocument doc, string stringStart, string childName) {
            XmlNode ancestorData = doc.SelectSingleNode(stringStart + childName + "']/property[@name='Extends']/@value");

            if (ancestorData == null) {
                return "";
            }

            return ancestorData.Value;
        }

        internal string GetAttributeValue(XmlDocument doc, string stringStart, string childName, string attributePath) {
            XmlNode attributeValue = doc.SelectSingleNode(stringStart + childName + "']/" + attributePath);

            if (attributeValue != null) {
                return attributeValue.Value;
            }

            string ancestor = GetAncestor(doc, stringStart, childName);
            if (!ancestor.Equals("")) {
                return GetAttributeValue(doc, stringStart, ancestor, attributePath);
            }

            return "";
        }

        internal string GetBlockShape(string blockName) {
            if (!blockShape.ContainsKey(blockName)) {
                blockShape[blockName] = GetAttributeValue(blocks, "blocks/block[@name='", blockName, "property[@name='Shape']/@value");
            }

            return blockShape[blockName];
        }

        internal int GetCurrentQuestVersion(string questID) {
            if (!questVersions.ContainsKey(questID)) {
                string currentQuestVersion = GetAttributeValue(quests, "quests/quest[@id='", questID, "property[@name='currentVersion']/@value");
                questVersions[questID] = currentQuestVersion == "" ? 0 : int.Parse(currentQuestVersion);
            }

            return questVersions[questID];
        }

        internal int GetObjectiveCount(string questID) {
            if (!questObjectiveCount.ContainsKey(questID)) {
                questObjectiveCount[questID] = quests.SelectNodes("/quests/quest[translate(@id,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='" + questID + "']/objective").Count;
            }

            return questObjectiveCount[questID];
        }

        internal int GetRewardCount(string questID) {
            if (!questRewardCount.ContainsKey(questID)) {
                questRewardCount[questID] = quests.SelectNodes("/quests/quest[translate(@id,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='" + questID + "']/reward").Count;
            }

            return questRewardCount[questID];
        }

        internal bool IsItemModifier(int itemID) {
            if (!isItemModifier.ContainsKey(itemID)) {
                isItemModifier[itemID] = itemModifiers.SelectSingleNode("/item_modifiers/item_modifier[@name='" + nameIdMapping.idToName[itemID] + "']") != null;
            }

            IsRentable(4);

            return isItemModifier[itemID];
        }

        internal bool IsRentable(int traderID) {
            if (!isRentable.ContainsKey(traderID)) {
                XmlNode rentable = traders.SelectSingleNode("/traders/trader_info[@id='" + traderID + "']/@rentable");
                isRentable[traderID] = rentable != null && rentable.Value.Equals("true");
            }

            return isRentable[traderID];
        }
    }
}
