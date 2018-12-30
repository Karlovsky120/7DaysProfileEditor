using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SevenDaysXMLParser.source.Items {
    class ItemsXml {

        public static Dictionary<string, Item> items = new Dictionary<string, Item>();

        public ItemsXml(string path) {
            Load(path);
        }

        public void Load(string path) {
            Read(Utils.LoadXml(path));
        }

        private void Read(XmlDocument questsXml) {
            if (questsXml.ChildNodes.Count < 1) {
                throw new XmlException("Quests.xml root node not found!");
            }

            foreach (XmlElement node in questsXml.ChildNodes) {
                try {
                    if (node.Name.Equals("quest")) {
                        Item item = new Item(node);
                        items[item.name] = item;
                    }
                } catch (XmlException) {}
            }

            foreach (KeyValuePair<string, Item> entry in items) {
                if (!entry.Value.parentName.Equals("")) {

                }
            }
        }
    }
}
