using SevenDaysSaveManipulator.GameData;

namespace SevenDaysProfileEditor.GUI
{
    public class InvertedIntegerTextBox : NumericTextBox<int>
    {
        public override void UpdateTextBox()
        {
            int newValue;

            if (int.TryParse(Text, out newValue))
            {
                if (newValue < min || newValue > max)
                {
                    newValue = Clamp(newValue);
                }

                value.Set(max - newValue);
                Text = newValue.ToString();
            }
            else
            {
                Text = newValue.ToString();
                Focus();
            }
        }

        public InvertedIntegerTextBox(Value<int> value, int min, int max, int width)
            : base(value, min, max, width)
        {
            Text = (max - value.Get()).ToString();
        }
    }
}