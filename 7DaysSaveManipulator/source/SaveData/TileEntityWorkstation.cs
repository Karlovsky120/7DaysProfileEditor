using System;
using System.Collections.Generic;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    class TileEntityWorkstation : TileEntity {

        //lastTickTime
        Value<ulong> lastTickTime;

        //fuel
        List<ItemStack> fuel;

        //input
        List<ItemStack> input;

        //tools
        List<ItemStack> tools;

        //output
        List<ItemStack> output;

        //queue
        List<RecipeQueueItem> queue;

        //isBurning
        Value<bool> isBurning;

        //currentBurnTimeLeft
        Value<float> currentBurnTimeLeft;

        //currentMeltTimesLeft
        List<float> currentMeltTimesLeft;

        public TileEntityWorkstation() {}

        internal TileEntityWorkstation(TypedBinaryReader reader, XmlData xmlData) {
            Read(reader, xmlData);
        }

        internal override void Read(TypedBinaryReader reader, XmlData xmlData) {
            base.Read(reader);

            Utils.VerifyVersion(reader.ReadByte(), SaveVersionConstants.TILE_ENTITY_WORKSTATION);

            lastTickTime = new Value<ulong>(reader);
            
            ReadItemStackList(reader, ref fuel, xmlData);
            ReadItemStackList(reader, ref input, xmlData);
            ReadItemStackList(reader, ref tools, xmlData);
            ReadItemStackList(reader, ref output, xmlData);

            byte recipeStackLength = reader.ReadByte();
            queue = new List<RecipeQueueItem>(recipeStackLength);
            for (byte i = 0; i < recipeStackLength; ++i) {
                queue.Add(new RecipeQueueItem(reader, xmlData));
            }

            isBurning = new Value<bool>(reader);
            currentBurnTimeLeft = new Value<float>(reader);

            byte currentMeltTimesLeftCount = reader.ReadByte();
            currentMeltTimesLeft = new List<float>(currentMeltTimesLeftCount);
            for (byte i = 0; i < currentMeltTimesLeftCount; ++i) {
                currentMeltTimesLeft.Add(reader.ReadSingle());
            }
        }

        internal override void Write(TypedBinaryWriter writer) {
            base.Write(writer);

            writer.Write(SaveVersionConstants.TILE_ENTITY_WORKSTATION);

            lastTickTime.Write(writer);

            WriteItemStackList(writer, fuel);
            WriteItemStackList(writer, input);
            WriteItemStackList(writer, tools);
            WriteItemStackList(writer, output);

            writer.Write(queue.Count);
            foreach (RecipeQueueItem recipeQueueItem in queue) {
                recipeQueueItem.Write(writer);
            }

            isBurning.Write(writer);
            currentBurnTimeLeft.Write(writer);

            writer.Write((byte)currentMeltTimesLeft.Count);
            foreach (float currentMeltTimeLeft in currentMeltTimesLeft) {
                writer.Write(currentMeltTimeLeft);
            }
        }

        private void ReadItemStackList(TypedBinaryReader reader, ref List<ItemStack> itemStackList, XmlData xmlData) {
            byte itemStackLength = reader.ReadByte();
            itemStackList = new List<ItemStack>(itemStackLength);
            for (byte i = 0; i < itemStackLength; ++i) {
                itemStackList.Add(new ItemStack(reader, xmlData));
            }
        }

        private void WriteItemStackList(TypedBinaryWriter writer, List<ItemStack> itemStackList) {
            writer.Write((byte)itemStackList.Count);
            foreach (ItemStack itemStack in itemStackList) {
                itemStack.Write(writer);
            }
        }
    }
}
