using SevenDaysProfileEditor.Data;
using SevenDaysProfileEditor.StatsAndGeneral;
using SevenDaysSaveManipulator;
using System.Collections.Generic;

namespace SevenDaysProfileEditor.Skills {

    /// <summary>
    /// Holds all static XML data about a skill along with dictionary of all the skills.
    /// </summary>
    internal class SkillData {
        public static int expToLevelDefault;
        public static int expToPlayerLevel;
        public static double experienceMultiplier;
        public static int maxLevelDefault;
        public static int maxPlayerLevel;
        public static List<SkillData> skillList = new List<SkillData>();
        public static List<string> skillRecipes = new List<string>();

        public int expToLevel;
        public UIIconData iconData;
        public int maxLevel;
        public string name;
        public List<RecipeData> recipes;
        public List<Requirement> requirements;
        public SkillType type;

        /// <summary>
        /// Returns skillData based on id
        /// </summary>
        /// <param name="id">Id of the skillData</param>
        /// <returns>skillData with requested id, null otherwise</returns>
        public static SkillData GetSkillDataById(int id) {
            foreach (SkillData skillData in SkillData.skillList) {
                if (Utils.GetMonoHash(skillData.name) == id) {
                    return skillData;
                }
            }

            return null;
        }
    }
}