using SevenDaysSaveManipulator.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysProfileEditor.Quests
{
    class CollectionQuest
    {
        public List<BinderQuest> binderQuests;
        public QuestJournal questJournal;

        public CollectionQuest(QuestJournal questJournal)
        {
            this.questJournal = questJournal;
            binderQuests = new List<BinderQuest>();

            foreach (Quest quest in questJournal.quests)
            {
                binderQuests.Add(new BinderQuest(quest));
            }
        }
    }
}
