using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace SevenDaysSaveManipulator {

    class MismatchedSaveVersionException<T> : Exception {

        public T expectedVersion;
        public T actualVersion;

        public MismatchedSaveVersionException(T expectedVersion, T actualVersion) {
            this.expectedVersion = expectedVersion;
            this.actualVersion = actualVersion;
        }

        public MismatchedSaveVersionException(string message, T expectedVersion, T actualVersion)
            : base(message) {
            this.expectedVersion = expectedVersion;
            this.actualVersion = actualVersion;
        }

        public MismatchedSaveVersionException(string message, Exception inner)
            : base(message, inner) {}
    }
}
