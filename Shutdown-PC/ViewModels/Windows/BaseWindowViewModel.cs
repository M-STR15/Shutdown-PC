using CommunityToolkit.Mvvm.ComponentModel;
using ShutdownPC.Stores;
using System.Windows;
using System.Windows.Input;

namespace ShutdownPC.ViewModels.Windows
{
	/// <summary>
	/// Základní třída pro ViewModely vyskakovacích oken.
	/// </summary>
	[ObservableObject]
	public abstract partial class BaseWindowViewModel : BaseViewModel
	{
		protected WindowStore _windowStore;

		[ObservableProperty]
		private string _title;

		public BaseWindowViewModel(WindowStore windowStore)
		{
			_windowStore = windowStore;
			CloseWindowCommand = new Helpers.RelayCommand(cmd_CloseWindow);
		}

		public ICommand CloseWindowCommand { get; private set; }
		public Window Window { get; set; }

		private void cmd_CloseWindow(object parameter)
		{
			_windowStore.CloseWindow<BaseWindowViewModel>(this);
		}
	}
}