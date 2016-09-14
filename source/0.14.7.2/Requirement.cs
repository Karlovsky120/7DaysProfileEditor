using SevenDaysSaveManipulator;
using SevenDaysSaveManipulator.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysProfileEditor.Skills
{
    class Requirement
    {
        public int perkLevel;
        public int requiredSkillId;
        public string requiredSkillName;
        public int requiredSkillLevel;

        public Value<int> requiredSkillCurrentLevel;

        public Requirement(int perkLevel, string requiredSkillName, int requiredSkillLevel)
        {
            this.perkLevel = perkLevel;
            this.requiredSkillId = Utils.GetMonoHash(requiredSkillName);
            this.requiredSkillName = requiredSkillName;
            this.requiredSkillLevel = requiredSkillLevel;
        }

        public bool IsRequirementSatisfied()
        {
            return requiredSkillCurrentLevel.Get() >= requiredSkillLevel;
        }
    }
}
