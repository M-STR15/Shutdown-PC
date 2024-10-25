using CommunityToolkit.Mvvm.ComponentModel;
using Prism.Events;
using ShutdownPC.Helpers;
using ShutdownPC.Models;
using ShutdownPC.Models.Enums;
using ShutdownPC.Services;
using ShutdownPC.Stores;
using ShutdownPC.ViewModels.Windows;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ShutdownPC
{
	public partial class MainViewModel : BaseWindowViewModel
	{
		public EventHandler SetTimeValueChange;
		public EventHandler StatusChange;
		private readonly WindowStore _windowStore;

		[ObservableProperty]
		private DateTime _clockTime;

		[ObservableProperty]
		private int _endAfterSeconds;

		[ObservableProperty]
		private IEventAggregator _eventRestartView;

		private IEventLogService _log;

		[ObservableProperty]
		private string _message;

		private DateTime _setTimeValue;

		private eStatus _status;

		[ObservableProperty]
		private string _title;

		[ObservableProperty]
		private eTypeAction _typeAction;

		[ObservableProperty]
		private eTypeModification _typeModification;

		[ObservableProperty]
		private string _version;

		private DispatcherTimer t_CountdownTimer;
		public MainViewModel(WindowStore windowsStore, IEventAggregator eventRestartView, IEventLogService log) : base(windowsStore)
		{
			EventRestartView = eventRestartView;
			_log = log;

			t_CountdownTimer = new DispatcherTimer();
			t_CountdownTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
			t_CountdownTimer.Tick += new EventHandler(onCountdown_Tick);

			TypeModification = eTypeModification.AfterTime;
			ShutdownCommand = new Helpers.RelayCommand(cmd_Shutdown);
			RestartCommand = new Helpers.RelayCommand(cmd_Restart);
			LogTheUserOutCommnad = new Helpers.RelayCommand(cmd_LogTheUserOut);
			SleepModeCommand = new Helpers.RelayCommand(cmd_SleepMode);
			ChangeStatusCommnad = new Helpers.RelayCommand(cmd_ChangeStatus);
			ShowSettingCommand = new Helpers.RelayCommand(cmd_ShowSetting);
			ShowInfoCommand = new Helpers.RelayCommand(cmd_ShowInfo);

			_windowStore = windowsStore;

			Status = eStatus.Stop;

			Version = BuildInfo.VersionStr;

			Title = "Shutdown-PC";

			_log.Information(new Guid("ce1a0a77-8074-4e33-b761-4dd8411eb252"), "Start");

			SetTimeValue = DateTime.Now;
		}

		public ICommand ChangeStatusCommnad { get; private set; }
		public ICommand LogTheUserOutCommnad { get; set; }
		public ICommand RestartCommand { get; private set; }

		public DateTime SetTimeValue
		{
			get => _setTimeValue;
			set
			{
				var currentDatetime = DateTime.Now;
				if (_setTimeValue != value)
				{
					_setTimeValue = value;
					onSetTimeValueChange();
					OnPropertyChanged();
				}
				else if (_setTimeValue < currentDatetime)
				{
					_setTimeValue = currentDatetime;
					onSetTimeValueChange();
					OnPropertyChanged();
				}
			}
		}

		public ICommand ShowInfoCommand { get; private set; }

		public ICommand ShowSettingCommand { get; private set; }

		public ICommand ShutdownCommand { get; private set; }

		public ICommand SleepModeCommand { get; private set; }

		public eStatus Status
		{
			get => _status;
			set
			{
				if (_status != value)
				{
					_status = value;
					OnPropertyChanged();
					setTimer();
				}
			}
		}

		private void cmd_ChangeStatus(object parameter) => changeStatus();

		private void cmd_LogTheUserOut(object parameter) => logOff();

		private void cmd_Restart(object parameter) => restart();

		private void cmd_ShowInfo(object parameter) => showInfo();

		private void cmd_ShowSetting(object parameter) => showSetting();

		private void cmd_Shutdown(object parameter) => shutdown();

		private void cmd_SleepMode(object parameter) => sleepMode();

		private async Task delayStartTimerAsync(TimeSpan delay)
		{
			await Task.Delay(delay);
			t_CountdownTimer.Start();
		}

		private void changeStatus()
		{
			switch (Status)
			{
				case eStatus.Run:
					Status = eStatus.Stop;
					break;

				default:
					Status = eStatus.Run;
					//_windowStore.ShowCountdownPopupWindow();
					break;
			}
		}

		private void logOff()
		{
			try
			{
				PrivilegeManager.EnableShutdownPrivilege();
				PcActionService.LogOff();
			}
			catch (Exception)
			{
				_log.Error(Guid.Parse("35c8a818-4254-4147-aa5d-24ca720284d1"), "Chyba při odhlášení uživatele.");
			}
		}
		private void onCountdown_Tick(object sender, EventArgs args)
		{
			try
			{
				var curentDatetime = DateTime.Now;

				if (SetTimeValue <= curentDatetime)
				{
					t_CountdownTimer.Stop();
					Status = eStatus.Completed;
					startMethodAfterTheTimerExpires();
				}
				else
				{
					EndAfterSeconds = (int)(SetTimeValue - curentDatetime).TotalSeconds;
				}

				EventRestartView.GetEvent<TickEvent>().Publish();
			}
			catch (Exception)
			{
				_log.Error(Guid.Parse("793f468a-de37-40c9-aaeb-f17ea4dd365b"), "Chyba při odpočtu času.");
			}
		}

		private void onSetTimeValueChange() => SetTimeValueChange?.Invoke(this, new EventArgs());

		private void restart()
		{
			try
			{
				PrivilegeManager.EnableShutdownPrivilege();
				PcActionService.Restart();
			}
			catch (Exception)
			{
				_log.Error(Guid.Parse("788cc881-3cdc-492f-9f27-a05d3591b18f"), "Chyba při pokusu o restart PC.");
			}
		}

		private async void setTimer()
		{
			try
			{
				if (Status == eStatus.Run)
				{
					var delay = TimeSpan.FromMicroseconds(1000 - ClockTime.Millisecond);
					await delayStartTimerAsync(delay);
				}
				else
				{
					t_CountdownTimer.Stop();
				}
			}
			catch (Exception)
			{
				_log.Error(Guid.Parse("1b3c07b7-63b4-484b-95d6-cc63684015d0"), "");
			}
		}

		private void showCountdownPopup()
		{
			try
			{
				var isOpen = _windowStore.ShowCountdownPopupWindow();
				if (isOpen != null && !(bool)isOpen)
					_log.Warning(Guid.Parse("1530568d-5d2b-4e55-b4c4-a34959dfea62"), "Nepodařilo se zobrazit okno s odpočtem času před dokončení cyklu.");
			}
			catch (Exception)
			{
				_log.Error(Guid.Parse("fde3b586-93d2-4071-a402-571dec09d194"), "Nepodařilo se zobrazit okno s odpočtem času před dokončení cyklu.");
			}
		}

		private void showInfo()
		{
			try
			{
				var isOpen = _windowStore.ShowInfoWindow();
				if (isOpen != null && !(bool)isOpen)
					_log.Warning(Guid.Parse("7a680274-f07c-4034-9b65-2d1790728ec2"), "Nepodařilo se zobrazit okno s informaci.");
			}
			catch (Exception)
			{
				_log.Error(Guid.Parse("d5d93a0a-d436-44e4-8be8-f5625053310e"), "Nepodařilo se zobrazit okno s informaci.");
			}
		}

		private void showSetting()
		{
			try
			{
				var isOpen = _windowStore.ShowSettigWindow();
				if (isOpen != null && !(bool)isOpen)
					_log.Warning(Guid.Parse("3eceb500-2818-42b3-b6fb-9591e69873c7"), "Nepodařilo se zobrazit okno s nastavením aplikace.");
			}
			catch (Exception)
			{
				_log.Error(Guid.Parse("5ce86b03-d99f-44e1-8307-37310ec5d0b5"), "Nepodařilo se zobrazit okno s nastavením aplikace.");
			}
		}

		private void shutdown()
		{
			try
			{
				PrivilegeManager.EnableShutdownPrivilege();
				PcActionService.Shutdown();
			}
			catch (Exception)
			{
				_log.Error(Guid.Parse("097abbf9-cd1e-4341-8a25-754aa966c919"), "Chyba při vypnutí aplikace.");
			}
		}

		private void sleepMode()
		{
			//pcAction.SleepMode();
		}

		private void startMethodAfterTheTimerExpires()
		{
			try
			{
				switch (Status)
				{
					case eStatus.Completed:
						Status = eStatus.Stop;

						switch (TypeAction)
						{
							case eTypeAction.Shutdown:
								PrivilegeManager.EnableShutdownPrivilege();
								PcActionService.Shutdown();
								break;

							case eTypeAction.Restart:
								PrivilegeManager.EnableShutdownPrivilege();
								PcActionService.Restart();
								break;

							case eTypeAction.LogTheUserOut:
								PrivilegeManager.EnableShutdownPrivilege();
								PcActionService.LogOff();
								break;

							case eTypeAction.SleepMode:

								break;
						}

						break;
				}
			}
			catch (Exception)
			{
				_log.Error(Guid.Parse("0e07a829-b019-4890-b02b-2456d419c114"), "Chyba při spuštění methody po odpočtu času.");
			}
		}
	}
}