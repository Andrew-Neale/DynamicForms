using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DynamicForms.Models.Xml
{
    public class Form : ObservableObject
    {
        public string Title { get; private set; }
        public List<Field> Fields { get; private set; } = new List<Field>();
        public List<XmlCommand> Commands { get; private set; } = new List<XmlCommand>();

        public Form(XElement formElement)
        {
            if (formElement == null)
            { 
                return;
            }

            Title = formElement.Attribute("title").Value;

            foreach (var fieldElement in formElement.Elements("fields").Elements("field"))
            {
                Fields.Add(new Field(fieldElement));
            }

			foreach (var fieldElement in formElement.Elements("commands").Elements("command"))
			{
				Commands.Add(new XmlCommand(fieldElement));
			}
        }

    }
}
