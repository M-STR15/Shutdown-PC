using CommunityToolkit.Mvvm.ComponentModel;
using Shutdown_PC.Models.Enums;
using Shutdown_PC.Services;
using Shutdown_PC.Stores;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Shutdown_PC
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private DateTime _endDateTime;

        private string _message;

        [ObservableProperty]
        private eStatus _status;

        [ObservableProperty]
        private eTypeAction _typeAction;

        [ObservableProperty]
        private eTypeModification _typeModification;
        private PcActionService pcAction;
        private DispatcherTimer t_CountdownTimer;

        //[ObservableProperty]
        private DateTime _setTimeValue;

        public DateTime SetTimeValue
        {
            get => _setTimeValue;
            set
            {
                _setTimeValue = value;
                EndDateTime = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(EndDateTime));
            }
        }

        private int _secondsToEnd;
        public int SecondsToEnd
        {
            get => _secondsToEnd;
            set
            {
                _secondsToEnd = value;
                OnPropertyChanged();
            }
        }

        private readonly WindowStore _windowStore;
        public MainViewModel(WindowStore windowsStore)
        {
            TypeModification = eTypeModification.InTime;
            ShutdownCommand = new Helpers.RelayCommand(shutdown);
            RestartCommand = new Helpers.RelayCommand(restart);
            LogTheUserOutCommnad = new Helpers.RelayCommand(logTheUserOut);
            SleepModeCommand = new Helpers.RelayCommand(sleepMode);
            StartCommand = new Helpers.RelayCommand(changeStatus);
            ShowSettingCommand = new Helpers.RelayCommand(showSetting);

            _windowStore = windowsStore;

            pcAction = new PcActionService();

            t_CountdownTimer = new DispatcherTimer();
            t_CountdownTimer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            t_CountdownTimer.Tick += new EventHandler(onCountdown_Tick);
            Status = eStatus.Stop;

            SetTimeValue = DateTime.Now;
        }

        public int Countdown { get; private set; }
        public ICommand LogTheUserOutCommnad { get; set; }
        public string Message
        {
            get => _message;
            private set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public ICommand RestartCommand { get; private set; }
        public ICommand ShutdownCommand { get; private set; }
        public ICommand SleepModeCommand { get; private set; }
        public ICommand StartCommand { get; private set; }
        public ICommand ShowSettingCommand { get; private set; }
        private void logTheUserOut(object parameter)
        {
            pcAction.LogOff();
            //Message = pcAction.Message;
        }

        private void onCountdown_Tick(object sender, EventArgs args)
        {
            if (SetTimeValue <= DateTime.Now)
            {
                MessageBox.Show("test timeru");
                t_CountdownTimer.Stop();
            }

            SecondsToEnd = (int)(SetTimeValue - DateTime.Now).TotalSeconds;
        }
        private void restart(object parameter)
        {
            pcAction.Reboot();
            //Message = pcAction.Message;
        }

        private void shutdown(object parameter)
        {
            pcAction.Shutdown();
            //Message=pcAction.Message;
        }
        private void sleepMode(object parameter)
        {
            //pcAction.SleepMode();
            //Message = pcAction.Message;
        }

        private void showSetting(object parameter)
        {
            _windowStore.ShowSettigWindow();
            //pcAction.SleepMode();
            //Message = pcAction.Message;
        }

        private void changeStatus(object parameter)
        {
            if (Status == eStatus.Run)
            {
                Status = eStatus.Stop;
                t_CountdownTimer.Stop();
            }
            else
            {
                Status = eStatus.Run;
                t_CountdownTimer.Start();
            }
        }
    }
}
