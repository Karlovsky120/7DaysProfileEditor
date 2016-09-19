using SevenDaysProfileEditor.Data;
using SevenDaysSaveManipulator;
using System.Collections.Generic;

namespace SevenDaysProfileEditor.Skills
{
    internal enum SkillType
    {
        PlayerSkill,
        ActionSkill,
        CraftingSkill,
        Perk
    }

    internal class SkillData
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
        public Dictionary<string, int> recipes;
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