using SevenDaysSaveManipulator.GameData;

namespace SevenDaysProfileEditor.GUI
{
    public class PercentageIntegerTextBox : NumericTextBox<int>
    {
        private int realMax;

        public override void UpdateTextBox()
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

        public PercentageIntegerTextBox(Value<int> value, int realMax, int width)
            : base(value, 0, 100, width)
        {
            this.realMax = realMax;

            Text = ((int)(100 * value.Get() / (float)realMax)).ToString();
        }
    }
}