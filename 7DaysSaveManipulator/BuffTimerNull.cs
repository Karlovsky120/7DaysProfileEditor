using System;

namespace SevenDaysSaveManipulator.GameData {

    [Serializable]
    public class BuffTimerNull : BuffTimer {

        public BuffTimerNull()
            : base(EnumBuffTimerClassId.Null) {
        }
    }
}