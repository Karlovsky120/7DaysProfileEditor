using System;
using System.Collections.Generic;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    public class Value<T> {

        [NonSerialized]
        private List<WeakReference<IValueListener<T>>> listeners = new List<WeakReference<IValueListener<T>>>();

        private T value;

        public Value(T value) {
            this.value = value;
        }

        internal Value(TypedBinaryReader reader) {
            Read(reader);
        }

        public void AddListener(IValueListener<T> listener) {
            listeners.Add(new WeakReference<IValueListener<T>>(listener));
        }

        public T Get() {
            return value;
        }

        public void Set(T value) {
            if (!this.value.Equals(value)) {
                this.value = value;

                foreach (WeakReference<IValueListener<T>> reference in listeners) {
                    if (reference.TryGetTarget(out IValueListener<T> listener)) {
                        listener.ValueUpdated(this);
                    }
                }
            }
        }

        public override string ToString() {
            return value.ToString();
        }

        public void Read(TypedBinaryReader reader) {
            value = reader.ReadBasicType<T>();
        }

        public void Write(TypedBinaryWriter writer) {
            writer.WriteBasicType(value);
        }

        public void TODO_REMOVE_ME_XML() {}
    }
}