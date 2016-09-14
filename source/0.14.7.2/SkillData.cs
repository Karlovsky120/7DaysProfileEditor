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

    class SkillData
    {
        public static List<SkillData> skillList = new List<SkillData>();

        public static int maxPlayerLevel;
        public static int expToPlayerLevel;

        public static int maxLevelDefault;
        public static int expToLevelDefault;

        public SkillType type;
        public string name;
        public int maxLevel;
        public int expToLevel;
        public UIIconData iconData;

        public List<Requirement> requirements;

        public static SkillData GetSkillDataById(int id)
        {
            foreach (SkillData skillData in SkillData.skillList)
            {
                if (Utils.GetMonoHash(skillData.name) == id)
                {
                    return skillData;
                }
            }

            return null;
        }
    }
}
