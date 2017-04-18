using SevenDaysSaveManipulator.PlayerData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysSaveManipulator.RegionData {
    public class TileEntityLootContainer : TileEntity {

        Value<int> lootListIndex;
        Vector3D<int> AD;
        Value<bool> bTouched;
        Value<ulong> worldTimeTouched;
        Value<bool> bPlayerBackpack;

        ItemStack[] items;

        Value<bool> bPlayerStorage;

        public override void Read(BinaryReader reader) {

            base.Read(reader);

            lootListIndex = new Value<int>((int)reader.ReadUInt16());
            AD = new Vector3D<int>();
            AD.x = new Value<int>((int)reader.ReadUInt16());
            AD.y = new Value<int>((int)reader.ReadUInt16());
            bTouched = new Value<bool>(reader.ReadBoolean());
            worldTimeTouched = new Value<ulong>((ulong)reader.ReadUInt32());
            bPlayerBackpack = new Value<bool>(reader.ReadBoolean());

            int itemsLength = Math.Min((int)reader.ReadInt16(), AD.x.Get() * AD.y.Get());
            items = new ItemStack[itemsLength];

            for (int i = 0; i < itemsLength; i++) {
                items[i] = new ItemStack();
                items[i].Read(reader);
            }

            bPlayerStorage = new Value<bool>(reader.ReadBoolean());
        }

        public override void Write(BinaryWriter writer) {
            base.Write(writer);

            writer.Write((ushort)lootListIndex.Get());
            writer.Write((ushort)AD.x.Get());
            writer.Write((ushort)AD.y.Get());
            writer.Write(bTouched.Get());
            writer.Write((uint)worldTimeTouched.Get());
            writer.Write(bPlayerBackpack.Get());
            writer.Write((short)items.Length);

            for (int i = 0; i < items.Length; ++i) {
                items[i].Write(writer);
            }

            writer.Write(bPlayerStorage.Get());
        }
    }
}
