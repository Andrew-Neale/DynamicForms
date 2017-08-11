using System;
using DynamicForms.Models.Xml;
using Newtonsoft.Json;

namespace DynamicForms.UIControls
{
    public class DateTimeControl : BaseControl
    {
        private Field _field;

        public DateTime Value { get; set; } = DateTime.Today;

        [JsonIgnore]
		public Field Field { get { return _field; } }

        // Required for json deserialisation
        public DateTimeControl(): base(string.Empty) { }

        public DateTimeControl(Field field) : base(field.Type)
        {
            _field = field;
        }

        public override bool IsValid()
        {
            return true;
        }

    }
}
