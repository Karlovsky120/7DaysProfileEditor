using SevenDaysProfileEditor.Skills;
using System.Collections.Generic;

namespace SevenDaysProfileEditor.StatsAndGeneral {

    /// <summary>
    /// Holds all static XML data about a recipe along with dictionary of all the recipes.
    /// </summary>
    internal class RecipeData {
        public static List<RecipeData> recipeList = new List<RecipeData>();

        public bool hasSchematic;
        public string name;
        public Dictionary<SkillData, int> skillUnlockConditions;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="name">Name of the skill</param>
        /// <param name="hasSchematic">True if skill can be unlocked with a schematic</param>
        public RecipeData(string name, bool hasSchematic) {
            this.name = name;
            this.hasSchematic = hasSchematic;
        }

        /// <summary>
        /// Returns recipeData based on name.
        /// </summary>
        /// <param name="name">Name of the recipe</param>
        /// <returns>Recipe, or null if recipe could not be found</returns>
        public static RecipeData GetRecipeDataByName(string name) {
            foreach (RecipeData recipeData in recipeList) {
                if (recipeData.name.Equals(name)) {
                    return recipeData;
                }
            }

            return null;
        }
    }
}