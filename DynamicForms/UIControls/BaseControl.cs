using System;
using DynamicForms.Models.Xml;
using Newtonsoft.Json;

namespace DynamicForms.UIControls
{
    public abstract class BaseControl
    {
        public string Type { get; set; }

        [JsonIgnore]
        public string ErrorMessage { get; set; }

        public BaseControl(string type)
        {
            Type = type;
        }

        public abstract bool IsValid();
    }
}
