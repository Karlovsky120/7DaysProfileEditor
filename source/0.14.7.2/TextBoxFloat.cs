using SevenDaysSaveManipulator.GameData;
using System;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI
{
    public class TextBoxFloat : TextBoxNum<float>
    {
        public override void Update(object sender, EventArgs e)
        {
            float newValue;

            if (float.TryParse(Text, out newValue))
            {
                if (newValue < min || newValue > max)
                {
                    newValue = Normalize(newValue);
                }

                value.Set(newValue);
                Text = value.Get().ToString();
            }

            else
            {
                Text = value.Get().ToString();
                Focus();
            }
        }

        public TextBoxFloat(Value<float> value, float min, float max, int width) : base(value, min, max, width)
        {

        }
    }
}
