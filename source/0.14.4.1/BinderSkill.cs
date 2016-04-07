using SevenDaysSaveManipulator;
using SevenDaysSaveManipulator.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysProfileEditor.Skills
{
    class BinderSkill
    {
        public SkillCollection skillCollection;
                
        public Skill skill;
        public DataSkill dataSkill;

        public Value<int> id;
        public Value<int> expToNextLevel;
        public Value<bool> isLocked;
        public Value<int> level;

        public SkillType type;
        public string name;
        public int maxLevel;
        public int expToLevel;

        public Value<int> maxLevelUnlocked = new Value<int>(1);
        public List<Tuple<string, int>> requirements;
        public List<BinderSkill> listeners = new List<BinderSkill>();
        public SlotSkill skillSlot;
        public List<BinderSkill> playerListeners;

        public void generateListeners()
        {
            foreach (Tuple<string, int> requirement in requirements)
            {
                if (requirement.Item1.Equals("Player Level"))
                {
                    if (!playerListeners.Contains(this))
                    {
                        playerListeners.Add(this);
                    }
                }

                else if (!requirement.Item1.Equals("Player Level"))
                {
                    int id = Utils.getMonoHash(requirement.Item1);
                    BinderSkill binderSkill = skillCollection.getBinderSkillById(id);

                    if (!binderSkill.listeners.Contains(this))
                    {
                        binderSkill.listeners.Add(this);
                    }
                }
            }
        }

        public void notifyListeners()
        {
            foreach (BinderSkill binderSkill in listeners)
            {
                binderSkill.update();
            }
        }

        public void update()
        {
            for (int i = requirements.Count-1; i >= 0; i--)
            {
                int skillLevel;
                int unlockLevel;

                if (!requirements[i].Item1.Equals("Player Level"))
                {
                    int id = Utils.getMonoHash(requirements[i].Item1);
                    BinderSkill binderSkill = skillCollection.getBinderSkillById(id);

                    skillLevel = binderSkill.level.get();
                    unlockLevel = requirements[i].Item2;
                }

                else
                {
                    skillLevel = skillCollection.playerLevel.get();
                    unlockLevel = requirements[i].Item2;
                }

                if (skillLevel >= unlockLevel)
                {
                    maxLevelUnlocked.set(i + 1);
                    isLocked.set(false);

                    if (level.get() > maxLevelUnlocked.get())
                    {
                        level.set(maxLevelUnlocked.get());
                    }

                    if (skillSlot != null)
                    {
                        skillSlot.update();
                    }

                    return;
                }
            }

            maxLevelUnlocked.set(0);
            isLocked.set(true);

            if (skillSlot != null)
            {
                skillSlot.update();
            }
        }

        public bool getNextRequirement(out string skill, out int level)
        {
            if (maxLevelUnlocked.get() == requirements.Count)
            {
                skill = "None";
                level = 0;
                return false;
            }

            Tuple<string, int> requirement = requirements[maxLevelUnlocked.get()];
            skill = requirement.Item1;
            level = requirement.Item2;
            return true;
        }

        public void updateSkill(Skill skill)
        {
            this.skill = skill;
            id = skill.id;
            expToNextLevel = skill.expToNextLevel;
            isLocked = skill.isLocked;
            level = skill.level;
        }

        private void populateSkill(Skill skill)
        {
            skill.id = new Value<int>(Utils.getMonoHash(name));
            skill.expToNextLevel = new Value<int>(expToLevel);
            skill.isLocked = new Value<bool>(true);
            if (type == SkillType.Perk)
            {
                skill.level = new Value<int>(0);
            }

            else
            {
                skill.level = new Value<int>(1);
            }

            if (!skillCollection.skillDictionary.ContainsKey(skill.id.get()))
            {
                skillCollection.skillDictionary.Add(skill.id.get(), skill);
            }
        }

        private void bindSkill(Skill skill)
        {
            this.skill = skill;
            id = skill.id;
            expToNextLevel = skill.expToNextLevel;
            isLocked = skill.isLocked;
            level = skill.level;
        }

        private void bindDataSkill(DataSkill dataSkill)
        {
            this.dataSkill = dataSkill;
            type = dataSkill.type;
            name = dataSkill.name;
            maxLevel = dataSkill.maxLevel;
            expToLevel = dataSkill.expToLevel;
            requirements = dataSkill.requirements;
            maxLevelUnlocked = new Value<int>(maxLevel);
        }

        public BinderSkill(DataSkill dataSkill, SkillCollection skillCollection, List<BinderSkill> playerListeners)
        {
            this.skillCollection = skillCollection;
            this.playerListeners = playerListeners;

            bindDataSkill(dataSkill);

            skill = new Skill();
            populateSkill(skill);
            bindSkill(skill);          
        }

        public static bool isDefaultValues(Skill skill)
        {
            DataSkill dataSkill = DataSkill.getDataSkillById(skill.id.get());

            bool flag;
            if (skill.isLocked.get())
            {                
                flag = (skill.level.get() == 0);
            }

            else
            {
                flag = false;
            }

            bool flag2;
            if (skill.level.get() == 1)
            {                
                flag2 = !(dataSkill.type == SkillType.Perk);
            }

            else
            {
                flag2 = false;
            }

            bool flag3 = skill.expToNextLevel.get() == dataSkill.expToLevel;

            if (!flag)
            {
                if (!skill.isLocked.get())
                {                   
                    if (flag2)
                    {                        
                        return flag3;
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
