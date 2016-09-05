using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysProfileEditor.Quests
{
    class DataQuest
    {
        public static List<DataQuest> questList = new List<DataQuest>();

        public string id;
        public string categoryKey;
        public List<DataObjective> objectives;

        public static DataQuest getQuestById(string id)
        {
            foreach (DataQuest dataQuest in DataQuest.questList)
            {
                if (dataQuest.id.ToLower().Equals(id.ToLower()))
                {
                    return dataQuest;
                }
            }

            return null;
        }

    }
}
