using System;
using System.Xml.Linq;

namespace DynamicForms.Models.Xml
{
    public class Option
    {
        public int Order { get; private set; }
        public string Text { get; private set; }

        public Option(XElement optionElement)
        {
			Text = optionElement.Attribute("text")?.Value;
			Order = Convert.ToInt32(optionElement.Attribute("order")?.Value);
		}
    }
}