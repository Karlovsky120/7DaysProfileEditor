using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysSaveManipulator.source.PlayerData
{

    class SaveVersionConstants {
        public const byte BASE_OBJECTIVE = 255;
        public const int  BODY_DAMAGE = 2;
        public const byte ENTITY_CREATION_DATA = 26;
        public const int  ENTITY_STATS = 8;
        public const byte EQUIPMENT = 1;
        public const byte ITEM_VALUE = 5;
        public const byte PLAYER_DATA_FILE = 40;
        public const byte QUEST_JOURNAL = 1;
        public const byte QUEST_FILE = 4;
        public const byte QUEST_QUEST = 255;
        public const byte WAYPOINT_COLLECTION = 2;

        private SaveVersionConstants() {}
    }
}
