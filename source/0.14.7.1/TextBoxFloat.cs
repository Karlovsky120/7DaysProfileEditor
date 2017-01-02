using SevenDaysSaveManipulator.GameData;
using System;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI
{
    public class TextBoxFloat : TextBox
    {
        public float min;
        public float max;

        public Value<float> value;

        public virtual void update(object sender, EventArgs e)
        {
            float newValue;

            if (float.TryParse(Text, out newValue))
            {
                if (newValue <= min || newValue >= max)
                {
                    newValue = Util.normalize(newValue, min, max);
                }

                value.set(newValue);
                Text = value.get().ToString();
            }

            else
            {
                Text = value.get().ToString();
                Focus();
            }
        }

        public TextBoxFloat(Value<float> value, float min, float max)
        {
            this.value = value;
            this.min = min;
            this.max = max;

            Text = value.get().ToString();
            TextAlign = HorizontalAlignment.Right;

            LostFocus += new EventHandler(update);
        }
    }
}
