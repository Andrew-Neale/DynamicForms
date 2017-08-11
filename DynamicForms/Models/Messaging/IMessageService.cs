using System;
namespace DynamicForms.Models.Messaging
{
    public interface IMessageService
    {
		void Send<TMessage>(TMessage message, object sender = null) where TMessage : class;

		void Subscribe<TMessage>(object subscriber, Action<object, TMessage> callback) where TMessage : class;

		void Unsubscribe<TMessage>(object subscriber) where TMessage : class;
    }
}
