using System;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class BuffTimerNull : BuffTimer {

        public BuffTimerNull()
            : base(EnumBuffTimerClassId.Null) {
        }
    }
}