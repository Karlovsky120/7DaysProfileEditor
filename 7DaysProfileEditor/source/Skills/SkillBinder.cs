using SevenDaysProfileEditor.StatsAndGeneral;
using SevenDaysSaveManipulator;
using SevenDaysSaveManipulator.PlayerData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SevenDaysProfileEditor.Skills {

    /// <summary>
    /// Used as a binder between static data in xmls and dynamic data in the ttp file.
    /// </summary>
    internal class SkillBinder {
        public static Dictionary<int, Skill> skillDictionary;

        public Value<int> playerLevel;
        public Skill skill;
        public SkillData skillData;
        public List<RecipeBinder> unlockedRecipeList = new List<RecipeBinder>();

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="skill">Skill from the ttp</param>
        /// <param name="playerLevel">Player level</param>
        /// <param name="recipeBinderList">List of recepies</param>
        public SkillBinder(Skill skill, Value<int> playerLevel, List<RecipeBinder> recipeBinderList) {
            this.skill = skill;
            skillData = SkillData.GetSkillDataById(skill.id.Get());

            foreach (RecipeData recipeData in skillData.recipes) {
                foreach (RecipeBinder recipeBinder in recipeBinderList) {
                    if (recipeData == recipeBinder.recipeData) {
                        unlockedRecipeList.Add(recipeBinder);

                        int requiredSkillLevel = 499;
                        recipeData.skillUnlockConditions.TryGetValue(skillData, out requiredSkillLevel);

                        if (!recipeBinder.relevantSkillList.ContainsKey(this)) {
                            recipeBinder.relevantSkillList.Add(this, requiredSkillLevel);
                        }
                    }
                }
            }

            if (skillData.requirements != null) {
                foreach (Requirement requirement in skillData.requirements) {
                    if (requirement.requiredSkillName.Equals("Player Level")) {
                        requirement.requiredSkillCurrentLevel = playerLevel;
                    }
                    else {
                        requirement.requiredSkillCurrentLevel = SkillBinder.GetSkillByName(requirement.requiredSkillName).level;
                    }
                }
            }

            this.playerLevel = playerLevel;
        }

        /// <summary>
        /// Creates an empty skill.
        /// </summary>
        /// <param name="id">Id of the skill</param>
        /// <param name="playerDataFile">PlayerDataFile to use</param>
        /// <returns></returns>
        public static Skill GetEmptySkill(int id, PlayerDataFile playerDataFile) {
            Skill skill = new Skill() {
                skillVersion = new Value<int>(2),
                id = new Value<int>(id),
                isLocked = new Value<bool>(false),
                parent = playerDataFile.skills,
                level = new Value<int>(0)
            };

            SkillData skillData = SkillData.GetSkillDataById(id);

            skill.expToNextLevel = new Value<int>(skillData.expToLevel);

            if (skillData.requirements.Count > 0) {
                foreach (Requirement requirement in skillData.requirements) {
                    if (requirement.perkLevel == 1) {
                        String skillName = requirement.requiredSkillName;
                        int requiredSkillLevel = 0;

                        if (skillName.Equals("Player Level")) {
                            requiredSkillLevel = playerDataFile.level.Get();
                        }
                        else {
                            int skillId = requirement.requiredSkillId;
                            Skill requiredSkill;

                            if (skill.parent.skillDictionary.TryGetValue(skillId, out requiredSkill)) {
                                requiredSkillLevel = requiredSkill.level.Get();
                            }
                        }

                        if (requirement.requiredSkillLevel > requiredSkillLevel) {
                            skill.isLocked.Set(true);
                            return skill;
                        }
                    }
                }
            }

            return skill;
        }

        /// <summary>
        /// Returns skill by name.
        /// </summary>
        /// <param name="name">Name of the skill</param>
        /// <returns>Skill or null if skill not found</returns>
        public static Skill GetSkillByName(string name) {
            Skill skill;

            skillDictionary.TryGetValue(Utils.GetMonoHash(name), out skill);

            return skill;
        }

        /// <summary>
        /// Checks if holds the defalut values for the skill. Method extracted from game files and left undedited.
        /// </summary>
        /// <param name="skill">Skill to check</param>
        /// <returns>True if defaut, false othrewise</returns>
        public static bool IsDefaultValues(Skill skill) {
            SkillData skillData = SkillData.GetSkillDataById(skill.id.Get());

            bool flag;
            if (skill.isLocked.Get()) {
                flag = (skill.level.Get() == 0);
            }
            else {
                flag = false;
            }

            bool flag2;
            if (skill.level.Get() == 1) {
                flag2 = !(skillData.type == SkillType.Perk);
            }
            else {
                flag2 = false;
            }

            if (!flag) {
                if (!skill.isLocked.Get()) {
                    if (flag2) {
                        return skill.expToNextLevel.Get() == skillData.expToLevel;
                    }
                }

                return false;
            }
            else {
                return true;
            }
        }

        /// <summary>
        /// Binds skill dictionary to playerDataFile.
        /// </summary>
        /// <param name="playerDataFile">PlayerDataFile to bind skill dictionary to</param>
        public static void SetSkillDicitionary(PlayerDataFile playerDataFile) {
            skillDictionary = playerDataFile.skills.skillDictionary;
        }

        /// <summary>
        /// Get highest unlocked perk level possible based on its requirements.
        /// </summary>
        /// <returns>Highest unlocked level possible</returns>
        public int GetHighestUnlockedLevel() {
            if (skillData.requirements.Count > 0) {
                bool isLevelUnlocked = true;
                int unlockedLevel = -1;

                do {
                    unlockedLevel++;

                    foreach (Requirement requirement in GetRequirementByLevel(unlockedLevel + 1)) {
                        isLevelUnlocked = requirement.IsRequirementMet();
                    }
                } while (isLevelUnlocked && (unlockedLevel < skillData.maxLevel));

                return unlockedLevel;
            }

            return skillData.maxLevel;
        }

        /// <summary>
        /// Creates image for byte array containing the image pixel data.
        /// </summary>
        /// <returns>Bitmap image</returns>
        public Bitmap GetImage() {
            if (skillData.iconData.data != null) {
                Bitmap bmp = new Bitmap(skillData.iconData.width, skillData.iconData.height);

                PixelFormat pxf = PixelFormat.Format32bppArgb;
                BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, pxf);
                IntPtr ptr = bmpData.Scan0;
                Marshal.Copy(skillData.iconData.data, 0, ptr, skillData.iconData.width * skillData.iconData.height * 4);
                bmp.UnlockBits(bmpData);

                return bmp;
            }

            return null;
        }

        /// <summary>
        /// Gets all requirements to unlock the specified level
        /// </summary>
        /// <param name="level">Specified level</param>
        /// <returns>List of requirements to unlock the specified level</returns>
        public List<Requirement> GetRequirementByLevel(int level) {
            if (skillData.requirements.Count > 0) {
                List<Requirement> requirementsByLevel = new List<Requirement>();

                foreach (Requirement requirement in skillData.requirements) {
                    if (requirement.perkLevel == level) {
                        requirementsByLevel.Add(requirement);
                    }
                }

                return requirementsByLevel;
            }

            return null;
        }
    }
}