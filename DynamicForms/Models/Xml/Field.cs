using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Xamarin.Forms;

namespace DynamicForms.Models.Xml
{
    public class Field: BindableObject
    {
        public string Type { get; private set; }
        public bool IsRequired { get; private set; }
        public string Caption { get; private set; }
        public int MaxLength { get; private set; }
        public int Order { get; private set; }
        public int MaxDecimalPlaces { get; private set; }
        public List<Option> Options { get; private set; } = new List<Option>();

        public Field(XElement fieldElement)
        {
            Type = fieldElement.Attribute("type").Value;
            IsRequired = Convert.ToBoolean(fieldElement.Attribute("isrequired")?.Value);
            Caption = fieldElement.Attribute("caption")?.Value;
            Order = Convert.ToInt32(fieldElement.Attribute("order")?.Value);
            MaxLength = Convert.ToInt32(fieldElement.Attribute("length")?.Value);
            MaxDecimalPlaces = Convert.ToInt32(fieldElement.Attribute("decimalplaces")?.Value);

            foreach (var optionElement in fieldElement.Elements("option"))
            {
                Options.Add(new Option(optionElement));
            }
        }
    }
}