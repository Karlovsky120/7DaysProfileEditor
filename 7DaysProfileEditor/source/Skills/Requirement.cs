using SevenDaysSaveManipulator;
using SevenDaysSaveManipulator.PlayerData;

namespace SevenDaysProfileEditor.Skills {

    /// <summary>
    /// Holds requirement for unlockin a perk.
    /// </summary>
    public class Requirement {
        public int perkLevel;
        public Value<int> requiredSkillCurrentLevel;
        public int requiredSkillId;
        public int requiredSkillLevel;
        public string requiredSkillName;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="perkLevel">Level of the perk to unlock</param>
        /// <param name="requiredSkillName">Name of the required skill</param>
        /// <param name="requiredSkillLevel">Level of the required skill</param>
        public Requirement(int perkLevel, string requiredSkillName, int requiredSkillLevel) {
            this.perkLevel = perkLevel;
            this.requiredSkillId = Utils.GetMonoHash(requiredSkillName);
            this.requiredSkillName = requiredSkillName;
            this.requiredSkillLevel = requiredSkillLevel;
        }

        /// <summary>
        /// Checks if the requirement is met.
        /// </summary>
        /// <returns>True if requirement is met, false otherwise</returns>
        public bool IsRequirementMet() {
            return requiredSkillCurrentLevel.Get() >= requiredSkillLevel;
        }
    }
}