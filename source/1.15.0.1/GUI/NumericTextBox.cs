using SevenDaysSaveManipulator;
using SevenDaysSaveManipulator.GameData;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI {

    /// <summary>
    /// Shows numerical data in a textBox.
    /// </summary>
    /// <typeparam name="T">A numerical type</typeparam>
    internal class NumericTextBox<T> : TextBox, IValueListener<T> where T : IComparable<T> {
        protected bool inputLock = false;
        protected T max;
        protected T min;
        protected Value<T> value;

        /// <summary>
        /// Default contructor.
        /// </summary>
        /// <param name="value">Value to track</param>
        /// <param name="min">Minimum allowed value</param>
        /// <param name="max">Maximum allowed valueUpdated</param>
        /// <param name="width">Width of the textBox</param>
        public NumericTextBox(Value<T> value, T min, T max, int width) {
            this.value = value;
            this.min = min;
            this.max = max;

            Text = value.ToString();
            Width = width;
            TextAlign = HorizontalAlignment.Right;

            value.AddListener(this);
            LostFocus += new EventHandler(OnLostFocus);
        }

        /// <summary>
        /// Event handler for when focus is lost.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnLostFocus(object sender, EventArgs e) {
            UpdateTextBox();
        }

        /// <summary>
        /// Updates max allowed value
        /// </summary>
        /// <param name="max">New max allowed value</param>
        public void UpdateMax(T max) {
            this.max = max;
            UpdateTextBox();
        }

        /// <summary>
        /// Called on every lost focus event. Checks if new input is correct or reverts to the previous one.
        /// </summary>
        public virtual void UpdateTextBox() {
            if (!inputLock) {
                Type type = typeof(T);
                MethodInfo tryParse = type.GetMethod("TryParse", new[] { typeof(string), type.MakeByRefType() });

                var args = new object[] { Text, null };
                var result = (bool)tryParse.Invoke(null, args);

                if (result) {
                    value.Set(Clamp((T)args[1]));
                    Text = value.ToString();
                }
                else {
                    Text = value.ToString();
                    Focus();
                }
            }
        }

        /// <summary>
        /// Updates the dispaly on value change.
        /// </summary>
        /// <param name="source"></param>
        public void ValueUpdated(Value<T> source) {
            Text = source.ToString();
        }

        /// <summary>
        /// Clamps the value between min and max.
        /// </summary>
        /// <param name="input">Value to be clamped</param>
        /// <returns>Clapmed value</returns>
        protected T Clamp(T input) {
            if (input.CompareTo(max) > 0) {
                return max;
            }

            if (input.CompareTo(min) < 0) {
                return min;
            }

            return input;
        }
    }
}