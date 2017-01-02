using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace SevenDaysProfileEditor.Data {

    /// <summary>
    /// Currently not implemented.
    /// </summary>
    internal class XmlNullReferenceException : NullReferenceException {
        private string error;

        public XmlNullReferenceException(string reference, XmlNode node) {
            List<XmlNode> nodePath = new List<XmlNode>();
            XmlNode counterNode = node;

            while (counterNode != null) {
                nodePath.Add(counterNode);
                counterNode = counterNode.ParentNode;
            }

            nodePath.RemoveAt(nodePath.Count - 1);

            StringBuilder s = new StringBuilder("An error occured while reading ");

            foreach (XmlNode childNode in node.OwnerDocument.ChildNodes) {
                if (!(childNode.Name.Equals("xml") || childNode.Name.Equals("#comment"))) {
                    s.Append(childNode.Name + ".xml");
                    break;
                }
            }

            for (int i = nodePath.Count - 1; i > -1; --i) {
                s.Append("\nat ");

                if (nodePath[i].Attributes.Count > 0) {
                    s.AppendFormat("{0}={1}", nodePath[i].Attributes[0].Name, nodePath[i].Attributes[0].Value);
                }
                else {
                    s.Append(nodePath[i].Name);
                }
            }

            s.AppendFormat("\nwhere \"{0}\" was expected, but not found.", reference);

            error = s.ToString();
        }

        public override string ToString() {
            return error;
        }
    }
}