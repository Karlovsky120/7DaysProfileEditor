using SevenDaysProfileEditor.Inventory;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysProfileEditor.GUI
{
    class IconData
    {
        public static int ICON_WIDTH = 116;
        public static int ICON_HEIGHT = 80;
        public static int ICON_BYTE_LENGTH = ICON_WIDTH * ICON_HEIGHT * 4;
        private static int ICON_MAP_DIMENSION = 4096;
        private static int IMAGE_BYTE_LENGTH = ICON_MAP_DIMENSION * ICON_MAP_DIMENSION * 4;

        public static Dictionary<string, byte[]> itemIconDictionary;
        public static Dictionary<string, UIIconData> uiIconDictionary;

        public static void CreateIconDictionarys()
        {
            itemIconDictionary = new Dictionary<string, byte[]>();
            uiIconDictionary = new Dictionary<string, UIIconData>();

            IconData.ReadModdedItemIcons();
            IconData.ReadDefaultItemIcons();
            IconData.ReadUIIcons();
            
        }

        private static void ReadModdedItemIcons()
        {
            string path = Config.GetSetting("gameRoot") + "\\Mods";

            string[] modDirectories = Directory.GetDirectories(path, "*");

            foreach (string modDirectory in modDirectories)
            {
                path = modDirectory + "\\ItemIcons";

                string[] imagePaths = Directory.GetFiles(path);

                foreach (string imagePath in imagePaths)
                {
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

        private static void ReadDefaultItemIcons()
        {
            string path = Config.GetSetting("gameRoot") + "\\7DaysToDie_Data\\resources.assets";

            BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open, FileAccess.Read));

            AssetInfo itemIconsTexture = AssetInfo.GetAssetInfoByNameAndType("ItemIcons", 28);

            reader.BaseStream.Position = itemIconsTexture.offsetToFileStart;

            int width = reader.ReadInt32();
            int height = reader.ReadInt32();

            reader.BaseStream.Position += 52;

            byte[] pixelArray = GetTextureStream(reader, width, height);

            AssetInfo itemIconsText = AssetInfo.GetAssetInfoByNameAndType("ItemIcons", 49);

            reader.BaseStream.Position = itemIconsText.offsetToFileStart + 11;

            while (reader.PeekChar() != '\0')
            {
                AddIconToDictionary(reader, pixelArray);
            }
        }

        private static void ReadUIIcons()
        {
            string path = Config.GetSetting("gameRoot") + "\\7DaysToDie_Data\\resources.assets";

            BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open, FileAccess.Read));

            AssetInfo uiIconReference = AssetInfo.GetAssetInfoByNameAndType("UIAtlas", 28);

            reader.BaseStream.Position = uiIconReference.offsetToFileStart;

            int width = reader.ReadInt32();
            int height = reader.ReadInt32();

            reader.BaseStream.Position += 52;

            int offsetInResSFile = reader.ReadInt32();

            path = Config.GetSetting("gameRoot") + "\\7DaysToDie_Data\\resources.assets.resS";

            reader = new BinaryReader(new FileStream(path, FileMode.Open, FileAccess.Read));
            reader.BaseStream.Position = offsetInResSFile;

            byte[] textureStream = IconData.GetTextureStream(reader, width, height);

            path = Config.GetSetting("gameRoot") + "\\7DaysToDie_Data\\resources.assets";

            reader = new BinaryReader(new FileStream(path, FileMode.Open, FileAccess.Read));

            AssetInfo uiIconText = AssetInfo.GetAssetInfoByIndex(43116);

            reader.BaseStream.Position = uiIconText.offsetToFileStart + 40;

            BinaryReader textureReader = new BinaryReader(new MemoryStream(textureStream));

            int itemNumber = reader.ReadInt32();

            for (int i = 0; i < itemNumber; i++)
            {
                int nameLength = reader.ReadInt32();
                string name = Util.ReadAssetString(reader, nameLength);

                while (reader.BaseStream.Position % 4 != 0)
                {
                    reader.BaseStream.Position++;
                }

                int XCoord = reader.ReadInt32();
                int YCoord = reader.ReadInt32();

                int imageWidth = reader.ReadInt32();
                int imageHeight = reader.ReadInt32();

                byte[] imageData = ExtractImageFromTexture(textureReader, width, YCoord, XCoord, imageWidth, imageHeight);

                UIIconData iconData = new UIIconData(imageData, imageWidth, imageHeight);

                uiIconDictionary.Add(name, iconData);

                reader.BaseStream.Position += 32;
            }
        }

        private static byte[] GetTextureStream(BinaryReader reader, int width, int height)
        {
            int streamLength = width * height * 4;

            byte[] byteStream = reader.ReadBytes(streamLength);

            //Reverse bytes since it's LittleEndian
            for (int i = 0; i < streamLength - 4; i += 4)
            {
                byte temp = byteStream[i + 0];
                byteStream[i + 0] = byteStream[i + 3];
                byteStream[i + 3] = temp;

                temp = byteStream[i + 1];
                byteStream[i + 1] = byteStream[i + 2];
                byteStream[i + 2] = temp;
            }

            //Load byte array into bmp
            PixelFormat pxf = PixelFormat.Format32bppArgb;
            Bitmap bmp = new Bitmap(width, height);
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);
            IntPtr ptr = bmpData.Scan0;
            Marshal.Copy(byteStream, 0, ptr, streamLength);
            bmp.UnlockBits(bmpData);

            //Flip bmp
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

            //Get byte array back
            bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);
            ptr = bmpData.Scan0;
            byteStream = new byte[streamLength];
            Marshal.Copy(ptr, byteStream, 0, streamLength);

            return byteStream;
        }

        private static void AddIconToDictionary(BinaryReader reader, byte[] pixelArray)
        {
            string name = Util.ReadAssetString(reader, '\t');
            int XCoord = Util.ReadAssetInt(reader, '\t');
            int YCoord = Util.ReadAssetInt(reader, '\n');

            BinaryReader pixelReader = new BinaryReader(new MemoryStream(pixelArray));

            byte[] imagePixels = ExtractImageFromTexture(pixelReader, ICON_MAP_DIMENSION, YCoord, XCoord, ICON_WIDTH, ICON_HEIGHT);
            byte[] dummy;

            if (!itemIconDictionary.TryGetValue(name, out dummy))                
            {
                itemIconDictionary.Add(name, imagePixels);
            }
        }

        private static byte[] ExtractImageFromTexture(BinaryReader textureReader, int textureWidth, int YCoord, int XCoord, int imageWidth, int imageHeight)
        {
            List<byte[]> pixelRows = new List<byte[]>();

            textureReader.BaseStream.Position = 4 * textureWidth * YCoord + XCoord * 4;

            for (int i = 0; i < imageHeight; i++)
            {
                pixelRows.Add(textureReader.ReadBytes(imageWidth * 4));
                textureReader.BaseStream.Position += 4 * (textureWidth - imageWidth);
            }

            return CombinePixelRows(pixelRows);
        }

        private static byte[] CombinePixelRows(List<byte[]> pixelRows)
        {
            byte[] combinedRow = new byte[pixelRows.Count * pixelRows[0].Length];
            int offset = 0;
            foreach (byte[] pixelRow in pixelRows)
            {
                System.Buffer.BlockCopy(pixelRow, 0, combinedRow, offset, pixelRow.Length);
                offset += pixelRow.Length;
            }

            return combinedRow;
        }

        /*public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }*/
    }
}