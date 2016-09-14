using SevenDaysSaveManipulator;
using SevenDaysSaveManipulator.GameData;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI
{
    public class TextBoxNum<T> : TextBox, IValueListener<T> where T : IComparable<T>
    {
        protected T min;
        protected T max;

        protected Value<T> value;

        public virtual void Update(object sender, EventArgs e)
        {

        }

        public void UpdateMax(T max)
        {
            this.max = max;
            Update(this, new EventArgs());
        }

        public void ValueUpdated(Value<T> source)
        {
            Text = source.Get().ToString();
        }

        protected T Clamp(T input) 
        {
            if (input.CompareTo(max) > 0)
            {
                return max;
            }

            if (input.CompareTo(max) < 0)
            {
                return min;
            }

            return input;
        }

        public TextBoxNum(Value<T> value, T min, T max, int TextBoxWidth)
        {
            this.value = value;
            this.min = min;
            this.max = max;

            Text = value.Get().ToString();
            Width = TextBoxWidth;
            TextAlign = HorizontalAlignment.Right;

            value.AddListener(this);
            LostFocus += new EventHandler(Update);           
        }
    }
}