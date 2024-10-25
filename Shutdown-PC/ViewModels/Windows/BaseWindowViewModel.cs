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

		public BaseWindowViewModel(WindowStore windowStore) : base()
		{

			_windowStore = windowStore;
			CloseWindowCommand = new Helpers.RelayCommand(cmd_CloseWindow);
			CloseCommand = new Helpers.RelayCommand(cmd_Close);
			MinimalizationCommand = new Helpers.RelayCommand(cmd_minimalize);
		}

		public ICommand CloseCommand { get; private set; }
		public ICommand CloseWindowCommand { get; private set; }
		public ICommand MinimalizationCommand { get; private set; }
		public Window Window { get; set; }

		protected virtual void cmd_Close(object parameter) => close();

		protected virtual void cmd_minimalize(object parameter) => minimalize();

		protected virtual void minimalize() => App.Current.MainWindow.WindowState = WindowState.Minimized;

		private void close()
		{
			App.Current.Shutdown();
		}

		private void cmd_CloseWindow(object parameter)
		{
			_windowStore.CloseWindow<BaseWindowViewModel>(this);
		}
	}
}