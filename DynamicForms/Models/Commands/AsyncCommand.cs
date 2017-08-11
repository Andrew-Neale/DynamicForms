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
    public class AsyncCommand : ICommand
    {
        private event EventHandler m_canExecuteChangedEvent;
        private readonly object m_lock = new object();
        private readonly Func<bool> m_canExecute;
        private readonly Func<object, Task> m_task;

        private bool m_previuosCanExecuteValue;

        public event EventHandler CanExecuteChanged
        {
            add
            {
                lock (m_lock)
                {
                    m_canExecuteChangedEvent += value;
                }
            }
            remove
            {
                lock (m_lock)
                {
                    m_canExecuteChangedEvent -= value;
                }
            }
        }

        public AsyncCommand(Func<object, Task> task, Func<bool> canExecute = null)
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
            if (m_canExecute != null)
            {
                var canExecute = m_canExecute();

                if (canExecute != m_previuosCanExecuteValue)
                {
                    m_previuosCanExecuteValue = canExecute;
                    RaiseCanExecuteChangedEvent();
                }

                return canExecute;
            }

            return true;
        }

        public bool CanExecute(object parameter)
        {
            return CanExecute();
        }

        public void Execute(object parameter)
        {
            try
            {
                Task.Run(() => m_task(parameter));
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }

        public Task ExecuteAsync(object parameter = null)
        {
            return m_task(parameter);
        }

        private void Log(Exception ex, [CallerMemberName] string caller = null)
        {
           System.Diagnostics.Debug.WriteLine(caller, ex);
        }

        private void RaiseCanExecuteChangedEvent()
        {
            var changedEvent = m_canExecuteChangedEvent;
            if (changedEvent != null)
            {
                changedEvent(this, new EventArgs());
            }
        }
    }
}
