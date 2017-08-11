using System;
using System.IO;
using System.Xml.Linq;

namespace DynamicForms.Tests.Helpers
{
	public static class XDocumentFactory
	{
		public static XDocument XmlWithOneForm()
		{
			var xml = File.ReadAllText(Path.Combine("XmlFiles", "OneForm.xml"));
			return XDocument.Parse(xml);
		}

        public static XDocument XmlWithManyForms()
        {
			var xml = File.ReadAllText(Path.Combine("XmlFiles", "ManyForms.xml"));
			return XDocument.Parse(xml);
        }

		public static XDocument XmlEasyValidationForm()
		{
			var xml = File.ReadAllText(Path.Combine("XmlFiles", "EasyValidationXml.xml"));
			return XDocument.Parse(xml);
		}
	}
}
