using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SevenDaysXMLParser.source.Quests {
    public class QuestItem {

        public readonly string name;
        public readonly string id;
        public readonly string description;
        public readonly string icon;
        public readonly string iconName;

        internal QuestItem(XmlElement e) {
            if (!e.HasAttribute("name")) {
                throw new XmlException("QuestItem does not have a name assigned!");
            }

            id = e.GetAttribute("id");
            description = e.GetAttribute("description");
            icon = e.GetAttribute("icon");
            iconName = e.GetAttribute("iconName");
        }
    }
}
