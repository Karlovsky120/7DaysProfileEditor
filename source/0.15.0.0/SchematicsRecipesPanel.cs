using SevenDaysProfileEditor.Inventory;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.General
{
    internal class SchematicsRecipesPanel : TableLayoutPanel
    {
        public SchematicsRecipesPanel()
        {
            foreach (string recipe in ItemData.schematicRecipeList)
            {
                //Controls.Add(new LabeledSchematicRecipe(recipe));
            }
        }
    }
}