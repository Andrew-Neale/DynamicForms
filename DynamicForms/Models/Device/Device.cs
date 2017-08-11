using System;
namespace DynamicForms.Models.Device
{
    public class Device : IDevice
    {
        public void BeginInvokeOnMainThread(Action action)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(action);
        }
    }
}

