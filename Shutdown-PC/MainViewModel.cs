using CommunityToolkit.Mvvm.ComponentModel;
using ShutdownPC.Models.Enums;
using ShutdownPC.Services;
using ShutdownPC.Stores;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ShutdownPC
{
    [ObservableObject]
    public partial class MainViewModel
    {
        public EventHandler SetTimeValueChange;
        public EventHandler StatusChange;
        private readonly WindowStore _windowStore;

        [ObservableProperty]
        private int _endAfterSeconds;

        [ObservableProperty]
        private string _message;

        private DateTime _setTimeValue;

        private eStatus _status;

        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private eTypeAction _typeAction;

        //private void onStatusChange(EventArgs args = null)
        //{
        //    StatusChange?.Invoke(this, args);
        //}
        [ObservableProperty]
        private eTypeModification _typeModification;

        [ObservableProperty]
        private string _version;

        private PcActionService pcAction;

        private DispatcherTimer t_CountdownTimer;

        public MainViewModel(WindowStore windowsStore)
        {
            t_CountdownTimer = new DispatcherTimer();
            t_CountdownTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            t_CountdownTimer.Tick += new EventHandler(onCountdown_Tick);

            TypeModification = eTypeModification.AfterTime;
            ShutdownCommand = new Helpers.RelayCommand(cmd_Shutdown);
            RestartCommand = new Helpers.RelayCommand(cmd_Restart);
            LogTheUserOutCommnad = new Helpers.RelayCommand(cmd_LogTheUserOut);
            SleepModeCommand = new Helpers.RelayCommand(cmd_SleepMode);
            ChangeStatusCommnad = new Helpers.RelayCommand(cmd_ChangeStatus);
            ShowSettingCommand = new Helpers.RelayCommand(cmd_ShowSetting);
            ShowInfoCommand = new Helpers.RelayCommand(cmd_ShowInfo);
            CloseCommand = new Helpers.RelayCommand(cmd_close);

            _windowStore = windowsStore;

            pcAction = new PcActionService();

            Status = eStatus.Stop;

            SetTimeValue = DateTime.Now;
            Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            Title = "Shutdown-PC";
            //StatusChange += onStatusChange;

        }

        public ICommand CloseCommand { get; private set; }

        public ICommand ChangeStatusCommnad { get; private set; }

        //private void onStatusChange(object sender, EventArgs args)
        //{
        //    changeStatus();
        //}
        public ICommand LogTheUserOutCommnad { get; set; }

        public ICommand RestartCommand { get; private set; }

        public DateTime SetTimeValue
        {
            get => _setTimeValue;
            set
            {
                if (_setTimeValue != value)
                {
                    _setTimeValue = value;
                }

                onSetTimeValueChange();
                OnPropertyChanged();
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
                    //onStatusChange();
                    OnPropertyChanged();
                    startMethodAfterTheTimerExpires();

                    if (Status == eStatus.Run)
                    {
                        t_CountdownTimer.Start();
                    }
                    else
                        t_CountdownTimer.Stop();
                }
            }
        }

        private void close()
        {
            App.Current.Shutdown();
        }

        private void cmd_close(object parameter) => close();

        private void cmd_ChangeStatus(object parameter) => changeStatus();

        private void cmd_LogTheUserOut(object parameter) => logOff();

        private void cmd_Restart(object parameter) => reboot();

        private void cmd_ShowInfo(object parameter) => showInfo();

        private void cmd_ShowSetting(object parameter) => showSetting();

        private void cmd_Shutdown(object parameter) => shutdown();

        private void cmd_SleepMode(object parameter) => sleepMode();

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
            pcAction.LogOff();
        }

        private void onCountdown_Tick(object sender, EventArgs args)
        {
            if (SetTimeValue <= DateTime.Now)
            {
                t_CountdownTimer.Stop();
                Status = eStatus.Completed;

                MessageBox.Show("test timeru");
            }
            else
            {
                _endAfterSeconds = (int)(SetTimeValue - DateTime.Now).TotalSeconds;
                //setLabelTimer();
            }

            resetTimeInControl();
        }

        private void onSetTimeValueChange() => SetTimeValueChange?.Invoke(this, new EventArgs());
        private void reboot()
        {
            pcAction.Reboot();
        }

        private void resetTimeInControl()
        {
            var backupTimeSetting= SetTimeValue;
            SetTimeValue = DateTime.MinValue;
            SetTimeValue = backupTimeSetting;
        }
        private void showCountdownPopup()
        {
            _windowStore.ShowCountdownPopupWindow();
        }

        private void showInfo()
        {
            _windowStore.ShowInfoWindow();
        }
        private void showSetting()
        {
            _windowStore.ShowSettigWindow();
            //pcAction.SleepMode();
        }
        private void shutdown()
        {
            pcAction.Shutdown();
        }
        private void sleepMode()
        {
            //pcAction.SleepMode();
        }

        private void startMethodAfterTheTimerExpires()
        {
            switch (Status)
            {
                case eStatus.Completed:
                    Status = eStatus.Stop;

                    switch (TypeAction)
                    {
                        case eTypeAction.Shutdown:
                            pcAction.Shutdown();
                            break;
                        case eTypeAction.Restart:
                            pcAction.Reboot();
                            break;
                        case eTypeAction.LogTheUserOut:
                            pcAction.LogOff();
                            break;
                        case eTypeAction.SleepMode:

                            break;
                    }

                    break;
            }
        }
    }
}