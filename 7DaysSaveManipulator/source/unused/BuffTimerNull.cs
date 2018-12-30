using System;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    public class BuffTimerNull : BuffTimer {

        public BuffTimerNull()
            : base(EnumBuffTimerClassId.Null) {
        }
    }
}