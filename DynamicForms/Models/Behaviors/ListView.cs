using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace DynamicForms.Models.Behaviors
{
    public static class ListView
    {
        public static readonly BindableProperty CommandProperty =
            BindableProperty.CreateAttached(
                "Command",
                typeof(ICommand),
                typeof(ListView),
                null,
                propertyChanged: OnCommandChanged);

        static void OnCommandChanged(BindableObject view, object oldValue, object newValue)
        {
            var entry = view as Xamarin.Forms.ListView;
            if (entry == null)
                return;

            entry.ItemTapped += (sender, e) =>
            {
                var command = (newValue as ICommand);
                if (command == null)
                    return;

                if (command.CanExecute(e.Item))
                {
                    command.Execute(e.Item);
                }
            };
        }
    }
}
