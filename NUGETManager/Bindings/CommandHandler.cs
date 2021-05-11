using System;
using System.Windows.Input;

namespace NUGETManager.Bindings
{
    public class CommandHandler : ICommand
    {
        private static readonly Func<bool> AlwaysTrue = () => true; 

        private Action _action;
        private Func<bool> _canExecute;

        public CommandHandler(Action action) : this(action, AlwaysTrue){}

        public CommandHandler(Action action, Func<bool> canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute.Invoke();
        }

        public void Execute(object parameter)
        {
            _action();
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public static implicit operator CommandHandler(Action a) => new CommandHandler(a);
    }
}