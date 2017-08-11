using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using System.Xml.Serialization;
using DynamicForms.ViewModels;
using Xamarin.Forms;

namespace DynamicForms.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel ()
        {
            Title = "About";

            OpenWebCommand = new Command (() => Device.OpenUri (new Uri ("https://xamarin.com/platform")));

        }


        /// <summary>
        /// Command to open browser to xamarin.com
        /// </summary>
        public ICommand OpenWebCommand { get; }
    }
}
