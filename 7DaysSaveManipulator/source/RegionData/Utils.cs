using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysSaveManipulator.RegionData {
    public class Utils {

        public static void WorldChunkToRegionCoordinates(int wcX, int wcZ, out int rX, out int rZ) {

            rX = wcX / 32;
            rZ = wcZ / 32;

            if (wcX < 0) {
                --rX;
            }

            if (wcZ < 0) {
                --rZ;
            }
        }

        public static void WorldChunkToRegionChunkCoordinates(int wcX, int wcZ, out int rcX, out int rcZ) {
            rcX = wcX % 32;
            rcZ = wcZ % 32;

            if (wcX < 0) {
                rcX = 31 - Math.Abs(rcX);
            }

            if (wcZ < 0) {
                rcZ = 31 - Math.Abs(rcZ);
            }
        }

        public static void StreamCopy(Stream source, Stream destination, byte[] buffer, bool bFlush = true) {
            bool flag = true;
            while (flag) {

                int num = source.Read(buffer, 0, buffer.Length);
                if (num > 0) {                    
                    destination.Write(buffer, 0, num);
                }

                else {

                    if (bFlush) {                        
                        destination.Flush();
                    }

                    flag = false;
                }
            }           
        }

        public static void PartiallyCopyStream(Stream origin, Stream destination, int length, int bufferLength = 32786) {

            byte[] buffer = new byte[bufferLength];
            int read;
            while (length > 0 && (read = origin.Read(buffer, 0, Math.Min(buffer.Length, length))) > 0) {
                destination.Write(buffer, 0, read);
                length -= read;
            }
        }
    }
}
