using SevenDaysProfileEditor.Skills;
using SevenDaysSaveManipulator.PlayerData;
using System.Collections.Generic;

namespace SevenDaysProfileEditor.StatsAndGeneral {

    /// <summary>
    /// Used as a binder between static data in xmls and dynamic data in the ttp file.
    /// </summary>
    internal class RecipeBinder {
        public RecipeData recipeData;
        public Dictionary<SkillBinder, int> relevantSkillList = new Dictionary<SkillBinder, int>();
        public Value<bool> unlocked;
        private List<string> unlockedRecipesList;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="recipeData">RecipeData to use</param>
        /// <param name="unlockedRecipeList">List of unlocked recipes from playerDataFile</param>
        public RecipeBinder(RecipeData recipeData, List<string> unlockedRecipeList) {
            this.recipeData = recipeData;
            this.unlockedRecipesList = unlockedRecipeList;

            unlocked = new Value<bool>(unlockedRecipeList.Contains(recipeData.name));
        }

        /// <summary>
        /// Check if recipe should be unlocked.
        /// </summary>
        /// <returns>True if recipe should be unlocked, false otherwise</returns>
        public bool CheckIfShouldBeUnlocked() {
            bool result = false;

            foreach (KeyValuePair<SkillBinder, int> pair in relevantSkillList) {
                result = result || (pair.Key.skill.level.Get() >= pair.Value);
            }

            return result;
        }

        /// <summary>
        /// Sets the unlock state of the recipe.
        /// </summary>
        /// <param name="unlocked">State to set to</param>
        public void SetUnlocked(bool unlocked) {
            if (unlocked && !unlockedRecipesList.Contains(recipeData.name)) {
                unlockedRecipesList.Add(recipeData.name);
                this.unlocked.Set(true);
            }
            else if (!unlocked && unlockedRecipesList.Contains(recipeData.name)) {
                unlockedRecipesList.Remove(recipeData.name);
                this.unlocked.Set(false);
            }
        }
    }
}