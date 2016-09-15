using SevenDaysSaveManipulator.GameData;
using System;

namespace SevenDaysProfileEditor.GUI
{
    public class TextBoxUInt : TextBoxNum<uint>
    {
        public override void Update(object sender, EventArgs e)
        {
            uint newValue;

            if (uint.TryParse(Text, out newValue))
            {
                if (newValue < min || newValue > max)
                {
                    newValue = Clamp(newValue);
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

        public TextBoxUInt(Value<uint> value, uint min, uint max, int width) : base(value, min, max, width)
        {

        }
    }
}