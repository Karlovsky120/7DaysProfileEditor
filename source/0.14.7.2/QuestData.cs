using System.Collections.Generic;

namespace SevenDaysProfileEditor.Quests
{
    class QuestData
    {
        public static List<QuestData> questList = new List<QuestData>();

        public string id;
        public string categoryKey;
        public List<ObjectiveData> objectives;

        public static QuestData GetQuestById(string id)
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
