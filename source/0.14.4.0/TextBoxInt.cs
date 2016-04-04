using SevenDaysSaveManipulator.GameData;
using System;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI
{
    public class TextBoxInt : TextBox
    {
        public int min;
        public int max;

        public Value<int> value;

        public virtual void update(object sender, EventArgs e)
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
            }

            else
            {
                Text = value.get().ToString();
                Focus();
            }
        }

        public void updateMax(int max)
        {
            this.max = max;
            update(this, new EventArgs());
        }

        public TextBoxInt(Value<int> value, int min, int max)
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
