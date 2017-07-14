using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysSaveManipulator.RegionData {
    public enum TileEntityType : byte {
        None,
        Loot = 5,
        Trader,
        VendingMachine,
        Forge,
        Campfire,
        SecureLoot,
        SecureDoor,
        Workstation,
        Sign
    }
}
