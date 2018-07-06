using ICSharpCode.SharpZipLib.Zip;
using SevenDaysSaveManipulator.PlayerData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysSaveManipulator.RegionData {
    public class Chunk {

        public Region parentRegion;

        public int rcX;
        public int rcZ;

        public uint version;

        public int xm;
        public int mm;
        public int rm;
        public ulong savedInWorldTicks;

        public ChunkBlockLayer[] cbl = new ChunkBlockLayer[64];
        public ChunkBlockChannel cbc;

        public byte[] im = new byte[256];
        public byte[] terrainHeight = new byte[256];
        public byte[] biomeId = new byte[256];
        public byte[] biomeIntensity = new byte[1536];

        public byte dominantBiome;
        public byte areaMasterDominantBiome;

        public Dictionary<string, ChunkCustomData> chunkCustomDataDictionary = new Dictionary<string, ChunkCustomData>();

        public byte[] pr = new byte[256];
        public byte[] jr = new byte[256];
        public byte[] fr = new byte[256];

        public ChunkBlockChannel cm = new ChunkBlockChannel();
        public ChunkBlockChannel vm = new ChunkBlockChannel();
        public ChunkBlockChannel gm = new ChunkBlockChannel();
        public ChunkBlockChannel km = new ChunkBlockChannel();

        public bool needsLightCalculation;

        public List<EntityCreationData> entityCreationDataList = new List<EntityCreationData>();

        public Dictionary<Vector3D<int>, TileEntity> tileEntityDictionary = new Dictionary<Vector3D<int>, TileEntity>();

        public uint entitySpawnerSaveVersion;

        public List<EntitySpawner> entitySpawnerList = new List<EntitySpawner>();

        public ushort[] ur;

        private Chunk() {}

        /// <summary>
        /// Creates the chunk object from region file at given LOCAL (relative to the region) coordinates (0,0 to 31,31).
        /// </summary>
        /// <param name="region">Region to load the chunk from.</param>
        /// <param name="rcX">X coordinate of the chunk within the region (LOCAL).</param>
        /// <param name="rcZ">Z coordinate of the chunk within the region (LOCAL).</param>
        public Chunk(Region region, int rcX, int rcZ) {

            parentRegion = region;

            if (rcX < 0 || rcX > 31 || rcZ < 0 || rcZ > 31) {
                throw new ArgumentOutOfRangeException("rcX and rcZ has to be an integer between 0 and 31");
            }

            this.rcX = rcX;
            this.rcZ = rcZ;

            Read(UnzipChunk(region.chunkStreams[rcX + rcZ * 32]));
        }

        /// <summary>
        /// Used to save chunk to region. You still need to save the region object to disk for the changes to stick.
        /// </summary>
        public void SaveChunk() {
            parentRegion.chunkStreams[rcX + rcZ * 32] = ZipChunk(Write());
        }

        /// <summary>
        /// Generates object from unzipped chunk data. Do not use with the MemoryStreams held in region object.
        /// </summary>
        /// <param name="data">Unzipped chunk data in form of a memory stream.</param>
        public void Read(MemoryStream data) {

            BinaryReader reader = new BinaryReader(data);

            xm = reader.ReadInt32();
            mm = reader.ReadInt32();
            rm = reader.ReadInt32();

            savedInWorldTicks = reader.ReadUInt64();

            for (int i = 0; i < 64; ++i) {
                bool flag = reader.ReadBoolean();

                if (flag) {
                    cbl[i] = new ChunkBlockLayer();
                    cbl[i].Read(reader);
                }
            }

            cbc = new ChunkBlockChannel();
            cbc.Read(reader);

            reader.Read(im, 0, 256);
            reader.Read(terrainHeight, 0, 256);
            reader.Read(biomeId, 0, 256);
            reader.Read(biomeIntensity, 0, 1536);

            dominantBiome = reader.ReadByte();
            areaMasterDominantBiome = reader.ReadByte();

            int chunkCustomDataCount = (int)reader.ReadUInt16();

            for (int j = 0; j < chunkCustomDataCount; ++j) {
                ChunkCustomData chunkCustomData = new ChunkCustomData();
                chunkCustomData.Read(reader);
                chunkCustomDataDictionary[chunkCustomData.key] = chunkCustomData;
            }

            reader.Read(pr, 0, 256);
            reader.Read(jr, 0, 256);
            reader.Read(fr, 0, 256);

            cm.Read(reader);
            vm.Read(reader);
            gm.Read(reader);
            km.Read(reader);

            needsLightCalculation = reader.ReadBoolean();

            int entityCount = reader.ReadInt32();

            for (int k = 0; k < entityCount; ++k) {
                EntityCreationData entityCreationData = new EntityCreationData();
                entityCreationData.Read(reader);
                entityCreationDataList.Add(entityCreationData);
            }

            int tileEntityCount = reader.ReadInt32();

            for (int l = 0; l < tileEntityCount; ++l) {
                TileEntityType type = (TileEntityType)reader.ReadInt32();
                TileEntity tileEntity = TileEntity.Instantiate(type);
                tileEntity.Read(reader);

                tileEntityDictionary[tileEntity.localChunkPosition] = tileEntity;
            }

            int entitySpawnerCount = (int)reader.ReadUInt16();
            entitySpawnerSaveVersion = (uint)reader.ReadByte();

            for (int m = 0; m < entitySpawnerCount; ++m) {
                EntitySpawner entitySpawner = new EntitySpawner();
                entitySpawner.Read(reader);
                entitySpawnerList.Add(entitySpawner);
            }

            bool flag2 = reader.ReadBoolean();

            if (flag2) {
                ur = new ushort[16];
                for (int n = 0; n < 16; ++n) {
                    ur[n] = reader.ReadUInt16();
                }
            }
        }

        /// <summary>
        /// Unzipps the data in the region object and returns it as a stream.
        /// </summary>
        /// <param name="zippedChunk">MemoryStream with zipped data (one in the region object).</param>
        /// <returns>An unzipped MemoryStream.</returns>
        public MemoryStream UnzipChunk(MemoryStream zippedChunk) {

            BinaryReader reader = new BinaryReader(zippedChunk);

            reader.BaseStream.Position += 4;
            version = reader.ReadUInt32();

            ZipInputStream zipInputStream = new ZipInputStream(zippedChunk);
            zipInputStream.GetNextEntry();

            MemoryStream unZippedChunk = new MemoryStream();
            Utils.StreamCopy(zipInputStream, unZippedChunk, new byte[4096]);

            unZippedChunk.Seek(0, SeekOrigin.Begin);

            return unZippedChunk;
        }


        /// <summary>
        /// Writes data to a MemoryStream. It still needs to be zipped before saving to the region object.
        /// </summary>
        /// <returns>An unzipped MemoryStream.</returns>
        public MemoryStream Write() {

            MemoryStream unzippedChunk = new MemoryStream();

            BinaryWriter writer = new BinaryWriter(unzippedChunk);

            writer.Write(xm);
            writer.Write(mm);
            writer.Write(rm);

            writer.Write(savedInWorldTicks);

            for (int i = 0; i < 64; ++i) {
                writer.Write(cbl[i] != null);

                if (cbl[i] != null) {
                    cbl[i].Write(writer);
                }
            }

            cbc.Write(writer);

            writer.Write(im);
            writer.Write(terrainHeight);
            writer.Write(biomeId);
            writer.Write(biomeIntensity);

            writer.Write(dominantBiome);
            writer.Write(areaMasterDominantBiome);

            writer.Write((ushort)chunkCustomDataDictionary.Count);

            foreach (KeyValuePair<string, ChunkCustomData> entry in chunkCustomDataDictionary) {
                entry.Value.Write(writer);
            }

            writer.Write(pr);
            writer.Write(jr);
            writer.Write(fr);

            cm.Write(writer);
            vm.Write(writer);
            gm.Write(writer);
            km.Write(writer);

            writer.Write(needsLightCalculation);

            writer.Write(entityCreationDataList.Count);
            for (int j = 0; j < entityCreationDataList.Count; ++j) {
                entityCreationDataList[j].Write(writer);
            }

            writer.Write(tileEntityDictionary.Count);
            foreach (KeyValuePair<Vector3D<int>, TileEntity> entry in tileEntityDictionary) {
                writer.Write((int)entry.Value.GetTileEntityType());
                entry.Value.Write(writer);
            }

            writer.Write((ushort)entitySpawnerList.Count);
            writer.Write((byte)entitySpawnerSaveVersion);

            for (int k = 0; k < entitySpawnerList.Count; ++k) {
                entitySpawnerList[k].Write(writer);
            }

            writer.Write(ur != null);

            if (ur != null) {
                for (int l = 0; l < 16; ++l) {
                    writer.Write(ur[l]);
                }
            }

            return unzippedChunk;
        }

        /// <summary>
        /// Zips the provided MemoryStream into form suitable for saving in the region object.
        /// </summary>
        /// <param name="unzippedChunk">Unzipped MemoryStream to be zipped.</param>
        /// <returns>Zipped MemoryStream for saving in region object.</returns>
        private MemoryStream ZipChunk(MemoryStream unzippedChunk) {

            MemoryStream zippedChunk = new MemoryStream();

            BinaryWriter writer = new BinaryWriter(zippedChunk);
            writer.Write((byte)116);
            writer.Write((byte)116);
            writer.Write((byte)99);
            writer.Write((byte)0);
            writer.Write(version);

            ZipOutputStream zipOutputStream = new ZipOutputStream(zippedChunk);
            zipOutputStream.SetLevel(3);

            ZipEntry entry = new ZipEntry(rcX + "/" + rcZ);
            zipOutputStream.PutNextEntry(entry);

            Utils.StreamCopy(unzippedChunk, zipOutputStream, new byte[4096]);
            zipOutputStream.CloseEntry();

            zipOutputStream.IsStreamOwner = false;
            zipOutputStream.Close();

            zippedChunk.Seek(0, SeekOrigin.Begin);
            zippedChunk.Close();

            return zippedChunk;
        }
    }
}