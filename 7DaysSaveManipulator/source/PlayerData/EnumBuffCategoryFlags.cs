using System;

namespace SevenDaysSaveManipulator.PlayerData {

    [Serializable]
    [Flags]
    public enum EnumBuffCategoryFlags {
        None = 0,
        Sickness = 1,
        Disease = 2,
        ArmorUp = 4,
        ArmorDown = 8,
        StaminaUp = 16,
        StaminaDown = 32,
        HealthUp = 64,
        HealthDown = 128,
        SpeedModifier = 256,
        Bleeding = 512,
        Drowning = 1024,
        Wellness = 2048,
        CoreTemp = 4096,
        Armor = 12,
        Stamina = 48,
        Health = 192
    }
}