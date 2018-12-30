using SevenDaysXMLParser.source;
using SevenDaysXMLParser.source.Quests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SevenDaysBasicXMLParser.Quests
{
    public class QuestsXml
    {
        public static Dictionary<string, Quest> quests = new Dictionary<string, Quest>();

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
                    if (node.Name.Equals("quest")) {
                        Quest quest = new Quest(node);
                        quests[quest.id] = quest;
                    }
                } catch (XmlException) {}
            }
        }
    }
}
