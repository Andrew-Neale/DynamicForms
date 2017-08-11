using System;
using System.Collections.Generic;
using DynamicForms.Models.Io;
using DynamicForms.Models.Messaging;
using DynamicForms.Models.Xml;
using DynamicForms.TemplateSelectors;
using DynamicForms.UIControls;
using DynamicForms.ViewModels;
using Xamarin.Forms;

namespace DynamicForms.Views
{
    public partial class FormPage : ContentPage
    {
        private FormPageViewModel _viewModel;

        public FormPage(Form selectedForm)
        {
			InitializeComponent();

            var messageService = new MessageService();
            var fileReaderWriter = new FileReaderWriter();
            var device = new DynamicForms.Models.Device.Device();

            BindingContext = _viewModel = new FormPageViewModel(selectedForm, Navigation, messageService, fileReaderWriter, device);
            BuildButtons(selectedForm.Commands);

            messageService.Subscribe<string>(this, (source, key) =>
             {
                 if (key == "FailedFormValidation")
                 {
                     
                     DisplayAlert("Validation Failed", source as string, "OK");
                 }
                 else if (key == "SavedSuccessfull")
                 {
                     DisplayAlert("Saved", "Form saved successfully", "OK");
                 }
             });

        }

        private void BuildButtons(List<XmlCommand> commands)
        {
            foreach (var command in commands)
            {
                Button button = null;
				switch (command.Type)
				{
					case "Close":
                        button = new Button()
                        {
                            Text = command.Caption,
                            Command = new Command((obj) => _viewModel.ClosePageCommand.Execute(0)),
                            Margin= new Thickness(10,10,10,10)
                                
                        };
                        break;
					case "Save":
						button = new Button()
						{
							Text = command.Caption,
                            Command = new Command((obj) => _viewModel.SaveFormCommand.Execute(0)),
                            Margin = new Thickness(10, 10, 10, 10)
						};
                        break;
				}

				CommandsContainer.Children.Add(button);
            }
        }
    }
}
