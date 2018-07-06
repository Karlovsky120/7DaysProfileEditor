using SevenDaysSaveManipulator.PlayerData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysSaveManipulator.RegionData {

    [Serializable]
    public class TileEntity {

        // 3
        public Value<int> tileEntityVersion;

        public Vector3D<int> localChunkPosition;

        public Value<int> entityId;

        public Value<ulong> WorldTimeHeatMapSomething;

        public virtual void Read(BinaryReader reader) {

            tileEntityVersion = new Value<int>((int)reader.ReadUInt16());
            localChunkPosition = new Vector3D<int>();
            localChunkPosition.x = new Value<int>(reader.ReadInt32());
            localChunkPosition.y = new Value<int>(reader.ReadInt32());
            localChunkPosition.z = new Value<int>(reader.ReadInt32());

            entityId = new Value<int>(reader.ReadInt32());

            WorldTimeHeatMapSomething = new Value<ulong>(reader.ReadUInt64());
        }

        public virtual void Write(BinaryWriter writer) {

            writer.Write((ushort)tileEntityVersion.Get());
            writer.Write(localChunkPosition.x.Get());
            writer.Write(localChunkPosition.y.Get());
            writer.Write(localChunkPosition.z.Get());

            writer.Write(entityId.Get());

            writer.Write(WorldTimeHeatMapSomething.Get());
        }

        public static TileEntity Instantiate(TileEntityType type) {
            switch (type) {
                case TileEntityType.Campfire:
                    return new TileEntityCampfire();
                case TileEntityType.Forge:
                    return new TileEntityForge();
                case TileEntityType.Loot:
                    return new TileEntityLootContainer();
                case TileEntityType.SecureDoor:
                    return new TileEntitySecureDoor();
                case TileEntityType.SecureLoot:
                    return new TileEntitySecureLootContainer();
                case TileEntityType.Sign:
                    return new TileEntitySign();
                case TileEntityType.Trader:
                    return new TileEntityTrader();
                case TileEntityType.VendingMachine:
                    return new TileEntityVendingMachine();
                case TileEntityType.Workstation:
                    return new TileEntityWorkstation();
                default:
                    return new TileEntity();

            }
        }

        public TileEntityType GetTileEntityType() {
            if (GetType() == typeof(TileEntityCampfire)) {
                return TileEntityType.Campfire;
            }
            else if (GetType() == typeof(TileEntityForge)) {
                return TileEntityType.Forge;
            }
            else if (GetType() == typeof(TileEntityLootContainer)) {
                return TileEntityType.Loot;
            }
            else if (GetType() == typeof(TileEntitySecureDoor)) {
                return TileEntityType.SecureDoor;
            }
            else if (GetType() == typeof(TileEntitySecureLootContainer)) {
                return TileEntityType.SecureLoot;
            }
            else if (GetType() == typeof(TileEntitySign)) {
                return TileEntityType.Sign;
            }
            else if (GetType() == typeof(TileEntityTrader)) {
                return TileEntityType.Trader;
            }
            else if (GetType() == typeof(TileEntityVendingMachine)) {
                return TileEntityType.VendingMachine;
            }
            else if (GetType() == typeof(TileEntityWorkstation)) {
                return TileEntityType.Workstation;
            }
            else { 
                return TileEntityType.None;
            }
        }
    }
}
