using System.Threading.Tasks;
using System.Windows.Input;
using DynamicForms.Models.Device;
using DynamicForms.Models.Io;
using DynamicForms.Views;
using Xamarin.Forms;
using DF = DynamicForms.Models.Xml;

namespace DynamicForms.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
		private readonly INavigation _navigation;
		private readonly IFileReaderWriter _fileReaderWriter;
        private readonly IDevice _device;
        private readonly string _resourcePrefix;

		private ICommand _selectPageCommand;

        public ObservableRangeCollection<DF.Form> FormPages { get; private set; } = new ObservableRangeCollection<DF.Form>();
		public ICommand SelectPageCommand => _selectPageCommand ?? (_selectPageCommand = new Command<DF.Form>(o => DoSelectPageCommand(o)));

        public HomeViewModel(INavigation navigation, IFileReaderWriter fileReaderWriter, IDevice device, string resourcePrefix)
        {
            _navigation = navigation;
            _fileReaderWriter = fileReaderWriter;
            _device = device;
            _resourcePrefix = resourcePrefix;

            Title = "Home Page";
        }

        public async Task Initialise()
        {
            IsBusy = true;
            FormPages.AddRange(await _fileReaderWriter.LoadResource<HomeViewModel>(_resourcePrefix));
            IsBusy = false;
        }

        private void DoSelectPageCommand(DF.Form selectedForm)
        {
            _device.BeginInvokeOnMainThread(async () => await _navigation.PushAsync(new FormPage(selectedForm)));
        }
    }
}
