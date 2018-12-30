using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SevenDaysXMLParser.source.Quests {
    public class QuestTierReward {

        public readonly string id;
        public readonly string type;

        internal QuestTierReward(XmlElement e ) {
            if (!e.HasAttribute("type")) {
                throw new XmlException("QuestTierReward does not have a type assigned!");
            }

            type = e.GetAttribute("type");
            id = e.GetAttribute("id");
        }
    }
}
