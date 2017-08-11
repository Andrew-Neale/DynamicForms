using System.Collections.Generic;
using DynamicForms.Views;
using Xamarin.Forms;

namespace DynamicForms
{
    public partial class App : Application
    {
        public static bool UseMockDataStore = true;
        public static string BackendUrl = "https://localhost:5000";

        public static IDictionary<string, string> LoginParameters => null;

        public App ()
        {
            InitializeComponent ();

            SetMainPage ();
        }

        public static void SetMainPage ()
        {

            GoToMainPage();
        }

        public static void GoToMainPage ()
        {
            Current.MainPage = new TabbedPage {
                Children = {
                    new NavigationPage(new HomePage())
                    {
                        Title = "Forms",
                        Icon = Device.OnPlatform("tab_feed.png", null, null)
                    }
                }
            };
        }
    }
}
