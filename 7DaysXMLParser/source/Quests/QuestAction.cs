using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SevenDaysXMLParser.source.Quests {
    public class QuestAction {

        public readonly string type;
        public readonly string id;
        public readonly string value;
        public readonly byte phase;

        internal QuestAction(XmlElement e) {
            if (!e.HasAttribute("type")) {
                throw new XmlException("QuestAction does not have a type assigned!");
            }

            type = e.GetAttribute("type");
            id = e.GetAttribute("id");
            value = e.GetAttribute("value");
            phase = (byte)Utils.GetAttribute<byte>(e, "phase", 0);
        }
    }
}
