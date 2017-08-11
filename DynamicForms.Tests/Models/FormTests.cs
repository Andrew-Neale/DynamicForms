using System;
using DynamicForms.Models.Xml;
using DynamicForms.Tests.Helpers;
using NUnit.Framework;

namespace DynamicForms.Tests.Models
{
    [TestFixture()]
    public class FormTests
    {
        [Test()]
        public void CanParseSingleForm()
        {
            // Setup
            var xdocument = XDocumentFactory.XmlWithOneForm();

            // Act
            var forms = new Forms(xdocument);

            // Verify
            Assert.IsNotNull(forms.FormViews);
            Assert.AreEqual(1, forms.FormViews.Count);
            Assert.AreEqual("Personal Details", forms.FormViews[0].Title);

            VerifyForms(forms);
            VerifyCommands(forms);
        }

		[Test()]
		public void CanParseMultipleForms()
		{
			// Setup
			var xdocument = XDocumentFactory.XmlWithManyForms();

			// Act
			var forms = new Forms(xdocument);

			// Verify
			Assert.IsNotNull(forms.FormViews);
			Assert.AreEqual(6, forms.FormViews.Count);
			Assert.AreEqual("Personal Details 1", forms.FormViews[0].Title);
			Assert.AreEqual("Risk Assessment 3", forms.FormViews[5].Title);
		}

        private void VerifyCommands(Forms forms)
        {
            var commands = forms.FormViews[0].Commands;
            Assert.IsNotNull(commands);
            Assert.AreEqual(2, commands.Count);
            AssertCommands("Save", "Save", 1, commands[0]);
            AssertCommands("Close", "Close", 2, commands[1]);
        }

        private void VerifyForms(Forms forms)
        {
            var fields = forms.FormViews[0].Fields;
            Assert.IsNotNull(fields);
            Assert.AreEqual(13, fields.Count);
            AssertFields("text", true, "First Name", 1, 100, fields[0]);
            AssertFields("dropdown", true, "Country", 8, fields[7]);
            AssertFields("document", false, "Document(s)", 13, fields[12]);
        }

        private void AssertCommands(string expectedType, string expectedCaption, int expectedOrder, XmlCommand command)
        {
			Assert.AreEqual(expectedType, command.Type);
			Assert.AreEqual(expectedCaption, command.Caption);
			Assert.AreEqual(expectedOrder, command.Order);
        }

		private void AssertFields(string expectedType, bool expectedRequired, string expectedCaption, int expectedOrder, Field field)
        {
            Assert.AreEqual(expectedType, field.Type);
            Assert.AreEqual(expectedRequired, field.IsRequired);
            Assert.AreEqual(expectedCaption, field.Caption);
            Assert.AreEqual(expectedOrder, field.Order);
        }

        private void AssertFields(string expectedType, bool expectedRequired, string expectedCaption, int expectedOrder, int expectedLength, Field field)
        {
            Assert.AreEqual(expectedType, field.Type);
            Assert.AreEqual(expectedRequired, field.IsRequired );
            Assert.AreEqual(expectedCaption, field.Caption);
            Assert.AreEqual(expectedOrder, field.Order);
            Assert.AreEqual(expectedLength, field.MaxLength);
        }
    }
}
