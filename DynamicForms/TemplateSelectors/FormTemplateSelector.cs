using System;
using DynamicForms.Models.Xml;
using DynamicForms.UIControls;
using Xamarin.Forms;

namespace DynamicForms.TemplateSelectors
{
    public class FormTemplateSelector : DataTemplateSelector
    {
        DataTemplate TextFieldTemplate { get; set; }
        DataTemplate RadioTemplate { get; set; }
        DataTemplate DatePickerTemplate { get; set; }
        DataTemplate NumberTemplate { get; set; }
        DataTemplate DocumentTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var field = item as BaseControl;

            switch (field.Type)
            {
                case "text":
                    return TextFieldTemplate;
                case "dropdown":
                case "radio":
                    return RadioTemplate;
                case "date":
                    return DatePickerTemplate;
                case "numeric":
                case "decimal":
                    return NumberTemplate;
                case "document":
                case "comment":
                    return DocumentTemplate;
            }

            return TextFieldTemplate;
        }
    }
}
