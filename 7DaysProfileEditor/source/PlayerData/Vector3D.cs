using System;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class Vector3D<T> {
        public Value<float> heading;
        public Value<T> x;
        public Value<T> y;
        public Value<T> z;
    }
}