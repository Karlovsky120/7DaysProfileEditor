using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysSaveManipulator.RegionData {

    /// <summary>
    /// Collection of static methods for manipulating region files.
    /// </summary>
    public class Region {

        int rX;
        int rZ;

        public MemoryStream[] chunkStreams = new MemoryStream[1024];
        public int[] timeStamps = new int[1024];

        private Region() {}

        public Region(string path, int rX, int rZ) {
            Read(path, rX, rZ);
        }

        /// <summary>
        /// Creates a region object from region file of given coordinates within the provided folder. It looks for a file named "r.rX.rZ.7rg". Note that region (0,0) contains chunks (0,0) to (31,31). Calculate accordingly.
        /// </summary>
        /// <param name="path">Path to the region file. Prefrably "Region" folder of the saved world folder.</param>
        /// <param name="rX">X coordinate of the region.</param>
        /// <param name="rZ">X coordinate of the region.</param>
        public void Read(string path, int rX, int rZ) {

            this.rX = rX;
            this.rZ = rZ;

            string file = String.Format(path + "\\r.{0}.{1}.7rg", rX, rZ);

            if (!File.Exists(file)) {
                throw new FileNotFoundException(String.Format("Region file with coordinates ({0},{1}) does not exist.", rX, rZ));
            }
            FileStream regionStream = new FileStream(file, FileMode.Open);

            BinaryReader reader = new BinaryReader(regionStream);

            int[] wasted = new int[1024];

            for (int i = 0; i < 1024; ++i) {
                reader.BaseStream.Seek(4 + 4 * i, SeekOrigin.Begin);

                ushort offset = reader.ReadUInt16();
                if (offset != 0) {

                    reader.BaseStream.Seek(4100 + 4 * i, SeekOrigin.Begin);
                    timeStamps[i] = reader.ReadInt32();
              
                    reader.BaseStream.Seek(offset * 4096 + 4, SeekOrigin.Begin);

                    int length = reader.ReadInt32();

                    // To skip a zero.
                    ++reader.BaseStream.Position;

                    chunkStreams[i] = new MemoryStream(reader.ReadBytes(length));
                    chunkStreams[i].Seek(0, SeekOrigin.Begin);
                }

                else {
                    timeStamps[i] = 0;
                    chunkStreams[i] = null;
                }
            }

            reader.Close();
        }

        /// <summary>
        /// Saves this region to disk.
        /// </summary>
        /// <param name="path">Path to save to. Preferably "Region" folder within the saved world folder.</param>
        public void Write(string path) {
            FileStream regionStream = new FileStream(string.Format(path + "\\r.{0}.{1}.7rg", rX, rZ), FileMode.Create);

            regionStream.WriteByte(55);
            regionStream.WriteByte(114);
            regionStream.WriteByte(103);
            regionStream.WriteByte(0);

            // To skip the offsets, which are filled in at the end.
            regionStream.Seek(4100, SeekOrigin.Begin);

            for (int i = 0; i < 1024; ++i) {
                regionStream.Write(BitConverter.GetBytes(timeStamps[i]), 0, 4);
            }

            int[] offsets = new int[1024];
            int[] size = new int[1024];

            for (int j = 0; j < 1024; ++j) {

                if (chunkStreams[j] != null) {
                    
                    long position = regionStream.Position;

                    offsets[j] = (int)Math.Ceiling((double)(position - 4) / 4096);

                    regionStream.Seek((long)((offsets[j] * 4096) + 4), SeekOrigin.Begin); 

                    size[j] = ((int)chunkStreams[j].Length / 4096) + 1;

                    regionStream.Write(BitConverter.GetBytes(chunkStreams[j].Length), 0, 4);
                    regionStream.WriteByte(0);
                    chunkStreams[j].WriteTo(regionStream);
                    chunkStreams[j].Seek(0, SeekOrigin.Begin);

                    for (int i = (int)chunkStreams[j].Length; i < (size[j] * 4096) - 5; ++i) {
                        regionStream.WriteByte(0);
                    }
                }

                else {
                    offsets[j] = 0;
                    size[j] = 0;
                }
            }

            regionStream.Seek(4, SeekOrigin.Begin);

            for (int k = 0; k < 1024; ++k) {
                regionStream.Write(BitConverter.GetBytes((short)offsets[k]), 0, 2);
                regionStream.WriteByte(0);
                regionStream.WriteByte((byte)size[k]);
            }

            regionStream.Close();
        }
    }
}
