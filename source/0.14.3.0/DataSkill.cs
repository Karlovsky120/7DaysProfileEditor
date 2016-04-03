using SevenDaysSaveManipulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysProfileEditor.Skills
{
    enum SkillType
    {
        PlayerSkill,
        ActionSkill,
        CraftingSkill,
        Perk
    }

    class DataSkill
    {
        public static List<DataSkill> skillList = new List<DataSkill>();

        public static int maxPlayerLevel;
        public static int expToPlayerLevel;

        public static int maxLevelDefault;
        public static int expToLevelDefault;

        public SkillType type;
        public string name;
        public int maxLevel;
        public int expToLevel;
        
        public List<Tuple<string, int>> requirements;

        public static DataSkill getDataSkillById(int id)
        {
            foreach (DataSkill dataSkill in DataSkill.skillList)
            {
                if (Utils.getMonoHash(dataSkill.name) == id)
                {
                    return dataSkill;
                }
            }

            return null;
        }
    }
}
