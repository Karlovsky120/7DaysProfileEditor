using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysProfileEditor.Quests
{
    class QuestData
    {
        public static List<QuestData> questList = new List<QuestData>();

        public string id;
        public string categoryKey;
        public List<ObjectiveData> objectives;

        public static QuestData getQuestById(string id)
        {
            foreach (QuestData questData in QuestData.questList)
            {
                if (questData.id.ToLower().Equals(id.ToLower()))
                {
                    return questData;
                }
            }

            return null;
        }

    }
}
