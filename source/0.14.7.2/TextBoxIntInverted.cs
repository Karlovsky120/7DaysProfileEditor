using SevenDaysProfileEditor.GUI;
using SevenDaysSaveManipulator.GameData;
using System;

namespace SevenDaysProfileEditor.GUI
{
    public class TextBoxIntInverted : TextBoxInt
    {
        public override void Update(object sender, EventArgs e)
        {
            int newValue;

            if (int.TryParse(Text, out newValue))
            {
                if (newValue < min || newValue > max)
                {
                    newValue = Normalize(newValue);
                }

                value.Set(max - newValue);
                Text = (max - value.Get()).ToString();
            }

            else
            {
                Text = (max - value.Get()).ToString();
                Focus();
            }
        }

        public TextBoxIntInverted(Value<int> value, int min, int max, int width) : base(value, min, max, width)
        {
            Text = (max - value.Get()).ToString();
        }
    }
}
