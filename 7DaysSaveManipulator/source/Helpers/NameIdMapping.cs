using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace SevenDaysSaveManipulator {

    /* 
     * ID 0             => Air
     * ID [1,256]       => terrain blocks (attribute Shape=Terrain)
     * ID [257,32768]   => other blocks
     * ID [32786,65536] => items directly followed by itemModifiers
    */

    public class NameIdMapping {

        private static readonly int TERRAIN_MAX = 256;
        private static readonly int BLOCK_MAX = 32768;
        private static readonly int ITEM_MAX = BLOCK_MAX * 2;

        public Dictionary<int, string> initialIdToName = new Dictionary<int, string>();
        public Dictionary<string, int> initialNameToId = new Dictionary<string, int>();
        public SortedDictionary<int, string> idToName;
        public Dictionary<string, int> nameToId;

        private XmlData xmlData;

        internal NameIdMapping(string blockMappingsPath, string itemMappingsPath, string blocksXmlPath, string itemsXmlPath, string itemModificationsXmlPath, XmlData xmlData) {
            this.xmlData = xmlData;
            ReadMappingsFile(blockMappingsPath);
            ReadMappingsFile(itemMappingsPath);

            idToName = new SortedDictionary<int, string>(initialIdToName);
            nameToId = new Dictionary<string, int>(initialNameToId);

            int currentId = 0;
            GenerateRestOfTheMappings(blocksXmlPath, ref currentId);
            currentId = BLOCK_MAX;
            GenerateRestOfTheMappings(itemsXmlPath, ref currentId);
            GenerateRestOfTheMappings(itemModificationsXmlPath, ref currentId);
        }

        private void GenerateRestOfTheMappings(string xmlPath, ref int startId) {
            XmlDocument document = new XmlDocument();
            document.Load(xmlPath);

            bool doneWithTerrain = false;
            foreach (XmlNode element in document.DocumentElement.ChildNodes.Cast<XmlNode>().Where(node => node.NodeType != XmlNodeType.Comment)) {
                string name = ((XmlElement)element).GetAttribute("name");
                if (!nameToId.ContainsKey(name)) {
                    if (document.DocumentElement.Name.Equals("blocks") && !doneWithTerrain) {
                        string shape = xmlData.GetBlockShape(name);
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
                Utils.VerifyVersion(reader.ReadInt32(), SaveVersionConstants.NAME_ID_MAPPING);

                int count = reader.ReadInt32();
                for (int i = 0; i < count; ++i) {
                    int id = reader.ReadInt32();
                    string name = reader.ReadString();
                    initialIdToName[id] = name;
                    initialNameToId[name] = id;
                }
            }
        }

        private void WriteMappingFile(string mappingFilePath, int startId, int endId) {
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

        internal void WriteToFiles(string blockMappingsPath, string itemMappingsPath) {
            WriteMappingFile(blockMappingsPath, 0, BLOCK_MAX);
            WriteMappingFile(itemMappingsPath, BLOCK_MAX, ITEM_MAX);
        }
    }
}
