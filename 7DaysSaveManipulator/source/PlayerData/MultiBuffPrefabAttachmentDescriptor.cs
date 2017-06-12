using System;
using System.IO;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    public class MultiBuffPrefabAttachmentDescriptor {

        //notSaved = 1
        public static Value<int> multiBuffPrefabAttachmentDescriptorVersion;

        //FirstPerson
        public Value<bool> firstPerson;

        //PrefabName
        public Value<string> prefabName;

        //ThirdPerson
        public Value<bool> thirdPerson;

        //TransformPath
        public Value<string> transformPath;

        //TTL
        public Value<float> TTL;

        public static MultiBuffPrefabAttachmentDescriptor Read(BinaryReader reader) {
            multiBuffPrefabAttachmentDescriptorVersion = new Value<int>(reader.ReadInt32());
            return new MultiBuffPrefabAttachmentDescriptor {
                prefabName = new Value<string>(reader.ReadString()),
                transformPath = new Value<string>(reader.ReadString()),
                TTL = new Value<float>(reader.ReadSingle()),
                firstPerson = new Value<bool>(reader.ReadBoolean()),
                thirdPerson = new Value<bool>(reader.ReadBoolean())
            };
        }

        public void Write(BinaryWriter writer) {
            writer.Write(multiBuffPrefabAttachmentDescriptorVersion.Get());
            writer.Write(prefabName.Get());
            writer.Write(transformPath.Get());
            writer.Write(TTL.Get());
            writer.Write(firstPerson.Get());
            writer.Write(thirdPerson.Get());
        }
    }
}