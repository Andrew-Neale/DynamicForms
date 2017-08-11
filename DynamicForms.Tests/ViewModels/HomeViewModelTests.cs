using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using DynamicForms.Models.Device;
using DynamicForms.Models.Io;
using DynamicForms.Models.Xml;
using DynamicForms.Tests.Helpers;
using DynamicForms.ViewModels;
using DynamicForms.Views;
using Moq;
using NUnit.Framework;
using Xamarin.Forms;

namespace DynamicForms.Tests.ViewModels
{
    [TestFixture]
    public class HomeViewModelTests
    {
        [Test]
        public void CanCreateHomeViewModel()
        {
            // Setup
            var homeViewModelMock = new HomeViewModelMock();

			// Act
			homeViewModelMock.SetupDefault();

			// Verify
			Assert.AreEqual("Home Page", homeViewModelMock.HomeViewModel.Title);
        }

        class HomeViewModelMock
        {
            public Mock<INavigation> NavigationMock { get; set; } = new Mock<INavigation>();
            public Mock<IFileReaderWriter> FileReaderWriterMock { get; set; } = new Mock<IFileReaderWriter>();
            public Mock<IDevice> DeviceMock { get; set; } = new Mock<IDevice>();

            public HomeViewModel HomeViewModel { get; private set; }

            public void SetupDefault()
            {
				var resourcePrefix = "Assets.FormsData.xml";
				FileReaderWriterMock.Setup(x => x.LoadResource<HomeViewModel>(It.IsAny<string>()))
									.Returns(new List<DynamicForms.Models.Xml.Form>()
				{
				    new Form(null)
				});

                HomeViewModel = new HomeViewModel(NavigationMock.Object, FileReaderWriterMock.Object, DeviceMock.Object, resourcePrefix);
            }
        }
    }
}
