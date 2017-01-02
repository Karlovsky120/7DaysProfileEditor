using System;
using System.Collections.Generic;
using System.IO;

namespace SevenDaysProfileEditor.Data
{
    internal class AssetInfo
    {
        public static List<AssetInfo> assetInfoList;

        public UInt64 index;
        public string name;
        public uint offsetToFileStart;
        public uint currentFileSize;
        public uint currentFileType;

        public AssetInfo(UInt64 index, string name, uint offsetToFileStart, uint currentFileSize, uint currentFileType)
        {
            this.index = index;
            this.name = name;
            this.offsetToFileStart = offsetToFileStart;
            this.currentFileSize = currentFileSize;
            this.currentFileType = currentFileType;
        }

        public static AssetInfo GetAssetInfoByNameAndType(string name, uint type)
        {
            foreach (AssetInfo assetInfo in assetInfoList)
            {
                if (assetInfo.name.Equals(name) && assetInfo.currentFileType == type)
                {
                    return assetInfo;
                }
            }

            return null;
        }

        public static AssetInfo GetAssetInfoByIndex(ulong index)
        {
            foreach (AssetInfo assetInfo in assetInfoList)
            {
                if (assetInfo.index == index)
                {
                    return assetInfo;
                }
            }

            return null;
        }

        public static void GenerateAssetDictionary()
        {
            AssetInfo.assetInfoList = new List<AssetInfo>();

            string path = Config.GetSetting("gameRoot") + "\\7DaysToDie_Data\\resources.assets";

            BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open, FileAccess.Read));

            // header
            reader.BaseStream.Position += 12;

            byte[] offsetInverted = reader.ReadBytes(4);
            Array.Reverse(offsetInverted);
            uint offsetFirstFile = BitConverter.ToUInt32(offsetInverted, 0);

            reader.BaseStream.Position += 4;

            // typeTree
            reader.BaseStream.Position += 12;
            bool hasTypeTree = reader.ReadBoolean();
            uint fieldCount = reader.ReadUInt32();
            for (int i = 0; i < fieldCount; i++)
            {
                int classId = reader.ReadInt32();

                if (classId < 0)
                {
                    reader.BaseStream.Position += 16;
                }

                reader.BaseStream.Position += 16;

                if (hasTypeTree)
                {
                    uint typeFieldsExCount = reader.ReadUInt32();
                    uint stringTableLen = reader.ReadUInt32();

                    reader.BaseStream.Position += 24 * typeFieldsExCount + stringTableLen;
                }
            }

            // objectInfo
            uint sizeFiles = reader.ReadUInt32();
            reader.BaseStream.Position += 3;

            for (int i = 0; i < sizeFiles; i++)
            {
                UInt64 index = reader.ReadUInt64();
                uint offsetCurrentFile = reader.ReadUInt32();
                uint currentFileSize = reader.ReadUInt32();
                uint currentFileType = reader.ReadUInt32();

                long position = reader.BaseStream.Position + 8;

                uint absoluteOffset = offsetFirstFile + offsetCurrentFile;
                reader.BaseStream.Position = absoluteOffset;

                int nameLength = reader.ReadInt32();
                string name = Util.ReadAssetString(reader, nameLength);

                uint additionalOffset = 4 + (4 * ((uint)nameLength / 4));

                if (nameLength % 4 != 0)
                {
                    additionalOffset += 4;
                }

                AssetInfo assetInfo = new AssetInfo(index, name, absoluteOffset + additionalOffset, currentFileSize - additionalOffset, currentFileType);
                AssetInfo.assetInfoList.Add(assetInfo);

                reader.BaseStream.Position = position;
            }
        }
    }
}