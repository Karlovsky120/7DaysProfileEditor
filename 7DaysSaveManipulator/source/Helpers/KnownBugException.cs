using System;

namespace SevenDaysSaveManipulator {

    public class KnownBugException : Exception {

        public KnownBugException(string message, Exception inner) : base(message, inner) { }
    }
}
