using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace SevenDaysProfileEditor.Data {

    /// <summary>
    /// Holds the icon data and the dictionaries of both itemIcons and uiIcons.
    /// </summary>
    internal class IconData {
        public static int ICON_BYTE_LENGTH = ICON_WIDTH * ICON_HEIGHT * 4;
        public static int ICON_HEIGHT = 80;
        public static int ICON_WIDTH = 116;
        public static Dictionary<string, byte[]> itemIconDictionary;
        public static Dictionary<string, UIIconData> uiIconDictionary;
        private static int ICON_MAP_DIMENSION = 4096;
        private static int IMAGE_BYTE_LENGTH = ICON_MAP_DIMENSION * ICON_MAP_DIMENSION * 4;

        /// <summary>
        /// Populates IconData dictionares.
        /// </summary>
        public static void PopulateIconDictionaries() {
            IconData.ReadModdedItemIcons();
            IconData.ReadDefaultItemIcons();
            IconData.ReadUIIcons();
        }

        /// <summary>
        /// Adds the itemIcon specified next in the file the reader is currently reading.
        /// </summary>
        /// <param name="reader">The reader reading the file</param>
        /// <param name="pixelArray">Pixel array that holds the image data for the picture being read</param>
        private static void AddIconToDictionary(BinaryReader reader, byte[] pixelArray) {
            string name = Util.ReadAssetString(reader, '\t');
            int XCoord = Util.ReadAssetInt(reader, '\t');
            int YCoord = Util.ReadAssetInt(reader, '\n');

            BinaryReader pixelReader = new BinaryReader(new MemoryStream(pixelArray));

            byte[] imagePixels = ExtractImageFromTextureAtlas(pixelReader, ICON_MAP_DIMENSION, YCoord, XCoord, ICON_WIDTH, ICON_HEIGHT);
            byte[] dummy;

            if (!itemIconDictionary.TryGetValue(name, out dummy)) {
                itemIconDictionary.Add(name, imagePixels);
            }
        }

        /// <summary>
        /// Combines number of array containing specific rows of an image into a single array.
        /// </summary>
        /// <param name="pixelRows">List of byte arrays holding the rows of the picture</param>
        /// <returns>Byte array that holds whole image data</returns>
        private static byte[] CombinePixelRows(List<byte[]> pixelRows) {
            byte[] combinedRow = new byte[pixelRows.Count * pixelRows[0].Length];
            int offset = 0;
            foreach (byte[] pixelRow in pixelRows) {
                System.Buffer.BlockCopy(pixelRow, 0, combinedRow, offset, pixelRow.Length);
                offset += pixelRow.Length;
            }

            return combinedRow;
        }

        /// <summary>
        /// Extracts image from a texture atlas.
        /// </summary>
        /// <param name="atlasReader">Reader that is reading the atlas</param>
        /// <param name="atlasWidth">Width of the atlas</param>
        /// <param name="YCoord">Y coordinate of the upper left corner of the image</param>
        /// <param name="XCoord">X coordinate of the upper left corner of the image_BYTE_LENGTH</param>
        /// <param name="imageWidth">Width of the image</param>
        /// <param name="imageHeight">Height of the image</param>
        /// <returns>Byte array holding all the pixel info of the image</returns>
        private static byte[] ExtractImageFromTextureAtlas(BinaryReader atlasReader, int atlasWidth, int YCoord, int XCoord, int imageWidth, int imageHeight) {
            List<byte[]> pixelRows = new List<byte[]>();

            atlasReader.BaseStream.Position = 4 * atlasWidth * YCoord + XCoord * 4;

            for (int i = 0; i < imageHeight; i++) {
                pixelRows.Add(atlasReader.ReadBytes(imageWidth * 4));
                atlasReader.BaseStream.Position += 4 * (atlasWidth - imageWidth);
            }

            return CombinePixelRows(pixelRows);
        }

        /// <summary>
        /// Gets the texture atlas for ItemIcons.
        /// </summary>
        /// <param name="reader">Reader currently reading the atlas data</param>
        /// <param name="width">Width of the atlas</param>
        /// <param name="height">Height of the atlas</param>
        /// <returns>Byte array holding the pixel info of the atlas</returns>
        private static byte[] GetTextureAtlas(BinaryReader reader, int width, int height) {
            int streamLength = width * height * 4;

            byte[] byteStream = reader.ReadBytes(streamLength);

            // Reverse bytes since it's LittleEndian
            for (int i = 0; i < streamLength - 4; i += 4) {
                byte temp = byteStream[i + 0];
                byteStream[i + 0] = byteStream[i + 3];
                byteStream[i + 3] = temp;

                temp = byteStream[i + 1];
                byteStream[i + 1] = byteStream[i + 2];
                byteStream[i + 2] = temp;
            }

            // Load byte array into bmp
            PixelFormat pxf = PixelFormat.Format32bppArgb;
            Bitmap bmp = new Bitmap(width, height);
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);
            IntPtr ptr = bmpData.Scan0;
            Marshal.Copy(byteStream, 0, ptr, streamLength);
            bmp.UnlockBits(bmpData);

            // Flip bmp
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

            // Get byte array back
            bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);
            ptr = bmpData.Scan0;
            byteStream = new byte[streamLength];
            Marshal.Copy(ptr, byteStream, 0, streamLength);

            return byteStream;
        }

        /// <summary>
        /// Reads and stores the ItemIcons of vanilla game.
        /// </summary>
        private static void ReadDefaultItemIcons() {
            string path = Config.GetSetting("gameRoot") + "\\7DaysToDie_Data\\resources.assets";

            BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open, FileAccess.Read));

            AssetInfo itemIconsTexture = AssetInfo.GetAssetInfoByNameAndType("ItemIcons", 28);

            reader.BaseStream.Position = itemIconsTexture.offsetToFileStart;

            int width = reader.ReadInt32();
            int height = reader.ReadInt32();

            reader.BaseStream.Position += 52;

            byte[] pixelArray = GetTextureAtlas(reader, width, height);

            AssetInfo itemIconsText = AssetInfo.GetAssetInfoByNameAndType("ItemIcons", 49);

            reader.BaseStream.Position = itemIconsText.offsetToFileStart + 11;

            while (reader.PeekChar() != '\0') {
                AddIconToDictionary(reader, pixelArray);
            }
        }

        /// <summary>
        /// Reads and stores ItemIcons of any mods installed.
        /// </summary>
        private static void ReadModdedItemIcons() {
            string path = Config.GetSetting("gameRoot") + "\\Mods";

            string[] modDirectories = Directory.GetDirectories(path, "*");

            foreach (string modDirectory in modDirectories) {
                path = modDirectory + "\\ItemIcons";

                string[] imagePaths = Directory.GetFiles(path);

                foreach (string imagePath in imagePaths) {
                    string name = Path.GetFileNameWithoutExtension(imagePath);

                    Image image = Image.FromFile(imagePath);
                    image.RotateFlip(RotateFlipType.RotateNoneFlipY);

                    MemoryStream byteStream = new MemoryStream();
                    image.Save(byteStream, ImageFormat.Bmp);

                    byte[] pngBytes = byteStream.ToArray();
                    byte[] bmpBytes = new byte[ICON_BYTE_LENGTH];

                    Array.Copy(pngBytes, 54, bmpBytes, 0, ICON_BYTE_LENGTH);

                    itemIconDictionary.Add(name, bmpBytes);
                }
            }
        }

        /// <summary>
        /// Reads and stores uiIcons.
        /// </summary>
        private static void ReadUIIcons() {
            string path = Config.GetSetting("gameRoot") + "\\7DaysToDie_Data\\resources.assets";

            BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open, FileAccess.Read));
            AssetInfo uiIconReference = AssetInfo.GetAssetInfoByNameAndType("UIAtlas", 28);

            reader.BaseStream.Position = uiIconReference.offsetToFileStart;

            int width = reader.ReadInt32();
            int height = reader.ReadInt32();

            // Skip the atlas header.
            reader.BaseStream.Position += 52;

            // Texture is empty, gets the link to the real atlas in .resS file.
            int offsetInResSFile = reader.ReadInt32();

            // Gets the texture atlas as a byte array of pixels.
            path = Config.GetSetting("gameRoot") + "\\7DaysToDie_Data\\resources.assets.resS";

            reader = new BinaryReader(new FileStream(path, FileMode.Open, FileAccess.Read));
            reader.BaseStream.Position = offsetInResSFile;

            byte[] textureStream = IconData.GetTextureAtlas(reader, width, height);

            // Go back to .assets file and get the coordinates of individual uiIcons to load.
            path = Config.GetSetting("gameRoot") + "\\7DaysToDie_Data\\resources.assets";
            reader = new BinaryReader(new FileStream(path, FileMode.Open, FileAccess.Read));

            AssetInfo uiIconText = AssetInfo.GetAssetInfoByIndex(43116);

            reader.BaseStream.Position = uiIconText.offsetToFileStart + 40;

            BinaryReader textureReader = new BinaryReader(new MemoryStream(textureStream));

            int itemNumber = reader.ReadInt32();

            for (int i = 0; i < itemNumber; i++) {
                int nameLength = reader.ReadInt32();
                string name = Util.ReadAssetString(reader, nameLength);

                // Align to 4-bytes.
                while (reader.BaseStream.Position % 4 != 0) {
                    reader.BaseStream.Position++;
                }

                int XCoord = reader.ReadInt32();
                int YCoord = reader.ReadInt32();

                int imageWidth = reader.ReadInt32();
                int imageHeight = reader.ReadInt32();

                byte[] imageData = ExtractImageFromTextureAtlas(textureReader, width, YCoord, XCoord, imageWidth, imageHeight);

                UIIconData iconData = new UIIconData(imageData, imageWidth, imageHeight);

                uiIconDictionary.Add(name, iconData);

                reader.BaseStream.Position += 32;
            }
        }
    }
}