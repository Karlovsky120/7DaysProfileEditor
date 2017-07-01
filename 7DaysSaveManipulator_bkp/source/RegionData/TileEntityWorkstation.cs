using SevenDaysSaveManipulator.PlayerData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysSaveManipulator.RegionData {
    class TileEntityWorkstation : TileEntity {

        Value<int> version;

        Value<ulong> ib;

        ItemStack[] gb;
        ItemStack[] kb;
        ItemStack[] eg;
        ItemStack[] mb;

        RecipeQueueItem[] oe;

        Value<bool> ye;
        Value<float> fe;
        float[] qe;

        public override void Read(BinaryReader reader) {
            base.Read(reader);

            version = new Value<int>((int)reader.ReadByte());
            ib = new Value<ulong>(reader.ReadUInt64());
            
            Vyf(reader, ref gb);
            Vyf(reader, ref kb);
            Vyf(reader, ref eg);
            Vyf(reader, ref mb);

            ReadRecipeStackArray(reader, ref oe);

            ye = new Value<bool>(reader.ReadBoolean());
            fe = new Value<float>(reader.ReadSingle());

            int num2 = (int)reader.ReadByte();
            qe =new float[num2];

            for (int i = 0; i < num2; ++i) {
                qe[i] = reader.ReadSingle();
            }
        }

        public override void Write(BinaryWriter writer) {
            base.Write(writer);

            writer.Write(version.Get());

            writer.Write(ib.Get());

            Kyf(writer, gb);
            Kyf(writer, kb);
            Kyf(writer, eg);
            Kyf(writer, mb);

            WriteRecipeStackArray(writer, oe);

            writer.Write(ye.Get());
            writer.Write(fe.Get());

            writer.Write((byte)qe.Length);
            for (int i = 0; i < qe.Length; ++i) {
                writer.Write(qe[i]);
            }
        }

        private void Vyf(BinaryReader reader, ref ItemStack[] itemStack) {
            int num = (int)reader.ReadByte();

            itemStack = new ItemStack[num];

            for (int i = 0; i < num; ++i) {
                itemStack[i] = new ItemStack();
                itemStack[i].Read(reader);
            }
        }

        private void Kyf(BinaryWriter writer, ItemStack[] itemStack) {
            byte length = (byte)((itemStack == null) ? 0 : ((byte)itemStack.Length));

            writer.Write(length);

            for (int i = 0; i < length; ++i) {
                itemStack[i].Write(writer);
            }
        }

        private void ReadRecipeStackArray(BinaryReader reader, ref RecipeQueueItem[] recipeQueueItem) {
            int num = (int)reader.ReadByte();

            recipeQueueItem = new RecipeQueueItem[num];

            for (int i = 0; i < num; ++i) {
                recipeQueueItem[i] = new RecipeQueueItem();
                recipeQueueItem[i].Read(reader);
            }
        }

        private void WriteRecipeStackArray(BinaryWriter writer, RecipeQueueItem[] recipeQueueItem) {
            byte length = (byte)((recipeQueueItem == null) ? 0 : ((byte)recipeQueueItem.Length));

            writer.Write(length);

            for (int i = 0; i < length; ++i) {
                if (recipeQueueItem[i] == null) {
                    recipeQueueItem[i] = new RecipeQueueItem();
                    recipeQueueItem[i].multiplier = new Value<int>(0);
                    recipeQueueItem[i].recipe = null;
                    recipeQueueItem[i].isCrafting = new Value<bool>(false);
                }

                recipeQueueItem[i].Write(writer);
            }
        }
    }
}
