using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace SevenDaysSaveManipulator {

    public class AdditionalFileData {

        /* 
         * ID 0             => Air
         * ID [1,256]       => terrain blocks (attribute Shape=Terrain)
         * ID [257,32768]   => other blocks
         * ID [32786,65536] => items directly followed by itemModifiers
        */

        internal XmlDocument blocks = new XmlDocument();
        internal XmlDocument items = new XmlDocument();
        internal XmlDocument itemModifiers = new XmlDocument();
        internal XmlDocument quests = new XmlDocument();
        internal XmlDocument traders = new XmlDocument();

        internal Dictionary<int, string> idToName = new Dictionary<int, string>();
        internal Dictionary<string, int> nameToId = new Dictionary<string, int>();

        private static readonly int TERRAIN_MAX = 256;
        internal static readonly int BLOCK_MAX = 32768;
        internal static readonly int ITEM_MAX = BLOCK_MAX * 2;

        private Dictionary<string, int> questObjectiveCount = new Dictionary<string, int>();
        private Dictionary<string, int> questRewardCount = new Dictionary<string, int>();
        private Dictionary<string, int> questVersions = new Dictionary<string, int>();
        private Dictionary<string, string> blockShape = new Dictionary<string, string>();
        private Dictionary<int, bool> isItemModifier = new Dictionary<int, bool>();
        private Dictionary<int, bool> isRentable = new Dictionary<int, bool>();

        private Dictionary<string, bool> recipeHasSchematic = new Dictionary<string, bool>();

        internal AdditionalFileData(string blockMappingsPath, string itemMappingsPath, XmlDocument blocks, XmlDocument items, XmlDocument itemModifiers, XmlDocument quests, XmlDocument traders) {
            this.blocks = blocks;
            this.items = items;
            this.itemModifiers = itemModifiers;
            this.quests = quests;
            this.traders = traders;

            ReadMappingsFile(blockMappingsPath);
            ReadMappingsFile(itemMappingsPath);

            //map blocks
            int currentId = 0;
            GenerateRestOfTheMappings(this.blocks, ref currentId);

            //map items
            currentId = BLOCK_MAX;
            GenerateRestOfTheMappings(this.items, ref currentId);
            GenerateRestOfTheMappings(this.itemModifiers, ref currentId);
        }

        private void GenerateDataTables() {

        }

        internal string GetBlockShape(string blockName) {
            if (!blockShape.ContainsKey(blockName)) {
                blockShape[blockName] = GetAttributeValue(blocks, "blocks/block[@name='", blockName, "property[@name='Shape']/@value");
            }

            return blockShape[blockName];
        }

        internal int GetCurrentQuestVersion(string questID) {
            if (!questVersions.ContainsKey(questID)) {
                string currentQuestVersion = GetAttributeValue(quests, "quests/quest[translate(@id,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='", questID, "property[@name='currentVersion']/@value");
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
                isItemModifier[itemID] = itemModifiers.SelectSingleNode("/item_modifiers/item_modifier[@name='" + idToName[itemID] + "']") != null;
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

        private string GetAncestor(XmlDocument doc, string queryStart, string elementID) {
            XmlNode ancestorAttribute = doc.SelectSingleNode(queryStart + elementID + "']/property[@name='Extends']/@value");

            if (ancestorAttribute == null) {
                return "";
            }

            return ancestorAttribute.Value;
        }

        private void GenerateRestOfTheMappings(XmlDocument xml, ref int startId) {
            bool doneWithTerrain = false;
            foreach (XmlNode element in xml.DocumentElement.ChildNodes.Cast<XmlNode>().Where(node => node.NodeType != XmlNodeType.Comment)) {
                string name = ((XmlElement)element).GetAttribute("name");
                if (!nameToId.ContainsKey(name)) {
                    if (xml.DocumentElement.Name.Equals("blocks") && !doneWithTerrain) {
                        string shape = GetBlockShape(name);
                        if (shape == null || !shape.Equals("Terrain")) {
                            startId = TERRAIN_MAX;
                            doneWithTerrain = true;
                        }
                    }

                    while (idToName.ContainsKey(startId)) {
                        ++startId;
                    }

                    idToName[startId] = name;
                    nameToId[name] = startId;
                }
            }
        }

        private void ReadMappingsFile(string mappingFilePath) {
            using (BinaryReader reader = new BinaryReader(new FileStream(mappingFilePath, FileMode.Open))) {
                Utils.VerifyVersion(reader.ReadInt32(), SaveVersionConstants.NAME_ID_MAPPING, "NameIDMapping");

                int count = reader.ReadInt32();
                for (int i = 0; i < count; ++i) {
                    int id = reader.ReadInt32();
                    string name = reader.ReadString();
                    idToName[id] = name;
                    nameToId[name] = id;
                }
            }
        }

        internal void WriteMappingFile(string mappingFilePath, int startId, int endId) {
            using (BinaryWriter writer = new BinaryWriter(new FileStream(mappingFilePath, FileMode.Create))) {
                writer.Write(SaveVersionConstants.NAME_ID_MAPPING);

                Dictionary<int, string> pruned = idToName.Where(kv => kv.Key >= startId && kv.Key < endId).ToDictionary(dict => dict.Key, dict => dict.Value);
                writer.Write(pruned.Count);
                foreach (KeyValuePair<int, string> entry in pruned) {
                    writer.Write(entry.Key);
                    writer.Write(entry.Value);
                }
            }
        }

        private string GetAttributeValue(XmlDocument doc, string queryStart, string elementID, string attributePath) {
            XmlNode attributeValue = doc.SelectSingleNode(queryStart + elementID + "']/" + attributePath);

            if (attributeValue != null) {
                return attributeValue.Value;
            }

            string ancestorID = GetAncestor(doc, queryStart, elementID);
            if (!ancestorID.Equals("")) {
                return GetAttributeValue(doc, queryStart, ancestorID, attributePath);
            }

            return "";
        }
    }
}
