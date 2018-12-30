using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SevenDaysXMLParser.source.Quests {
    public class QuestCriteria {

        public readonly string type;
        public readonly string id;
        public readonly string value;

        internal QuestCriteria(XmlElement e) {
            if (!e.HasAttribute("type")) {
                throw new XmlException("QuestObjective does not have a type assigned!");
            }

            type = e.GetAttribute("type");
            id = e.GetAttribute("id");
            value = e.GetAttribute("value");
        }
    }
}
