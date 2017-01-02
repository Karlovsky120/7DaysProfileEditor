using SevenDaysProfileEditor.GUI;
using SevenDaysProfileEditor.Skills;
using SevenDaysSaveManipulator.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysProfileEditor.Skills
{
    class TextBoxLevel : TextBoxInt
    {
        private BinderSkill binderSkill;
        private new Value<int> max;

        public override void update(object sender, EventArgs e)
        {
            int newValue;

            if (int.TryParse(Text, out newValue))
            {
                if (newValue <= min || newValue >= max.get())
                {
                    newValue = Util.normalize(newValue, min, max.get());
                }

                value.set(newValue);
                Text = value.get().ToString();

                binderSkill.notifyListeners();
            }

            else
            {
                Text = value.get().ToString();
                Focus();
            }
        }

        public TextBoxLevel(Value<int> value, int min, Value<int> max, BinderSkill binderSkill) : base(value, min, max.get())
        {
            this.binderSkill = binderSkill;
            this.max = max;
        }
    }
}
