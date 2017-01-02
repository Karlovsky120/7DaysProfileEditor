using SevenDaysSaveManipulator.GameData;

namespace SevenDaysProfileEditor.GUI {

    /// <summary>
    /// Show the the input as "max-input".
    /// </summary>
    internal class InvertedIntegerTextBox : NumericTextBox<int> {

        /// <summary>
        /// Creates the textBox.
        /// </summary>
        /// <param name="value">Value to track</param>
        /// <param name="min">Minimum allowed value</param>
        /// <param name="max">Maximum allowed value</param>
        /// <param name="width">Width of the textBox</param>
        public InvertedIntegerTextBox(Value<int> value, int min, int max, int width)
            : base(value, min, max, width) {
            Text = (max - value.Get()).ToString();
        }

        /// <summary>
        /// Invoked when tracked value is updated. It updates the display appropriately.
        /// </summary>
        public override void UpdateTextBox() {
            int newValue;

            if (int.TryParse(Text, out newValue)) {
                if (newValue < min || newValue > max) {
                    newValue = Clamp(newValue);
                }

                value.Set(max - newValue);
                Text = newValue.ToString();
            }
            else {
                Text = newValue.ToString();
                Focus();
            }
        }
    }
}