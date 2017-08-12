using System;
using System.Xml.Linq;

namespace DynamicForms.Models.Xml
{
    public class XmlCommand
    {
        public string Type { get; private set; }
        public string Caption { get; private set; }
        public int Order { get; private set; }

        public XmlCommand(XElement commandElement)
        {
			Type = commandElement.Attribute("type").Value;
			Caption = commandElement.Attribute("caption")?.Value;
			Order = Convert.ToInt32(commandElement.Attribute("order")?.Value);
		}
    }
}
