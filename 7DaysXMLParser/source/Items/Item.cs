using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SevenDaysXMLParser.source.Items {
    class Item {
        public readonly string name;

        public readonly string parentName = "";

        public readonly int stackNumber = -1;
        public readonly int durabilityLow = -1;
        public readonly int durabilityHigh = -1;
        public readonly int modSlotCount = -1;

        internal Item(XmlElement e) {
            if (!e.HasAttribute("name")) {
                throw new XmlException("Item does not have name assigned!");
            }

            name = e.GetAttribute("name");

            foreach (XmlElement child in e.ChildNodes) {
                switch (child.Name) {
                    case ("Extends"):
                        parentName = child.GetAttribute("value");
                        break;
                    case ("Stacknumber"):
                        stackNumber = (int)Utils.GetAttribute<int>(child, "value", 1000);
                        break;
                    case ("Base Effects"):
                        foreach (XmlElement effect in child.ChildNodes) {
                            switch (effect.Name) {
                                case ("DegradationMax"):
                                    string value = effect.GetAttribute("value");
                                    string[] limits = value.Split(',');
                                    durabilityLow = int.Parse(limits[0]);
                                    durabilityHigh = int.Parse(limits[1]);
                                    break; 
                                case ("ModSlots"):
                                    modSlotCount = (int)Utils.GetAttribute<int>(effect, "value", 0);
                                    break;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
