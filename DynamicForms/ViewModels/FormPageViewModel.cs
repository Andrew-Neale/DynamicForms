using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DynamicForms.Models.Device;
using DynamicForms.Models.Io;
using DynamicForms.Models.Messaging;
using DynamicForms.Models.Xml;
using DynamicForms.UIControls;
using Xamarin.Forms;

namespace DynamicForms.ViewModels
{
    public class FormPageViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private readonly IMessageService _messageService;
        private readonly IFileReaderWriter _filerReaderWriter;
        private readonly IDevice _device;

		private ICommand _closePageCommand;
		private ICommand _saveFormCommand;
		private Form _form;

		public ObservableRangeCollection<BaseControl> FormFields { get; private set; } = new ObservableRangeCollection<BaseControl>();

        public ICommand ClosePageCommand => _closePageCommand ?? (_closePageCommand = new Command( ()=> DoClosePageCommand()));
		public ICommand SaveFormCommand => _saveFormCommand ?? (_saveFormCommand = new Command(() => DoSaveFormCommand()));

        public FormPageViewModel(Form selectedForm, INavigation navigation, IMessageService messageService, IFileReaderWriter fileReaderWriter, IDevice device)
        {
            _form = selectedForm;
            _navigation = navigation;
            _messageService = messageService;
            _filerReaderWriter = fileReaderWriter;
            _device = device;

            Title = selectedForm.Title;

            var savedForm = _filerReaderWriter.LoadForm(GenerateFilename());

            if (savedForm.Count > 0)
            {
                PopulateForm(savedForm);
            }
            else
            {
                CreateEmptyForm();
            }
        }

        private void CreateEmptyForm()
        {
            foreach (var field in _form.Fields)
            {
                if (field.Type == "text" || 
                    field.Type == "numeric" || 
                    field.Type == "decimal" || 
                    field.Type == "document" ||
                    field.Type =="comment")
                {
                    FormFields.Add(new TextControl(field));
                }
                else if (field.Type == "date")
                {
                    FormFields.Add(new DateTimeControl(field));
                }
                else if (field.Type == "radio" || field.Type == "dropdown")
                {
                    FormFields.Add((new RadioControl(field)));
                }
            }
        }

        private void PopulateForm(List<BaseControl> savedForm)
        {
            for (int i = 0; i < _form.Fields.Count; i++)
            {
                var field = _form.Fields[i];

                if (field.Type == "text" || 
                    field.Type == "numeric" || 
                    field.Type == "decimal" || 
                    field.Type == "document" ||
                    field.Type == "comment")
                {
                    FormFields.Add(new TextControl(field) 
                    {
                        Value = (savedForm[i] as TextControl)?.Value
                    });
                }
                else if (field.Type == "date")
                {
                    FormFields.Add(new DateTimeControl(field)
                    {
                        Value = (savedForm[i] as DateTimeControl).Value
                    });
                }
                else if (field.Type == "radio" || field.Type == "dropdown")
                {
                    FormFields.Add(new RadioControl(field)
                    {
                        SelectedIndex = (savedForm[i] as RadioControl).SelectedIndex
                    });
                }
            }
        }

        private void DoClosePageCommand()
		{
            _device.BeginInvokeOnMainThread(async () => await _navigation.PopAsync());
		}

		private void DoSaveFormCommand()
        {
            BaseControl failedField = ValidateForm();

            if (failedField != null)
            {
                _messageService.Send<string>("FailedFormValidation", failedField.ErrorMessage);
            }
            else
            {
                _filerReaderWriter.SaveForm(FormFields.ToList(), GenerateFilename());
                _messageService.Send<string>("SavedSuccessfull", this);
            }
        }

        private BaseControl ValidateForm()
        {
			BaseControl failedField = null;

            foreach (var field in FormFields)
            {
                if (!field.IsValid())
                {
                    failedField = field;
                    break;
                }
            }

            return failedField;
        }

        private string GenerateFilename()
        {
            return Title.Replace(" ", string.Empty);
        }
    }
}

