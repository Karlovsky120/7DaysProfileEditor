using SevenDaysSaveManipulator.GameData;

namespace SevenDaysSaveManipulator {

    public interface IValueListener<T> {

        void ValueUpdated(Value<T> source);
    }
}