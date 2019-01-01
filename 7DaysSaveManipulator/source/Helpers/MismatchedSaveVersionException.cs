using System;

namespace SevenDaysSaveManipulator {

    public class MismatchedSaveVersionException : Exception {

        public MismatchedSaveVersionException(string message) : base(message) {}
    }
}
