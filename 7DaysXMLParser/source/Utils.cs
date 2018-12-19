using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SevenDaysXMLParser.source {
    class Utils {
        internal static XmlDocument LoadXml(String path) {
            XmlDocument xml = new XmlDocument();
            using (FileStream fileStream = new FileStream(path, FileMode.Open)) {
                xml.Load(fileStream);
                return xml;
            }
        }

        internal static Object GetAttribute<T>(XmlElement e, string attribute, T defaultValue) {
            switch (Type.GetTypeCode(typeof(T)) {
                case (TypeCode.Boolean): {
                        if (bool.TryParse(e.GetAttribute(attribute), out bool result)) {
                            return result;
                        }
                        return defaultValue;
                    }
                case (TypeCode.Byte): {
                        if (byte.TryParse(e.GetAttribute(attribute), out byte result)) {
                            return result;
                        }
                        return defaultValue;
                    }
            }
            return null;
        }
    }
}
