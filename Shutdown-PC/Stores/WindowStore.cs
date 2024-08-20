using CommunityToolkit.Mvvm.ComponentModel;
using ShutdownPC.ViewModels.Windows;
using ShutdownPC.Windows;
using System.Windows;
using System.Windows.Media.Effects;

namespace ShutdownPC.Stores
{
	/// <summary>
	/// Store pro spouštění a nastavování oken aplikace.
	/// </summary>
	public partial class WindowStore : ObservableObject
	{
		private readonly ClasesStore _clasesStore;
		private readonly MainWindow _mainWindow;

		[ObservableProperty]
		private TimeSpan _setTimeValueProperty;

		public WindowStore(ClasesStore clasesStore, MainWindow mainWindow)
		{
			_clasesStore = clasesStore;
			_mainWindow = mainWindow;
		}
		/// <summary>
		/// Zobrazení okna s nastavením aplikace.
		/// </summary>
		/// <returns>Funkce napíše, zda se podařilo okno otevřít či nikoliv.</returns>
		public bool? ShowSettigWindow()// TODO ještě není doděláno vnitřek
		{
			try
			{
				var viewModel = _clasesStore.GetSettingWindowViewModel();
				var window = new SettingWindow();

				return showDialog<SettingWindowViewModel, SettingWindow>(viewModel, window, _mainWindow);
			}
			catch (Exception)
			{
				return false;
			}
		}
		/// <summary>
		/// Zobrazení okna s nastavením aplikace.
		/// </summary>
		/// <returns>Funkce napíše, zda se podařilo okno otevřít či nikoliv.</returns>
		public bool? ShowInfoWindow()
		{
			try
			{
				var viewModel = _clasesStore.GetInfoWindowViewModel();
				var window = new InfoWindow();
				return showDialog<InfoWindowViewModel, InfoWindow>(viewModel, window, _mainWindow);
			}
			catch (Exception)
			{
				return false;
			}
		}
		/// <summary>
		/// Zobrazení okna s informací, že se blíží konec času.
		/// Uživatel může rozhodnout, zda chce prodloužit čas či nikoliv.
		/// Okno se zobrazí po nastavení aplikace.
		/// </summary>
		/// <returns>Funkce napíše, zda se podařilo okno otevřít či nikoliv.</returns>
		public bool? ShowCountdownPopupWindow() // TODO ještě není doděláno vnitřek
		{
			try
			{
				var viewModel = _clasesStore.GetCountdownPopupViewModel();
				var window = new CountdownPopupWindow();
				return showDialog<CountdownPopupViewModel, CountdownPopupWindow>(viewModel, window, _mainWindow, WindowStartupLocation.Manual);
			}
			catch (Exception)
			{
				return false;
			}
		}
		/// <summary>
		/// Zavře okno.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="viewModel"> Je potřeba zadat ViewModel pro okno co se má zavřít</param>
		/// <returns>Funkce napíše, zda se podařilo okno zavřít či nikoliv.</returns>
		public bool CloseWindow<T>(T viewModel)
			 where T : BaseWindowViewModel
		{
			try
			{
				closeWindow(viewModel);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		private bool? showDialog<T, R>(T viewModel, R window, Window owner, WindowStartupLocation windowStartupLocation = WindowStartupLocation.CenterOwner)
			where T : BaseWindowViewModel
			where R : Window
		{
			BlurEffect objBlur = new BlurEffect();
			objBlur.Radius = 4;

			viewModel.Window = window;
			viewModel.Window.DataContext = viewModel;
			viewModel.Window.Owner = owner;
			viewModel.Window.Owner.Effect = objBlur;
			viewModel.Window.WindowStartupLocation = windowStartupLocation;

			return viewModel.Window.ShowDialog();
		}

		private void closeWindow<T>(T viewModel)
			  where T : BaseWindowViewModel
		{
			if (viewModel.Window is Window)
			{
				BlurEffect objBlur = new BlurEffect();
				objBlur.Radius = 0;
				viewModel.Window.Owner.Effect = objBlur;
			}

			viewModel.Window?.Close();
		}
	}
}