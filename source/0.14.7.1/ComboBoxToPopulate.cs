using SevenDaysSaveManipulator.GameData;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.GUI
{
    class ComboBoxToPopulate<T> : ComboBox
    {
        public ComboBoxToPopulate(T[] list, int selectedItemIndex)
        {
            DataSource = new T[] { list[selectedItemIndex] };

            EventHandler handler = null;
            handler = (sender, e) => populateComboBox(sender, e, list, list[selectedItemIndex], handler);

            DropDown += handler;
        }

        private void populateComboBox(object sender, EventArgs e, T[] list, T selectedItem, EventHandler handler)
        {
            DataSource = list;
            SelectedItem = selectedItem;

            ComboBox box = (ComboBox)sender;
            EventInfo eventInfo = typeof(ComboBox).GetEvent("DropDown");
            eventInfo.RemoveEventHandler(box, handler);
        }
    }
}
