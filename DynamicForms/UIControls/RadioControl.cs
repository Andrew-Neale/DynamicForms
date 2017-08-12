using System;
using DynamicForms.Models.Xml;
using Newtonsoft.Json;

namespace DynamicForms.UIControls
{
    public class RadioControl : BaseControl
    {
        private Field _field;

        public int SelectedIndex { get; set; } = -1;

        [JsonIgnore]
        public Field Field { get { return _field; }}

		// Required for json deserialisation
		public RadioControl(): base(string.Empty) { }

        public RadioControl(Field field) : base(field.Type)
        {
            _field = field;
        }

        public override bool IsValid()
        {
			// Validate Required field constraint
			if (_field.IsRequired && SelectedIndex < 0 )
            {
                ErrorMessage = $"{_field.Caption} is required";
                return false;
            }

            return true;
        }
    }
}
