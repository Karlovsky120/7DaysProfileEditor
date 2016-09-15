using SevenDaysSaveManipulator.GameData;
using System;

namespace SevenDaysProfileEditor.GUI
{
    public class TextBoxIntPercentage : TextBoxInt
    {
        private int realMax;

        public override void Update(object sender, EventArgs e)
        {
            int newValue;

            if (int.TryParse(Text, out newValue))
            {
                if (newValue < min || newValue > max)
                {
                    newValue = Clamp(newValue);
                }

                value.Set((int)((newValue * realMax) / 100));
                Text = newValue.ToString();
            }

            else
            {
                Text = newValue.ToString();
                Focus();
            }
        }

        public TextBoxIntPercentage(Value<int> value, int realMax, int width) : base(value, 0, 100, width)
        {
            this.realMax = realMax;

            Text = ((int)(100 * value.Get() / (float)realMax)).ToString();
        }
    }
}
