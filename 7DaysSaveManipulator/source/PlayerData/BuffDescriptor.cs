using System;
using System.Collections.Generic;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class BuffDescriptor {

        //num = 2
        public static Value<int> buffDescriptorVersion;

        //CategoryFlags
        public EnumBuffCategoryFlags categoryFlags;

        //NotificationClass
        public string notificationClass;

        //Overrides
        public HashSet<string> overrides;

        public static BuffDescriptor Read(BinaryReader reader) {
            buffDescriptorVersion = new Value<int>(reader.ReadInt32());
            BuffDescriptor buffDescriptor = new BuffDescriptor();
            buffDescriptor.categoryFlags = (EnumBuffCategoryFlags)reader.ReadInt32();

            buffDescriptor.notificationClass = reader.ReadString();

            //
            int overrideCount = reader.ReadInt32();
            buffDescriptor.overrides = new HashSet<string>();
            for (int i = 0; i < overrideCount; i++) {
                buffDescriptor.overrides.Add(reader.ReadString());
            }

            return buffDescriptor;
        }

        public void Write(BinaryWriter writer) {
            writer.Write(buffDescriptorVersion.Get());
            writer.Write((int)categoryFlags);
            writer.Write(notificationClass);
            writer.Write(overrides.Count);
            foreach (string text in overrides) {
                writer.Write(text);
            }
        }
    }
}