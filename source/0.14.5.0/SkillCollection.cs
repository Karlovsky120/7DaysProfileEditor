using SevenDaysSaveManipulator;
using SevenDaysSaveManipulator.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysProfileEditor.Skills
{
    class SkillCollection
    {
        public List<BinderSkill> skillList = new List<BinderSkill>();
        public Dictionary<int, Skill> skillDictionary;
        public Value<int> playerLevel;
        public List<BinderSkill> playerListeners;

        public BinderSkill getBinderSkillById(int id)
        {
            foreach (BinderSkill binderSkill in skillList)
            {
                if (Utils.getMonoHash(binderSkill.name) == id)
                {
                    return binderSkill;
                }
            }

            return null;
        }

        private void setUpSkills(Dictionary<int, Skill> skillDictionary)
        {
            foreach (DataSkill dataSkill in DataSkill.skillList)
            {
                BinderSkill binderSkill = new BinderSkill(dataSkill, this, playerListeners);
                skillList.Add(binderSkill);
            }

            foreach (KeyValuePair<int, Skill> entry in skillDictionary)
            {
                BinderSkill binderSkill = getBinderSkillById(entry.Key);
                binderSkill.updateSkill(entry.Value);
            }

            foreach (BinderSkill binderSkill in skillList)
            {
                if (binderSkill.requirements != null)
                {
                    binderSkill.generateListeners();
                    binderSkill.update();
                }
            }
        }

        public SkillCollection(Dictionary<int, Skill> skillDictionary, Value<int> playerLevel, List<BinderSkill> playerListeners)
        {
            this.skillDictionary = skillDictionary;
            this.playerLevel = playerLevel;
            this.playerListeners = playerListeners;

            setUpSkills(skillDictionary);
        }
    }
}
