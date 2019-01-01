using SevenDaysSaveManipulator;
using SevenDaysSaveManipulator.SaveData;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Tester {
    class Program {
        static void Main(string[] args) {
            /*NameIdMapping tes1 = new NameIdMapping("C:\\Users\\Karlovsky120\\AppData\\Roaming\\7DaysToDie\\Saves\\Navezgane\\A17b233\\blockmappings.nim",
                "C:\\Users\\Karlovsky120\\AppData\\Roaming\\7DaysToDie\\Saves\\Navezgane\\A17b233\\itemmappings.nim",
                "blocks.xml",
                "items.xml",
                "item_modifiers.xml");

            tes1.WriteToFile("blockMappings.b", "itemsMappings.b");

            BinaryReader r = new BinaryReader(new FileStream("C:\\Users\\Karlovsky120\\AppData\\Roaming\\7DaysToDie\\Saves\\Navezgane\\A17b233\\itemmappings.nim", FileMode.Open));
            r.ReadInt32();
            int count = r.ReadInt32();

            SortedDictionary<int, string> dic = new SortedDictionary<int, string>();

            for (int i = 0; i < count; ++i) {
                int id = r.ReadInt32() - 32768;
                string name = r.ReadString();
                dic[id] = name;
            }


            XmlData.Initialize("quests.xml", "", "", "");

            count = XmlData.GetObjectiveCount("quest_BasicSurvival1");*/



            XmlDocument quests = new XmlDocument();
            quests.Load("quests.xml");

            XmlDocument items = new XmlDocument();
            items.Load("items.xml");

            XmlDocument blocks = new XmlDocument();
            blocks.Load("blocks.xml");

            XmlDocument itemModifiers = new XmlDocument();
            quests.Load("quests.xml");

            XmlDocument traders = new XmlDocument();
            quests.Load("quests.xml");

            /*string value = "test";
            string param = "quest:id@tier6_clear\\reward:type@LootItem&id@questMelee,questRanged&ischosen@true&value";
            string param2 = "quest:id@tier6_clear\\reward:name@maxRange";
            string param3 = "block:name@terrOreSandPlusLead\\property:name@FilterTags";
            string param4 = "block:name@terrOreSandPlusLead\\property:name@FilterTags&value";
            string param5 = "block:name@terrForestWGrass1\\drop:event@Harvest&name@resourceClayLump&count";
            bool test = XmlData.XmlAttributeExists(blocks, param3);
            string test2 = XmlData.GetXmlAttributeValue(blocks, param5);*/


            PlayerDataFile file = new PlayerDataFile("76561198004739854.ttp", "blockmappings.nim", "itemmappings.nim", blocks, items, itemModifiers, quests, traders);

            file.Save("76561198004739854.written.ttp");
        }
    }
}