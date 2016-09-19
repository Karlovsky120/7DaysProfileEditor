using SevenDaysSaveManipulator;
using SevenDaysSaveManipulator.GameData;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI
{
    public class NumericTextBox<T> : TextBox, IValueListener<T> where T : IComparable<T>
    {
        protected T min;
        protected T max;

        protected Value<T> value;

        protected bool inputLock = false;

        public virtual void UpdateTextBox()
        {
            if (!inputLock)
            {
                Type type = typeof(T);
                MethodInfo tryParse = type.GetMethod("TryParse", new[] { typeof(string), type.MakeByRefType() });

                var args = new object[] { Text, null };
                var result = (bool)tryParse.Invoke(null, args);

                if (result)
                {
                    value.Set(Clamp((T)args[1]));
                    Text = value.ToString();
                }
                else
                {
                    Text = value.ToString();
                    Focus();
                }
            }
        }

        public void UpdateMax(T max)
        {
            this.max = max;
            UpdateTextBox();
        }

        public void UpdateMin(T min)
        {
            this.min = min;
            UpdateTextBox();
        }

        public void LockInput()
        {
            inputLock = true;
        }

        public void UnlockInput()
        {
            inputLock = false;
        }

        public void ValueUpdated(Value<T> source)
        {
            Text = source.ToString();
        }

        protected T Clamp(T input)
        {
            if (input.CompareTo(max) > 0)
            {
                return max;
            }

            if (input.CompareTo(min) < 0)
            {
                return min;
            }

            return input;
        }

        public void OnLostFocus(object sender, EventArgs e)
        {
            UpdateTextBox();
        }

        public NumericTextBox(Value<T> value, T min, T max, int TextBoxWidth)
        {
            this.value = value;
            this.min = min;
            this.max = max;

            Text = value.ToString();
            Width = TextBoxWidth;
            TextAlign = HorizontalAlignment.Right;

            value.AddListener(this);
            LostFocus += new EventHandler(OnLostFocus);
        }
    }
}