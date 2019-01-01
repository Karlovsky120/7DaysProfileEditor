using SevenDaysSaveManipulator.SaveData;

namespace SevenDaysProfileEditor.GUI {

    /// <summary>
    /// Shows the input as percentage.
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    internal class PercentageIntegerTextBox : NumericTextBox<int> {
        private readonly int realMax;

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
            if (int.TryParse(Text, out int newValue)) {
                newValue = Clamp(newValue);

                value.Set((newValue * realMax) / 100);
                Text = newValue.ToString();
            }
            else {
                Text = newValue.ToString();
                Focus();
            }
        }
    }
}