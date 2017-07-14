namespace SevenDaysSaveManipulator.PlayerData {

    public interface IValueListener<T> {

        void ValueUpdated(Value<T> source);
    }
}