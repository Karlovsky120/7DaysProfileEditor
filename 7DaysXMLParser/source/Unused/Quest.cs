using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SevenDaysXMLParser.source.Quests {
    public class Quest {

        public readonly string id;
        public readonly string groupName;
		public readonly string name;
		public readonly string subtitle;
		public readonly string description;
		public readonly string icon;
		public readonly string category;
		public readonly string offer;
        public readonly string statement;
        public readonly string response;
        public readonly string completion;

		public readonly bool shareable = true;
        public readonly bool repeatable = false;
        public readonly byte currentVersion = 0;
		public readonly QuestDifficulty difficulty;
        public readonly CompletionTypes completionType;

        public readonly Dictionary<string, QuestAction> actions = new Dictionary<string, QuestAction>();
        public readonly Dictionary<string, QuestRequirement> requirements = new Dictionary<string, QuestRequirement>();
        public readonly Dictionary<string, QuestObjective> objectives = new Dictionary<string, QuestObjective>();
        public readonly Dictionary<string, QuestRewards> rewards = new Dictionary<string, QuestRewards>();
        public readonly Dictionary<string, QuestCriteria> criteria = new Dictionary<string, QuestCriteria>();

        public enum QuestDifficulty {
            none,
            veryeasy,
            easy,
            medium,
            hard,
            insane
        }

        public enum CompletionTypes {
            AutoComplete,
            TurnIn
        }

        internal Quest(XmlElement e) {
            if (!e.HasAttribute("id")) {
                throw new XmlException("Quest does not have id assigned!");
            }

            id = e.GetAttribute("id");
            groupName = e.GetAttribute("group_name_key");
            name = e.GetAttribute("name_key");
            subtitle = e.GetAttribute("subtitle_key");
            description = e.GetAttribute("description_key");
            icon = e.GetAttribute("icon");
            category = e.GetAttribute("category_key");
            offer = e.GetAttribute("offer_key");
            statement = e.GetAttribute("statement_key");
            response = e.GetAttribute("response_key");
            completion = e.GetAttribute("completion_key");
            shareable = (bool)Utils.GetAttribute<bool>(e, "shareable", true);
            repeatable = (bool)Utils.GetAttribute<bool>(e, "repeatable", false);
            currentVersion = (byte)Utils.GetAttribute<byte>(e, "currentVersion", 0);
            Enum.TryParse(e.GetAttribute("difficulty"), out difficulty);
            Enum.TryParse(e.GetAttribute("completiontype"), out completionType);

            foreach(XmlElement child in e.ChildNodes) {
                try {
                    switch (child.Name) {
                        case "action":
                            QuestAction action = new QuestAction(child);
                            actions[action.id] = action;
                            break;
                        case "requirement":
                            QuestRequirement requirement = new QuestRequirement(child);
                            requirements[requirement.id] = requirement;
                            break;
                        case "objective":
                            QuestObjective objective = new QuestObjective(child);
                            objectives[objective.id] = objective;
                            break;
                        case "reward":
                            QuestRewards reward = new QuestRewards(child);
                            rewards[reward.id] = reward;
                            break;
                        case "quest_criteria":
                        case "offer_criteria":
                            QuestCriteria criterium = new QuestCriteria(child);
                            criteria[criterium.id] = criterium;
                            break;
                        default:
                            break;
                    }
                } catch (XmlException) {}
            }
        }
    }
}
