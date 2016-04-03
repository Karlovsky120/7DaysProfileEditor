using SevenDaysProfileEditor.GUI;
using SevenDaysSaveManipulator.GameData;
using System;

namespace SevenDaysProfileEditor.Inventory
{
    class TextBoxQuality : TextBoxInt
    {
        TextBoxDegradation textBoxDegradation;

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

                textBoxDegradation.updateMax(newValue);
            }

            else
            {
                Text = value.get().ToString();
                Focus();
            }
        }

        public TextBoxQuality(Value<int> value, int min, int max, TextBoxDegradation textBoxDegradation) : base(value, min, max)
        {
            this.textBoxDegradation = textBoxDegradation;
        }
    }
}
