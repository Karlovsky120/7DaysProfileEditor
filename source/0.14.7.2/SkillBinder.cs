using SevenDaysProfileEditor.GUI;
using SevenDaysSaveManipulator;
using SevenDaysSaveManipulator.GameData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysProfileEditor.Skills
{
    class SkillBinder
    {
        public static Dictionary<int, Skill> skillDictionary;

        public Skill skill;
        public SkillData skillData;

        public Value<int> id;
        public Value<int> expToNextLevel;
        public Value<bool> isLocked;
        public Value<int> level;

        public SkillType type;
        public string name;
        public int maxLevel;
        public int expToLevel;
        public UIIconData iconData;

        public List<Requirement> requirements;

        public Value<int> playerLevel;


        public SkillBinder(Skill skill, Value<int> playerLevel)
        {
            this.skill = skill;

            id = skill.id;
            expToNextLevel = skill.expToNextLevel;
            isLocked = skill.isLocked;
            level = skill.level;

            skillData = SkillData.GetSkillDataById(id.Get());

            type = skillData.type;
            name = skillData.name;
            maxLevel = skillData.maxLevel;
            expToLevel = skillData.expToLevel;
            iconData = skillData.iconData;
            requirements = skillData.requirements;

            if (requirements != null)
            {
                foreach (Requirement requirement in requirements)
                {
                    if (requirement.requiredSkillName.Equals("Player Level"))
                    {
                        requirement.requiredSkillCurrentLevel = playerLevel;
                    }

                    else
                    {
                        requirement.requiredSkillCurrentLevel = SkillBinder.GetSkillByName(requirement.requiredSkillName).level;
                    }
                }
            }

            this.playerLevel = playerLevel;
        }

        public int GetHighestUnlockedLevel()
        {
            if (requirements.Count > 0)
            {
                bool isLevelUnlocked = true;
                int unlockedLevel = -1;

                do
                {
                    unlockedLevel++;

                    foreach (Requirement requirement in GetRequirementByLevel(unlockedLevel + 1))
                    {
                        isLevelUnlocked = requirement.IsRequirementSatisfied();
                    }

                } while (isLevelUnlocked && (unlockedLevel < maxLevel));

                return unlockedLevel;
            }

            return maxLevel;
        }

        public List<Requirement> GetRequirementByLevel(int level)
        {
            if (requirements.Count > 0)
            {
                List<Requirement> requirementsByLevel = new List<Requirement>();

                foreach (Requirement requirement in requirements)
                {
                    if (requirement.perkLevel == level)
                    {
                        requirementsByLevel.Add(requirement);
                    }
                }

                return requirementsByLevel;
            }

            return null;
        }

        public static Skill GetSkillByName(string name)
        {
            Skill skill;

            skillDictionary.TryGetValue(Utils.GetMonoHash(name), out skill);

            return skill;
        }

        public static Skill GetEmptySkill(int id, PlayerDataFile playerDataFile)
        {
            Skill skill = new Skill();

            skill.skillVersion = new Value<int>(2);

            skill.id = new Value<int>(id);
            skill.level = new Value<int>(0);
            skill.parent = playerDataFile.skills;

            SkillData skillData = SkillData.GetSkillDataById(id);

            skill.isLocked = new Value<bool>(false);

            if (skillData.requirements.Count > 0)
            {
                foreach (Requirement requirement in skillData.requirements)
                {
                    if (requirement.perkLevel == 1)
                    {
                        String skillName = requirement.requiredSkillName;
                        int requiredSkillLevel = 0;

                        if (skillName.Equals("Player Level"))
                        {
                            requiredSkillLevel = playerDataFile.level.Get();
                        }

                        else
                        {
                            int skillId = requirement.requiredSkillId;
                            Skill requiredSkill;

                            if (skill.parent.skillDictionary.TryGetValue(skillId, out requiredSkill))
                            {
                                requiredSkillLevel = requiredSkill.level.Get();
                            }
                        }
                        
                        if (requirement.requiredSkillLevel > requiredSkillLevel)
                        {
                            skill.isLocked.Set(true);
                            return skill;
                        }
                    }
                }
            }

            return skill;
        }

        public static void SetSkillDicitionary(PlayerDataFile playerDataFile)
        {
            skillDictionary = playerDataFile.skills.skillDictionary;
        }

        public Bitmap GetImage()
        {
            if (iconData.data != null)
            {
                Bitmap bmp = new Bitmap(iconData.width, iconData.height);

                PixelFormat pxf = PixelFormat.Format32bppArgb;
                BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, pxf);
                IntPtr ptr = bmpData.Scan0;
                Marshal.Copy(iconData.data, 0, ptr, iconData.width * iconData.height * 4);
                bmp.UnlockBits(bmpData);

                return bmp;
            }

            return null;
        }


        public static bool IsDefaultValues(Skill skill)
        {
            SkillData skillData = SkillData.GetSkillDataById(skill.id.Get());

            bool flag;
            if (skill.isLocked.Get())
            {
                flag = (skill.level.Get() == 0);
            }

            else
            {
                flag = false;
            }

            bool flag2;
            if (skill.level.Get() == 1)
            {
                flag2 = !(skillData.type == SkillType.Perk);
            }

            else
            {
                flag2 = false;
            }

            if (!flag)
            {
                if (!skill.isLocked.Get())
                {
                    if (flag2)
                    {
                        return skill.expToNextLevel.Get() == skillData.expToLevel;
                    }
                }

                return false;
            }

            else
            {
                return true;
            }
        }
    }
}