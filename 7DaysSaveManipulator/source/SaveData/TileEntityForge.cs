using System;
using System.Collections.Generic;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    class TileEntityForge : TileEntity {

        //lastTickTime
        Value<ulong> lastTickTime;

        //fuel
        List<ItemStack> fuel;

        //input
        List<ItemStack> input;

        //mold
        ItemStack mold;

        //output
        ItemStack output;

        //fuelInForgeInTicks
        Value<int> fuelInForgeInTicks;

        //moldedMetalSoFar
        Value<short> moldedMetalSoFar;

        //metalInForge
        Value<short> metalInForge;

        //burningItemValue
        ItemValue burningItemValue;

        public TileEntityForge() {}

        internal TileEntityForge(TypedBinaryReader reader, AdditionalFileData xmlData) {
            Read(reader, xmlData);
        }

        internal override void Read(TypedBinaryReader reader, AdditionalFileData xmlData) {
            base.Read(reader);

            lastTickTime = new Value<ulong>(reader);

            byte fuelStackLength = reader.ReadByte();
            fuel = new List<ItemStack>(fuelStackLength);
            for (byte i = 0; i < fuelStackLength; ++i) {
                fuel[i] = new ItemStack(reader, xmlData);
            }

            byte inputStackLength = reader.ReadByte();
            input = new List<ItemStack>(inputStackLength);
            for (byte i = 0; i < inputStackLength; ++i) {
                input[i] = new ItemStack(reader, xmlData);
            }

            mold = new ItemStack(reader, xmlData);
            output = new ItemStack(reader, xmlData);

            fuelInForgeInTicks = new Value<int>(reader);
            moldedMetalSoFar = new Value<short>(reader);
            metalInForge = new Value<short>(reader);
            burningItemValue = new ItemValue(reader, xmlData);   
        }


        internal override void Write(TypedBinaryWriter writer) {
            base.Write(writer);

            lastTickTime.Write(writer);

            writer.Write((byte)fuel.Count);
            foreach(ItemStack fuelStack in fuel) {
                fuelStack.Write(writer);
            }

            writer.Write((byte)input.Count);
            foreach(ItemStack inputStack in input) {
                inputStack.Write(writer);
            }

            mold.Write(writer);
            output.Write(writer);

            fuelInForgeInTicks.Write(writer);
            moldedMetalSoFar.Write(writer);
            moldedMetalSoFar.Write(writer);
            burningItemValue.Write(writer);
        }
    }
}
