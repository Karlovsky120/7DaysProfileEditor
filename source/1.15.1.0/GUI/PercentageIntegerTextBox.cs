using SevenDaysSaveManipulator.GameData;

namespace SevenDaysProfileEditor.GUI {

    /// <summary>
    /// Shows the input as percentage.
    /// </summary>
    internal class PercentageIntegerTextBox : NumericTextBox<int> {
        private int realMax;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="value">Value to be tracked</param>
        /// <param name="realMax">Max allowed value before it's turned into percentage</param>
        /// <param name="width">Width of the textBox</param>
        public PercentageIntegerTextBox(Value<int> value, int realMax, int width)
            : base(value, 0, 100, width) {
            this.realMax = realMax;

            Text = ((int)(100 * value.Get() / (float)realMax)).ToString();
        }

        /// <summary>
        /// Called on every lost focus event. Checks if new input is correct or reverts to the previous one.
        /// </summary>
        public override void UpdateTextBox() {
            int newValue;

            if (int.TryParse(Text, out newValue)) {
                if (newValue < min || newValue > max) {
                    newValue = Clamp(newValue);
                }

                value.Set((int)((newValue * realMax) / 100));
                Text = newValue.ToString();
            }
            else {
                Text = newValue.ToString();
                Focus();
            }
        }
    }
}