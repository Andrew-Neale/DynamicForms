using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DynamicForms.Models.Xml
{
    public class Forms
    {
        public List<Form> FormViews { get; set; } = new List<Form>();

        public Forms(XDocument document)
        {
            var formElements = document.Element("forms").Elements("form");

            foreach (var formElement in formElements)
            {
                FormViews.Add(new Form(formElement));
            }
        }
    }
}
