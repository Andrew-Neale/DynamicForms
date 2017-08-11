using System;
namespace DynamicForms.Models.Device
{
    public interface IDevice
    {
		/// <summary>
		/// Executes action on UI thread.
		/// </summary>
		/// <param name="action">The action to perform on the UI thread</param>
        void BeginInvokeOnMainThread(Action action);
    }
}

