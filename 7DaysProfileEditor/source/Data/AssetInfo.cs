using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace SevenDaysProfileEditor.Data
{

    /// <summary>
    /// Holds the list of all the assets and is used to populate it.
    /// </summary>
    internal class AssetInfo
    {
        public static List<AssetInfo> assetInfoList;

        public uint currentFileSize;
        public uint currentFileType;
        public UInt64 index;
        public string name;
        public uint offsetToFileStart;

        /// <summary>
        /// Creates an AssetInfo object with specified data.
        /// </summary>
        /// <param name="index">Index of the asset</param>
        /// <param name="name">Index of asset</param>
        /// <param name="offsetToFileStart">Offset from the files start of the asset</param>
        /// <param name="currentFileSize">Size of the asset</param>
        /// <param name="currentFileType">Type of the asset</param>
        public AssetInfo(UInt64 index, string name, uint offsetToFileStart, uint currentFileSize, uint currentFileType)
        {
            this.index = index;
            this.name = name;
            this.offsetToFileStart = offsetToFileStart;
            this.currentFileSize = currentFileSize;
            this.currentFileType = currentFileType;
        }

        /// <summary>
        /// Populates assetInfoList with assets formatted as AssetInfo objects.
        /// </summary>
        public static void GenerateAssetInfoList()
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

                // We save the current position of the BaseStream.
                long position = reader.BaseStream.Position + 8;

                // Now we move to the file at hand.
                uint absoluteOffset = offsetFirstFile + offsetCurrentFile;
                reader.BaseStream.Position = absoluteOffset;

                // We read the name of the file.
                int nameLength = reader.ReadInt32();
                string name = Util.ReadAssetString(reader, nameLength);

                // Since this is 4-byte aligned, we have to account for any trailing zero bytes.
                uint additionalOffset = 4 + (4 * ((uint)nameLength / 4));

                if (nameLength % 4 != 0)
                {
                    additionalOffset += 4;
                }

                AssetInfo assetInfo = new AssetInfo(index, name, absoluteOffset + additionalOffset, currentFileSize - additionalOffset, currentFileType);
                assetInfoList.Add(assetInfo);

                // We restore the position, ready to read the next file.
                reader.BaseStream.Position = position;
            }
        }

        /// <summary>
        /// Returns AssetInfo with specified index.
        /// </summary>
        /// <param name="index">Specified index</param>
        /// <returns>The AssetInfo requested, or null if no such asset was not found</returns>
        public static AssetInfo GetAssetInfoByIndex(ulong index)
        {
            return assetInfoList.Where(q => q.index == index).FirstOrDefault();
            /*
            foreach (AssetInfo assetInfo in assetInfoList)
            {
                if (assetInfo.index == index)
                {
                    return assetInfo;
                }
            }

            return null;
            */
        }

        /// <summary>
        /// Returns AssetInfo that has the specified name and type.
        /// </summary>
        /// <param name="name">Specified name</param>
        /// <param name="type">Specified type</param>
        /// <returns>The AssetInfo requested, or null if no such asset was not found</returns>
        public static AssetInfo GetAssetInfoByNameAndType(string name, uint type)
        {
            return assetInfoList.Where(q => q.name == name && q.currentFileType== type).FirstOrDefault();
            /*
            foreach (AssetInfo assetInfo in assetInfoList)
            {
                if (assetInfo.name.Equals(name) && assetInfo.currentFileType == type)
                {
                    return assetInfo;
                }
            }
            return null;
            */
        }

        /// <summary>
        /// Clears assetInfo list
        /// </summary>
        public static void ClearAssetInfoList()
        {
            //assetInfoList.Clear();
            assetInfoList = null;
        }
    }
}