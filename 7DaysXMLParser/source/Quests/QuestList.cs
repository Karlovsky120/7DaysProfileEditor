using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SevenDaysXMLParser.source.Quests {
    public class QuestList {

        public readonly string id;
        public readonly List<string> questList = new List<string>();

        internal QuestList(XmlElement e) {

            if (!e.HasAttribute("id")) {
                throw new XmlException("QuestList does not have an id assigned!");
            }

            id = e.GetAttribute("id");

            foreach (XmlElement child in e.ChildNodes) {
                if (child.Name.Equals("quest")) {
                    questList.Add((child.GetAttribute("id"));
                }
            }
        }
    }
}
