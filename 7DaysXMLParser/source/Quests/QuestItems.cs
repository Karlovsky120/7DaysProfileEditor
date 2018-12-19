using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SevenDaysXMLParser.source.Quests {
    public class QuestItems {

        public readonly int max_count;
        public readonly List<QuestItem> items = new List<QuestItem>();

        internal QuestItems(XmlElement e) {
            max_count = (int)Utils.GetAttribute<int>(e, "max_count", 100);

            foreach (XmlElement child in e) {
                if (child.Name.Equals("quest_item")) {
                    try {
                        items.Add(new QuestItem(child));
                    } catch (XmlException) {}
                }
            }
        }
    }
}
