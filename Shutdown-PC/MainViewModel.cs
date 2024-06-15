using CommunityToolkit.Mvvm.ComponentModel;
using Shutdown_PC.Models.Enums;
using Shutdown_PC.Services;
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
        public MainViewModel()
        {
            TypeModification = eTypeModification.InTime;
            ShutdownCommand = new Helpers.RelayCommand(shutdown);
            RestartCommand = new Helpers.RelayCommand(restart);
            LogTheUserOutCommnad = new Helpers.RelayCommand(logTheUserOut);
            SleepModeCommand = new Helpers.RelayCommand(sleepMode);
            StartCommand = new Helpers.RelayCommand(start);

            pcAction = new PcActionService();

            t_CountdownTimer = new DispatcherTimer();
            t_CountdownTimer.Interval = new TimeSpan(0, 0, 1);
            t_CountdownTimer.Tick += onCountdown_Tick;
            Status = eStatus.Stop;
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

        public ICommand RestartCommand { get; set; }
        public ICommand ShutdownCommand { get; set; }
        public ICommand SleepModeCommand { get; set; }
        public ICommand StartCommand { get; set; }
        private void logTheUserOut(object parameter)
        {
            pcAction.LogOff();
            //Message = pcAction.Message;
        }

        private void onCountdown_Tick(object sender, EventArgs args)
        {

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

        private void start(object parameter)
        {
            if (Status == eStatus.Run)
                Status = eStatus.Stop;
            else
                Status = eStatus.Run;
        }
    }
}
