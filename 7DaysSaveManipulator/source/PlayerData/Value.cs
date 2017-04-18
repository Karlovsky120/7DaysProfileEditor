using System;
using System.Collections.Generic;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class Value<T> {

        [NonSerialized]
        private List<WeakReference<IValueListener<T>>> listeners = new List<WeakReference<IValueListener<T>>>();

        private T value;

        public Value(T value) {
            this.value = value;
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
                    IValueListener<T> listener;
                    if (reference.TryGetTarget(out listener)) {
                        listener.ValueUpdated(this);
                    }
                }
            }
        }

        public override string ToString() {
            return value.ToString();
        }
    }
}