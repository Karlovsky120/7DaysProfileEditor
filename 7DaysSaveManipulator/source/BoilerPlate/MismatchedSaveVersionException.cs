using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysSaveManipulator.source.PlayerData
{
    class MismatchedSaveVersionException<T> : Exception
    {
        public T expectedVersion;
        public T actualVersion;

        public string ResourceReferenceProperty { get; set; }

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

        protected MismatchedSaveVersionException(SerializationInfo info, StreamingContext context)
            : base(info, context) {
            ResourceReferenceProperty = info.GetString("ResourceReferenceProperty");
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            if (info == null) {
                throw new ArgumentNullException("info");
            }
            info.AddValue("ResourceReferenceProperty", ResourceReferenceProperty);
            base.GetObjectData(info, context);
        }
    }
}
