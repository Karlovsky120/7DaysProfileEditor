using System;
using System.Collections.Generic;

namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    class TileEntityPowered : TileEntity {

        public enum PoweredItemType : byte {
            None,
            Consumer,
            ConsumerToggle,
            Trigger,
            Timer,
            Generator,
            SolarPanel,
            BatteryBank,
            RangedTrap,
            ElectricWireRelay,
            TripWireRelay,
            PressurePlate
        }

        //CenteredPitch
        public Value<float> centeredPitch;

        //CenteredYaw
        public Value<float> centeredYaw;

        //isPlayerPlaced
        public Value<bool> isPlayerPlaced;

        //parentPosition
        public Vector3D<int> parentPosition;

        //PowerItemType
        public Value<PoweredItemType> powerItemType;

        //wireDataList
        public List<Vector3D<int>> wireDataList;

        public TileEntityPowered() {}

        internal TileEntityPowered(TypedBinaryReader reader) {
            Read(reader);
        }

        internal override void Read(TypedBinaryReader reader, AdditionalFileData xmlData = null) {
            base.Read(reader);

            Utils.VerifyVersion(reader.ReadInt32(), SaveVersionConstants.TILE_ENTITY_POWERED, "TileEntityPowered");
            isPlayerPlaced = new Value<bool>(reader);
            powerItemType = new Value<PoweredItemType>((PoweredItemType)reader.ReadByte());

            byte wireDataListCount = reader.ReadByte();
            wireDataList = new List<Vector3D<int>>();
            for (int i = 0; i < wireDataListCount; ++i) {
                wireDataList.Add(new Vector3D<int>(reader));
            }

            parentPosition = new Vector3D<int>(reader);
            centeredPitch = new Value<float>(reader);
            centeredYaw = new Value<float>(reader);
        }

        internal override void Write(TypedBinaryWriter writer) {
            base.Write(writer);

            writer.Write(SaveVersionConstants.TILE_ENTITY_POWERED);
            isPlayerPlaced.Write(writer);
            writer.Write((byte)powerItemType.Get());
            powerItemType.Write(writer);

            writer.Write(wireDataList.Count);
            foreach (Vector3D<int> wireData in wireDataList) {
                wireData.Write(writer);
            }

            parentPosition.Write(writer);
            centeredPitch.Write(writer);
            centeredYaw.Write(writer);
        }
    }
}
