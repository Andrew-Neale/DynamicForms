using System;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace DynamicForms.Models.Commands
{
    /// <summary>
    /// Asynchronous implementation of ICommand, which allows tests to await on the execution.
    /// Regular usage in the UI via bindings *does not* wait on the execution and is run off the UI thread.
    /// </summary>
    public class AsyncCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add {}
            remove {}
        }

        private readonly Func<bool> m_canExecute;
        private readonly Func<T, Task> m_task;

        public AsyncCommand(Func<T, Task> task, Func<bool> canExecute = null)
        {
            if (task == null)
            {
                throw new ArgumentNullException("task");
            }

            m_task = task;
            m_canExecute = canExecute;
        }

        public bool CanExecute()
        {
            return m_canExecute == null || m_canExecute();
        }

        public bool CanExecute(object parameter)
        {
            return CanExecute();
        }

        public void Execute(object parameter)
        {
            try
            {
                Task.Run(() => m_task((T)parameter));
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }

        public Task ExecuteAsync(T parameter = default(T))
        {
            return m_task(parameter);
        }

        private void Log(Exception ex, [CallerMemberName] string caller = null)
        {
            System.Diagnostics.Debug.WriteLine(caller, ex);
        }
    }
}
