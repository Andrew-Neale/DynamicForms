using System;
using System.Linq;
using DynamicForms.Models.Xml;
using DynamicForms.Tests.Helpers;
using DynamicForms.UIControls;
using NUnit.Framework;

namespace DynamicForms.Tests.UIControls
{
    [TestFixture]
    public class TextControlTests
    {
        [Test]
        public void CanCreateTextControl()
        {
			// setup
			var textField = new Field(XDocumentFactory.XmlWithOneForm().Element("forms").Element("form").Element("fields").Element("field"));

            // Act
			var textControl = new TextControl(textField);

            // Verify
            Assert.IsNotNull(textControl.Field);
            Assert.AreEqual("text", textControl.Type);
            Assert.IsNull(textControl.ErrorMessage);
            Assert.IsNull(textControl.Value);
            Assert.IsFalse(textControl.IsValid());
        }

        [Test]
        public void CanCreateValidRequiredTextField()
        {
			// setup
			var textField = new Field(XDocumentFactory.XmlWithOneForm().Element("forms").Element("form").Element("fields").Element("field"));
			var textControl = new TextControl(textField);

            // Act
            textControl.Value = "some value";

			// Verify
			Assert.IsTrue(textControl.IsValid());
            Assert.IsNull(textControl.ErrorMessage);
        }


        [TestCase(101)]
        [TestCase(102)]
        public void MaxLengthConstraintWillFailValidation(int count)
        {
			// setup
			var textField = new Field(XDocumentFactory.XmlWithOneForm().Element("forms").Element("form").Element("fields").Element("field"));
			var textControl = new TextControl(textField);

            // Act
            textControl.Value = new string('a', count);

			// Verify
			Assert.IsFalse(textControl.IsValid());
			Assert.AreEqual("First Name must be less than 100 characters", textControl.ErrorMessage);
        }

	    [Test]
		public void MaxLengthConstraintWillPassWhenLenghtIsSame()
		{
			// setup
			var textField = new Field(XDocumentFactory.XmlWithOneForm()
                                      .Element("forms")
                                      .Element("form")
                                      .Element("fields")
                                      .Element("field"));
            
			var textControl = new TextControl(textField);

			// Act
			textControl.Value = new string('a', 100);

			// Verify
			Assert.IsTrue(textControl.IsValid());
			Assert.IsNull(textControl.ErrorMessage);
		}

        [Test]
        public void TooManyDecimalPlacesConstraintWillFire()
        {
            // setup
            var grossSalaryField = XDocumentFactory.XmlWithOneForm()
                                                   .Element("forms")
                                                   .Element("form")
                                                   .Element("fields")
                                                   .Elements("field")
                                                   .ToList()[10];
            
            var textField = new Field(grossSalaryField);
			var textControl = new TextControl(textField);

            // Act
            textControl.Value = "11.0000";

			// Verify
			Assert.IsFalse(textControl.IsValid());
            Assert.AreEqual("Gross Salary has too many decimal places (max 2)", textControl.ErrorMessage);
        }

		[TestCase("10")]
		[TestCase("10.")]
		[TestCase("10.0")]
		[TestCase("10.00")]
		public void DecimalPlacesConstraintWillPass(string validValues)
		{
			// setup
            // get field with decimal place constraint
			var grossSalaryField = XDocumentFactory.XmlWithOneForm()
												   .Element("forms")
												   .Element("form")
												   .Element("fields")
												   .Elements("field")
												   .ToList()[10];

			var textField = new Field(grossSalaryField);
			var textControl = new TextControl(textField);

			// Act
			textControl.Value = validValues;

			// Verify
			Assert.IsTrue(textControl.IsValid());
			Assert.IsNull(textControl.ErrorMessage);
		}
    }
}
