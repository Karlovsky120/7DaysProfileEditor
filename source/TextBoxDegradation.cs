using SevenDaysProfileEditor.GUI;
using SevenDaysSaveManipulator.GameData;
using System;

namespace SevenDaysProfileEditor.Inventory
{
    class TextBoxDegradation : TextBoxInt
    {
        BinderItem itemBinder;
        public override void update(object sender, EventArgs e)
        {
            int newValue;

            if (int.TryParse(Text, out newValue))
            {
                if (newValue <= min || newValue >= max)
                {
                    newValue = Util.normalize(newValue, min, max);
                }

                value.set(max - newValue);
                Text = (max - value.get()).ToString();
            }

            else
            {
                Text = (max - value.get()).ToString();
                Focus();
            }
        }

        public TextBoxDegradation(Value<int> value, int min, int max, BinderItem itemBinder) : base(value, min, max)
        {
            Text = (max - value.get()).ToString();
            this.itemBinder = itemBinder;
        }
    }
}
