using System;
using System.Collections.Generic;
using DynamicForms.ViewModels;
using DynamicForms.Models.Io;
using DynamicForms.Models.Messaging;
using DynamicForms.Models.Xml;
using DynamicForms.Tests.Helpers;
using Moq;
using NUnit.Framework;
using Xamarin.Forms;
using DynamicForms.UIControls;
using DynamicForms.Models.Device;
using System.Threading.Tasks;

namespace DynamicForms.Tests.ViewModels
{
    [TestFixture]
    public class FormPageViewModelTests
    {
        [Test]
        public async Task CanCreateEmptyForm()
        {
            // setup
            var mocker = new HomeViewModelMock();
			mocker.SetupEmptyForm();

            // Act
            await mocker.FormPageViewModel.Initialise();

			// Verify
			Assert.AreEqual("Personal Details", mocker.FormPageViewModel.Title);
            Assert.AreEqual(13, mocker.FormPageViewModel.FormFields.Count);
            mocker.FileReaderWriterMock.VerifyAll();
        }

        [Test]
        public async Task CanDisplaySavedForm()
        {
			// setup
			var mocker = new HomeViewModelMock();
			mocker.SetupSavedForm();

            // Act
            await mocker.FormPageViewModel.Initialise();

			// Verify
			Assert.AreEqual("Personal Details", mocker.FormPageViewModel.Title);
			Assert.AreEqual(13, mocker.FormPageViewModel.FormFields.Count);
			mocker.FileReaderWriterMock.VerifyAll();
        }

        [Test]
        public void CanExecuteClosePageCommand()
        {
            // setup
            var mocker = new HomeViewModelMock();

            mocker.SetupSavedForm();
            mocker.NavigationMock.Setup(x => x.PopAsync())
                  .ReturnsAsync<INavigation, Page>(new Page());
            mocker.DeviceMock.Setup(x => x.BeginInvokeOnMainThread(It.IsAny<Action>()))
                  .Callback<Action>((action) => action());

            // Act
            mocker.FormPageViewModel.ClosePageCommand.Execute(null);

            // Verify
            mocker.NavigationMock.VerifyAll();
        }

        [Test]
        public async Task WillNotSaveFormWhenNotValid()
        {
			// setup
			var mocker = new HomeViewModelMock();

			mocker.SetupSavedForm();
            mocker.MessageServiceMock.Setup(x => x.Send<string>(It.Is<string>(key => key == "FailedFormValidation"), It.IsAny<object>())).Verifiable();
            await mocker.FormPageViewModel.Initialise();

			// Act
			mocker.FormPageViewModel.SaveFormCommand.Execute(null);

            // Verify
            mocker.FileReaderWriterMock.Verify(x => x.SaveForm(It.IsAny<List<BaseControl>>(), It.IsAny<string>()), Times.Never());
            mocker.MessageServiceMock.VerifyAll();
        }

        [Test]
        public async Task WillSaveValidForm()
        {
			// setup
			var mocker = new HomeViewModelMock();

			mocker.SetupEasyToValidateForm();
            mocker.FileReaderWriterMock.Setup(x => x.SaveForm(It.IsAny<List<BaseControl>>(), It.IsAny<string>())).Verifiable();
			mocker.MessageServiceMock.Setup(x => x.Send<string>(It.Is<string>(key => key == "SavedSuccessfull"), It.IsAny<object>())).Verifiable();
            await mocker.FormPageViewModel.Initialise();

			// Act
			mocker.FormPageViewModel.SaveFormCommand.Execute(null);

            // Verify
            mocker.FileReaderWriterMock.VerifyAll();
            mocker.MessageServiceMock.VerifyAll();
        }

		class HomeViewModelMock
		{
			public Mock<INavigation> NavigationMock { get; set; } = new Mock<INavigation>();
			public Mock<IFileReaderWriter> FileReaderWriterMock { get; set; } = new Mock<IFileReaderWriter>();
			public Mock<IMessageService> MessageServiceMock { get; set; } = new Mock<IMessageService>();
            public Mock<IDevice> DeviceMock { get; set; } = new Mock<IDevice>();

			public FormPageViewModel FormPageViewModel { get; private set; }

            public void SetupEmptyForm()
            {
				FileReaderWriterMock.Setup(x => x.LoadForm(It.IsAny<string>()))
				 .ReturnsAsync(new List<DynamicForms.UIControls.BaseControl>()).Verifiable();
                
                FormPageViewModel = new FormPageViewModel(GenerateForm(),
                                                          NavigationMock.Object,
                                                          MessageServiceMock.Object,
                                                          FileReaderWriterMock.Object,
                                                          DeviceMock.Object);
			}

            public void SetupSavedForm()
            {
				FileReaderWriterMock.Setup(x => x.LoadForm(It.IsAny<string>()))
				 .ReturnsAsync(GenerateField(13)).Verifiable();

				FormPageViewModel = new FormPageViewModel(GenerateForm(),
														  NavigationMock.Object,
														  MessageServiceMock.Object,
														  FileReaderWriterMock.Object,
                                                          DeviceMock.Object);

			}

            public void SetupEasyToValidateForm()
            {
                var formDoc = new Form( XDocumentFactory.XmlEasyValidationForm().Element("forms").Element("form"));

				FileReaderWriterMock.Setup(x => x.LoadForm(It.IsAny<string>()))
				 .ReturnsAsync(GenerateField(1)).Verifiable();

				FormPageViewModel = new FormPageViewModel(formDoc,
														  NavigationMock.Object,
														  MessageServiceMock.Object,
														  FileReaderWriterMock.Object,
														  DeviceMock.Object);
            }


            private List<BaseControl> GenerateField(int count)
            {
                var controls = new List<BaseControl>();
                for (int i = 0; i < 0; i++)
                {
					controls.Add(new TextControl()
					{
						Type = "text",
						Value = "test " + i
					});
                }

                return controls;
            }
            private Form GenerateForm()
            {
                var formDoc = XDocumentFactory.XmlWithOneForm().Element("forms").Element("form");
                return new Form(formDoc);
            }
		}
    }
}
