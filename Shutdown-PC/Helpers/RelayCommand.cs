using System.ComponentModel;
using System.Windows.Input;

namespace ShutdownPC.Helpers
{
	/// <summary>
	/// Třída sloužící k nastavení command. Ovladačů ve Views.
	/// </summary>
	public class RelayCommand : ICommand
	{
		#region Fields

		private readonly Func<bool> _canExecute;
		private readonly Action<object> _execute;

		#endregion Fields

		#region Constructors

		public RelayCommand(Action<object> execute)
		: this(execute, null)
		{
		}

		public RelayCommand(Action<object> execute, Func<bool>? canExecute = null, INotifyPropertyChanged? propChangedSource = null)
		{
			if (execute == null)
				throw new ArgumentNullException("execute");

			_execute = execute;
			_canExecute = canExecute;
		}

		#endregion Constructors

		#region ICommand Members

		public event EventHandler CanExecuteChanged
		{
			add => CommandManager.RequerySuggested += value;
			remove => CommandManager.RequerySuggested -= value;
		}

		private event EventHandler _canExecuteChanged;

		/// <summary>
		/// Metoda ověřuje, zda může být příkaz vykonán.
		/// </summary>
		public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

		/// <summary>
		/// Metoda vykoná příkaz s daným parametrem.
		/// </summary>
		public void Execute(object parameter) => _execute(parameter);

		/// <summary>
		/// Metoda vyvolá událost CanExecuteChanged.
		/// </summary>
		protected virtual void OnCanExecuteChanged()
		{
			_canExecuteChanged?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		/// Metoda, která se spustí při změně vlastnosti zdroje.
		/// </summary>
		private void PropChangedSource_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			OnCanExecuteChanged();
		}

		#endregion ICommand Members
	}
}