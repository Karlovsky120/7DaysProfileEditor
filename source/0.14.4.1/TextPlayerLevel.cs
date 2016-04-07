using SevenDaysProfileEditor.GUI;
using SevenDaysSaveManipulator.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysProfileEditor.Skills
{
    class TextBoxPlayerLevel : TextBoxInt
    {
        private List<BinderSkill> playerListeners;

        public override void update(object sender, EventArgs e)
        {
            int newValue;

            if (int.TryParse(Text, out newValue))
            {
                if (newValue <= min || newValue >= max)
                {
                    newValue = Util.normalize(newValue, min, max);
                }

                value.set(newValue);
                Text = value.get().ToString();

                foreach (BinderSkill binderSkill in playerListeners)
                {
                    binderSkill.update();
                }
            }

            else
            {
                Text = value.get().ToString();
                Focus();
            }
        }
        public TextBoxPlayerLevel(Value<int> value, int min, int max, List<BinderSkill> playerListeners) : base(value, min, max)
        {
            this.playerListeners = playerListeners;
        }
    }
}
