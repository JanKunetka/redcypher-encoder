using System.Windows.Input;

namespace RedCipher.VMs.Core
{
    /// <summary>
    /// A reusable command.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object> OnExecute;
        private readonly Predicate<object>? WhenCanExecute;
        
        public RelayCommand(Action<object> onExecute, Predicate<object>? whenCanExecute = null)
        {
            if (onExecute == null) throw new NullReferenceException("Execute command cannot be null");
            
            OnExecute = onExecute;
            WhenCanExecute = whenCanExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
        
        public bool CanExecute(object? parameter)
        {
            return (WhenCanExecute == null) ? true : WhenCanExecute.Invoke(parameter);
        }

        public void Execute(object? parameter)
        {
            OnExecute?.Invoke(parameter);
        }

    }
}