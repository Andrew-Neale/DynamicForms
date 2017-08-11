using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using DynamicForms.Models.Commands;
using DF = DynamicForms.Models.Xml;
using Xamarin.Forms;
using DynamicForms.Views;
using DynamicForms.Models.Io;
using DynamicForms.Models.Device;

namespace DynamicForms.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
		private readonly INavigation _navigation;
		private readonly IFileReaderWriter _fileReaderWriter;
        private readonly IDevice _device;

		private ICommand _selectPageCommand;

        public ObservableRangeCollection<DF.Form> FormPages { get; private set; } = new ObservableRangeCollection<DF.Form>();
		public ICommand SelectPageCommand => _selectPageCommand ?? (_selectPageCommand = new Command<DF.Form>(o => DoSelectPageCommand(o)));

        public HomeViewModel(INavigation navigation, IFileReaderWriter fileReaderWriter, IDevice device, string resourcePrefix)
        {
            _navigation = navigation;
            _fileReaderWriter = fileReaderWriter;
            _device = device;

            Title = "Home Page";
            LoadResourceFile(resourcePrefix);
        }

        public void LoadResourceFile(string resourcePrefix)
        {
            FormPages.AddRange(_fileReaderWriter.LoadResource<HomeViewModel>(resourcePrefix));
        }

        private void DoSelectPageCommand(DF.Form selectedForm)
        {
            _device.BeginInvokeOnMainThread(async () => await _navigation.PushAsync(new FormPage(selectedForm)));
        }
    }
}
