using SevenDaysSaveManipulator.SaveData;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace SevenDaysSaveManipulator {

    public class XmlData {

        private XmlDocument quests;
        private XmlDocument blocks;
        private XmlDocument items;
        private XmlDocument itemModifiers;
        private XmlDocument traders;

        private Dictionary<string, int> questObjectiveCount = new Dictionary<string, int>();
        private Dictionary<string, int> questRewardCount = new Dictionary<string, int>();
        private Dictionary<string, string> blockShape = new Dictionary<string, string>();
        private Dictionary<int, bool> isItemModifier = new Dictionary<int, bool>();
        private Dictionary<int, bool> isRentable = new Dictionary<int, bool>();
        private Dictionary<string, int> questVersions = new Dictionary<string, int>();

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

        internal int GetObjectiveCount(string questID) {
            if (!questObjectiveCount.ContainsKey(questID)) {
                questObjectiveCount[questID] = CountXmlAttributes(quests, "quest:id@" + questID +"\\objective");
            }

            return questObjectiveCount[questID];
        }

        internal int GetRewardCount(string questID) {
            if (!questRewardCount.ContainsKey(questID)) {
                questRewardCount[questID] = CountXmlAttributes(quests, "quest:id@" + questID + "\\reward");
            }

            return questRewardCount[questID];
        }

        internal string GetBlockShape(string blockName) {
            if(!blockShape.ContainsKey(blockName)) {
                blockShape[blockName] = GetXmlAttributeValue(blocks, "block:name@" + blockName + "\\property:name@Shape&value");
            }

            return blockShape[blockName];
        }

        internal bool IsItemModifier(int itemID) {
            if (!isItemModifier.ContainsKey(itemID)) {
                isItemModifier[itemID] = XmlAttributeExists(itemModifiers, "item_modifier:name@" + nameIdMapping.idToName[itemID]);
            }

            return isItemModifier[itemID];
        }

        /*internal bool HasQuality(int itemID) {
            if (!hasQuality.ContainsKey(itemID)) {
                hasQuality[itemID] = GetMatchingXmlElements(items, "item:name@" + nameIdMapping.idToName[itemID] + "\\effect_group\\passive_effect:name@DegradationMax") > 0;
            }

            return hasQuality[itemID];
        }*/

        internal bool IsRentable(int traderID) {
            if (!isRentable.ContainsKey(traderID)) {
                isRentable[traderID] = XmlAttributeExists(traders, "trader_info:id@" + traderID + "&rentable@true");
            }

            return isRentable[traderID];
        }

        internal int GetCurrentQuestVersion(string questID) {
            if (!questVersions.ContainsKey(questID)) {
                string currentQuestVersion = GetXmlAttributeValue(quests, "quest:id@" + questID + "\\property:name@currentVersion&value", true);
                questVersions[questID] = currentQuestVersion == "" ? 0 : int.Parse(currentQuestVersion);
            }

            return questVersions[questID];
        }

        /// <summary>
        /// Returns list of XmlElements that fit the given <paramref name="path"/>
        /// </summary>
        /// <param name="xml">Xml to look for the attribute in</param>
        /// <param name="path">The path to the parameter. It must be formatted as
        /// "element:atrribute@attributeValue&attribute@attributeValue\element\element:atrribute@attributeValue\element:attribute@attributeValue".
        /// There can be any number of levels (between \). First element in the list must be a child of the root node. Number of attributes per element can be any (including none).</param>
        /// <returns>Returns list of  XmlElements</returns>
        private List<XmlElement> GetMatchingXmlElements(XmlDocument xml, string path, bool ignoreCase = false) {
            List<XmlElement> currentElements = new List<XmlElement> { xml.DocumentElement };

            string[] elementLevels = path.Split('\\');

            bool elementFound = true;
            foreach (string elementLevel in elementLevels) {
                List<XmlElement> newElements = new List<XmlElement>();
                foreach (XmlElement currentElement in currentElements) {
                    newElements.AddRange(CheckMatchingOnCurrentLevel(currentElement, elementLevel, ignoreCase));
                }

                if (newElements.Count == 0) {
                    elementFound = false;
                    break;
                }

                currentElements = newElements;
            }

            if (elementFound) {
                return currentElements;
            }

            //Extension cannot be inherited
            if (!path.Contains("\\property:name@Extends")) {

                //Check if any of the elements this item extends have the value we need
                string extensionPath = elementLevels[0] + "\\property:name@Extends&value";
                string baseItem = GetXmlAttributeValue(xml, extensionPath);

                if (!baseItem.Equals("")) {
                    string baseItemPath = elementLevels[0].Split(':')[0] + ":name@" + baseItem;
                    for (int i = 1; i < elementLevels.Length; ++i) {
                        baseItemPath += "\\" + elementLevels[i];
                    }

                    return GetMatchingXmlElements(xml, baseItemPath);
                }
            }

            return new List<XmlElement>();
        }

        /// <summary>
        /// Returns the childern of <paramref name="currentElement"/> that fit the given <<paramref name="data"/>
        /// <param name="currentElement">XmlElements whose childern are being checked</param>
        /// <param name="data">Data to match XmlElements against</param>
        /// <returns>Returns a List od XmlElements</returns>
        private List<XmlElement> CheckMatchingOnCurrentLevel(XmlElement currentElement, string data, bool ignoreCase = false) {
            string[] elementLevelData = data.Split(':');
            string elementName = elementLevelData[0];
            XmlNodeList candidates = currentElement.GetElementsByTagName(elementName);

            //if no elements under requested name exist
            if (candidates.Count == 0) {
                return new List<XmlElement>();
            }

            if (elementLevelData.Length == 1) {
                return new List<XmlElement>(candidates.Cast<XmlElement>().ToArray());
            }

            string[] attributeConstraints = elementLevelData[1].Split('&');
            List<string[]> attributeKeyValuePairs = new List<string[]>();
            foreach (string attributeConstraint in attributeConstraints) {
                attributeKeyValuePairs.Add(attributeConstraint.Split('@'));
            }

            return ParseElementLevel(candidates, attributeKeyValuePairs, ignoreCase);
        }

        /// <summary>
        /// Returns list of XmlElements that fit the given <paramref name="attributeKeyValuePairs"/>
        /// </summary>
        /// <param name="candidates">List of possible XmlElements</param>
        /// <param name="attributeKeyValuePairs">List of attribute constraints</param>
        /// <returns>Returns list of requested XmlElements</returns>
        private List<XmlElement> ParseElementLevel(XmlNodeList candidates, List<string[]> attributeKeyValuePairs, bool ignoreCase = false) {
            List<XmlElement> validElements = new List<XmlElement>();
            foreach (XmlElement candidate in candidates) {
                foreach (string[] attributeKeyValuePair in attributeKeyValuePairs) {
                    if (candidate.HasAttribute(attributeKeyValuePair[0]) &&
                        (ignoreCase && candidate.GetAttribute(attributeKeyValuePair[0]).ToLower().Equals(attributeKeyValuePair[1].ToLower()) ||
                        !ignoreCase && candidate.GetAttribute(attributeKeyValuePair[0]).Equals(attributeKeyValuePair[1]))) {
                        validElements.Add(candidate);
                    }
                }
            }

            if (validElements.Count != 0) {
                return validElements;
            }

            return new List<XmlElement>();
        }

        /// <summary>
        /// Return string value of the first attribute encountered in given xml specified in <paramref name="path"/>
        /// </summary>
        /// <param name="xml">Xml to look for the attribute in</param>
        /// <param name="path">The path to the parameter. It must be formatted as
        /// "element:atrribute@attributeValue&attribute@attributeValue\element\element:atrribute@attributeValue\element:attribute@attributeValue".
        /// There can be any number of levels (between \). First element in the list must be a child of the root node. Number of attributes per element can be any (including none).
        /// Final element must end with "attribute" with no value specified ("element:attribute", at minimum) where attribute's string value will be returned.</param>
        /// <returns>Returns value of the requested attribute, or empty string if one does not exist</returns>
        private string GetXmlAttributeValue(XmlDocument xml, string path, bool ignoreCase = false) {
            //Get the attribute name in which were are interested
            int delimiterIndex = path.LastIndexOf('&');
            if (delimiterIndex == -1) {
                delimiterIndex = path.LastIndexOf(":");
            }

            string truncatedPath = path.Substring(0, delimiterIndex);
            string requestedAttribute = path.Substring(delimiterIndex + 1);

            List<XmlElement> elements = GetMatchingXmlElements(xml, truncatedPath, ignoreCase);
            if (elements.Count == 0) {
                return "";
            }

            return elements[0].GetAttribute(requestedAttribute);
        }

        /// <summary>
        /// Returns true if element that fits parameters given in <paramref name="path"/> exists.
        /// </summary>
        /// <param name="xml">Xml to look for the attribute in</param>
        /// <param name="path">The path to the parameter. It must be formatted as
        /// "element:atrribute@attributeValue&attribute@attributeValue\element\element:atrribute@attributeValue\element:attribute@attributeValue".
        /// There can be any number of levels (between \). First element in the list must be a child of the root node. Number of attributes per element can be any (including none).
        /// Final element must end with "attribute" with no value specified ("element:attribute", at minimum).</param>
        /// <returns>Returns true if element that fits parameters given in <paramref name="path"/> exists</returns>
        private bool XmlAttributeExists(XmlDocument xml, string path) {
            return GetMatchingXmlElements(xml, path).Count != 0;
        }

        private int CountXmlAttributes(XmlDocument xml, string path) {
            return GetMatchingXmlElements(xml, path, true).Count;
        }
    }
}
