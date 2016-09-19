using SevenDaysSaveManipulator;
using SevenDaysSaveManipulator.GameData;

namespace SevenDaysProfileEditor.Skills
{
    internal class Requirement
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

        public bool IsRequirementMet()
        {
            return requiredSkillCurrentLevel.Get() >= requiredSkillLevel;
        }
    }
}