using System;
using Xamarin.Forms;
using DynamicForms.ViewModels;
using DynamicForms.Models.Io;
using Xamarin.Forms.Xaml;

namespace DynamicForms.Views
{
    [XamlCompilation (XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private HomeViewModel ViewModel { get; set; }
        private bool _isLoaded;

        public HomePage()
        {
            InitializeComponent();
            string resourcePrefix = string.Empty;

#if __IOS__
            resourcePrefix = "DynamicForms.iOS.";
#endif
#if __ANDROID__
		    resourcePrefix = "DynamicForms.Droid.";
#endif
			resourcePrefix += "Assets.FormsData.xml";

            ViewModel = new HomeViewModel(Navigation, new FileReaderWriter(), new DynamicForms.Models.Device.Device(), resourcePrefix);
            BindingContext = ViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //Due to onAppearing twice bug https://bugzilla.xamarin.com/show_bug.cgi?id=51574
            //We check if we have already loaded
            if (!_isLoaded)
            {
                _isLoaded = true;
                await ViewModel.Initialise();
            }
        }
    }
}
