using System.ComponentModel;
using System.Windows.Input;

namespace ShutdownPC.Helpers
{
    public class RelayCommand : ICommand
    {
        #region Fields

        private readonly Action<object> _execute;
        private readonly Func<bool> _canExecute;

        #endregion Fields

        #region Constructors

        public RelayCommand(Action<object> execute)
        : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Func<bool> canExecute = null, INotifyPropertyChanged propChangedSource = null)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion Constructors

        #region ICommand Members

        private event EventHandler _canExecuteChanged;

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        private void PropChangedSource_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }

        protected virtual void OnCanExecuteChanged()
        {
            _canExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

        #endregion ICommand Members
    }
}