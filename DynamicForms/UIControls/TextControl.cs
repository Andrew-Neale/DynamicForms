using System;
using DynamicForms.Models.Xml;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace DynamicForms.UIControls
{
    public class TextControl : BaseControl
    {
        private Field _field;

		public string Value { get; set; }

        [JsonIgnore]
		public Field Field { get { return _field; } }

		// Required for json deserialisation
		public TextControl(): base(string.Empty) { }

        public TextControl(Field field) : base(field.Type)
        {
            _field = field;
        }

		public override bool IsValid()
		{
			if (_field.IsRequired)
			{
				if (string.IsNullOrEmpty(Value))
				{
                    ErrorMessage = $"{_field.Caption} is required";
					return false;
				}
			}

			if (_field.MaxLength != 0)
			{
				if (Value.Length > _field.MaxLength)
				{
                    ErrorMessage = $"{_field.Caption} must be less than {_field.MaxLength} characters";
					return false;
				}
			}

            if (_field.MaxDecimalPlaces > 0)
            {
                if (!string.IsNullOrEmpty(Value))
                {
                    string[] tokens = Value.Split(new char[] { '.' });

                    if (tokens.Length >= 1)
                    {
                        int decimals = tokens[1].Length;

                        if (decimals > _field.MaxDecimalPlaces)
                        {
                            ErrorMessage = $"{_field.Caption} has too many decimal places (max {_field.MaxDecimalPlaces})";
                            return false;
                        }
                    }
                }
            }

			return true;
		}
    }
}

