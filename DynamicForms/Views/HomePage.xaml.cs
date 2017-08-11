using Xamarin.Forms;
using DynamicForms.ViewModels;
using DynamicForms.Models.Io;

namespace DynamicForms.Views
{
    public partial class HomePage : ContentPage
    {
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

			BindingContext = new HomeViewModel(Navigation, new FileReaderWriter(), new DynamicForms.Models.Device.Device(), resourcePrefix);
        }
    }
}
