using System;
using System.Collections.Generic;
using System.Reflection;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    public abstract class TileEntity {
        public enum TileEntityType : byte {
            None,
            Loot = 5,
            Trader,
            VendingMachine,
            Forge,
            Campfire, //NoLongerUsed
            SecureLoot,
            SecureDoor,
            Workstation,
            Sign,
            GoreBlock,
            Powered,
            PowerSource,
            PowerRangeTrap,
            Light,
            Trigger,
            Sleeper
        }

        //localChunkPos
        public Vector3D<int> localChunkPosition;

        //entityId
        public Value<int> entityId;

        //LastHeapMapTile
        public Value<ulong> lastHeapMapTime;

        private readonly static Dictionary<TileEntityType, Type> bindings = new Dictionary<TileEntityType, Type>() {
            {TileEntityType.Forge, typeof(TileEntityForge)},
            {TileEntityType.Loot, typeof(TileEntityLootContainer)},
            {TileEntityType.SecureDoor, typeof(TileEntitySecureDoor)},
            {TileEntityType.SecureLoot, typeof(TileEntityLootContainer)},
            {TileEntityType.Sign, typeof(TileEntitySign)},
            {TileEntityType.Trader, typeof(TileEntityTrader)},
            {TileEntityType.VendingMachine, typeof(TileEntityVendingMachine)},
            {TileEntityType.Workstation, typeof(TileEntityWorkstation)},
            {TileEntityType.GoreBlock, typeof(TileEntityGoreBlock)},
            {TileEntityType.Powered, typeof(TileEntityPoweredBlock)},
            {TileEntityType.PowerSource, typeof(TileEntityPowerSource)},
            {TileEntityType.PowerRangeTrap, typeof(TileEntityPoweredRangedTrap)},
            {TileEntityType.Light, typeof(TileEntityLight)},
            {TileEntityType.Sleeper, typeof(TileEntitySleeper)}
        };

        internal TileEntity() {}

        internal TileEntity(TypedBinaryReader reader) {
            Read(reader);
        }

        internal virtual void Read(TypedBinaryReader reader, XmlData xmlData = null) {
            Utils.VerifyVersion(reader.ReadUInt16(), SaveVersionConstants.TILE_ENTITY);
            localChunkPosition = new Vector3D<int>(reader);

            entityId = new Value<int>(reader);
            lastHeapMapTime = new Value<ulong>(reader);
        }

        internal virtual void Write(TypedBinaryWriter writer) {
            writer.Write(SaveVersionConstants.TILE_ENTITY);
            localChunkPosition.Write(writer);

            entityId.Write(writer);
            lastHeapMapTime.Write(writer);
        }

        internal static TileEntity Instantiate(TileEntityType type, TypedBinaryReader reader, XmlData xmlData) {
            return (TileEntity)bindings[type].GetTypeInfo().GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)[0].Invoke(new object[] { reader, xmlData });
        }

        internal TileEntityType GetTileEntityType() {
            foreach (KeyValuePair<TileEntityType, Type> entry in bindings) {
                if (GetType() == entry.Value) {
                    return entry.Key;
                }
            }

            throw new ArgumentException(string.Format("TileEntity of type {0} is not liste in {1}", GetType(), typeof(TileEntityType)));
        }
    }
}
