using SevenDaysSaveManipulator.PlayerData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysSaveManipulator.RegionData {
    class TraderData {

        public Value<int> traderID;
        Value<ulong> lastInventoryUpdate;
        Value<byte> fileVersion;

        List<ItemStack> primaryInventory = new List<ItemStack>();
        List<ItemStack[]> tierItemGroups = new List<ItemStack[]>();

        Value<int> availableMoney;
        List<sbyte> jj = new List<sbyte>();

        public void Read(BinaryReader reader) {
            traderID = new Value<int>(reader.ReadInt32());
            lastInventoryUpdate = new Value<ulong>(reader.ReadUInt64());
            fileVersion = new Value<byte>(reader.ReadByte());

            primaryInventory.AddRange(ItemStack.ReadItemStack(reader));

            int tierItemGroupsCount = (int)reader.ReadByte();
            for (int i = 0; i < tierItemGroupsCount; ++i) {
                tierItemGroups.Add(ItemStack.ReadItemStack(reader));
            }

            availableMoney = new Value<int>(reader.ReadInt32());

            int sByteCount = reader.ReadInt32();
            for (int j = 0; j < sByteCount; ++j) {
                jj.Add(reader.ReadSByte());
            }
        }

        public void Write(BinaryWriter writer) {
            writer.Write(traderID.Get());
            writer.Write(lastInventoryUpdate.Get());
            writer.Write(fileVersion.Get());

            ItemStack.WriteItemStack(writer, primaryInventory.ToArray());

            writer.Write((byte)tierItemGroups.Count);
            for (int i = 0; i < tierItemGroups.Count; ++i) {
                ItemStack.WriteItemStack(writer, tierItemGroups[i].ToArray());
            }

            writer.Write(availableMoney.Get());

            writer.Write(jj.Count);
            for (int j = 0; j < jj.Count; ++j) {
                writer.Write(jj[j]);
            }
        }
    }
}
