using SevenDaysXMLParser.source;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SevenDaysBasicXMLParser.source.Quests {
    public class Quest {

        public readonly string id;

        public readonly int objectiveCount;

        internal Quest(XmlElement e) {
            if (!e.HasAttribute("id")) {
                throw new XmlException("Quest does not have id assigned!");
            }

            id = e.GetAttribute("id");

            foreach(XmlElement child in e.ChildNodes) {
                if (child.Name.Equals("objective")) {
                        ++objectiveCount;
                }
            }
        }
    }
}
