using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SevenDaysXMLParser.source.Quests {
    public class QuestRequirement {

        public readonly string type;
        public readonly string id;
        public readonly string value;
        public readonly bool hidden;
        public readonly bool optional;
        public readonly bool isChosen;
        public readonly bool isfixed;
        public readonly bool isChainReward;
        public readonly ReceiveStage stage;
        public readonly Dictionary<string, QuestRequirement> childRequirements = new Dictionary<string, QuestRequirement>();

        public enum ReceiveStage {
            None,
            QuestStart,
            QuestCompletion,
            AfterCompleteNotification
        }

        internal QuestRequirement(XmlElement e) {
            if (!e.HasAttribute("type")) {
                throw new XmlException("QuestRequirement does not have a type assigned!");
            }

            type = e.GetAttribute("type");
            id = e.GetAttribute("id");
            value = e.GetAttribute("value");
            hidden = (bool)Utils.GetAttribute<bool>(e, "hidden", false);
            optional = (bool)Utils.GetAttribute<bool>(e, "optional", false);
            isChosen = (bool)Utils.GetAttribute<bool>(e, "isChosen", false);
            isfixed = (bool)Utils.GetAttribute<bool>(e, "isfixed", false);
            isChainReward = (bool)Utils.GetAttribute<bool>(e, "isChainReward", false);
            Enum.TryParse(e.GetAttribute("stage"), out stage);

            if (type.Equals("group")) {
                foreach (XmlNode child in e.ChildNodes) {
                    try {
                        if (child.Name.Equals("requirement")) {
                            QuestRequirement requirement = new QuestRequirement(child as XmlElement);
                            childRequirements[requirement.type] = requirement;
                        }
                    } catch (XmlException) {}
                }
            }
        }
    }
}
