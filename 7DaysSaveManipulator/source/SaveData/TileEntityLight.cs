using System;
namespace SevenDaysSaveManipulator.SaveData {

    [Serializable]
    class TileEntityLight : TileEntity {

        //Delay
        public Value<float> delay;

        //LightAngle
        public Value<float> lightAngle;

        //LightColor
        public Value<uint> lightColor;

        //LightIntensity
        public Value<float> lightIntensity;

        //LightRange
        public Value<float> lightRange;

        //LightShadows
        public Value<byte> lightShadows;

        //LightState
        public Value<byte> lightState;

        //LightType
        public Value<byte> lightType;

        //Rate
        public Value<float> rate;

        public TileEntityLight() {}

        internal TileEntityLight(TypedBinaryReader reader) {
            Read(reader);
        }

        internal override void Read(TypedBinaryReader reader, AdditionalFileData xmlData = null) {
            base.Read(reader);

            lightIntensity = new Value<float>(reader);
            lightRange = new Value<float>(reader);
            lightColor = new Value<uint>(reader);

            lightType = new Value<byte>(reader);
            lightAngle = new Value<float>(reader);
            lightShadows = new Value<byte>(reader);

            lightState = new Value<byte>(reader);
            rate = new Value<float>(reader);
            delay = new Value<float>(reader);
        }

        internal override void Write(TypedBinaryWriter writer) {
            base.Write(writer);

            lightIntensity.Write(writer);
            lightRange.Write(writer);
            lightColor.Write(writer);

            lightType.Write(writer);
            lightAngle.Write(writer);
            lightShadows.Write(writer);

            lightState.Write(writer);
            rate.Write(writer);
            delay.Write(writer);
        }
    }
}
