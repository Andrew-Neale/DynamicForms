using System;
using Xamarin.Forms;

namespace DynamicForms.Models.Messaging
{
    public class MessageService : IMessageService
    {
        public void Send<T>(T message, object sender = null) where T : class
        {
            MessagingCenter.Send(sender ?? new object(), typeof(T).FullName, message);
        }

        public void Subscribe<T>(object subscriber, Action<object, T> callback) where T : class
        {
            MessagingCenter.Subscribe(subscriber, typeof(T).FullName, callback, null);
        }

        public void Unsubscribe<T>(object subscriber) where T : class
        {
            MessagingCenter.Unsubscribe<object, T>(subscriber, typeof(T).FullName);
        }
    }
}
