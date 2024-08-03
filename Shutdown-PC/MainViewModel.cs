using CommunityToolkit.Mvvm.ComponentModel;
using ShutdownPC.Models.Enums;
using ShutdownPC.Services;
using ShutdownPC.Stores;
using System.Windows.Input;

namespace ShutdownPC
{
    [ObservableObject]
    public partial class MainViewModel 
    {
        private readonly WindowStore _windowStore;

        [ObservableProperty]
        private int _endAfterSeconds;

        [ObservableProperty]
        private string _message;

        [ObservableProperty]
        private DateTime _setTimeValue;

        [ObservableProperty]
        private eStatus _status;

        [ObservableProperty]
        private eTypeAction _typeAction;

        [ObservableProperty]
        private eTypeModification _typeModification;

        [ObservableProperty]
        private string _version;

        private PcActionService pcAction;
        public MainViewModel(WindowStore windowsStore)
        {
            TypeModification = eTypeModification.AfterTime;
            ShutdownCommand = new Helpers.RelayCommand(shutdown);
            RestartCommand = new Helpers.RelayCommand(restart);
            LogTheUserOutCommnad = new Helpers.RelayCommand(logTheUserOut);
            SleepModeCommand = new Helpers.RelayCommand(sleepMode);
            StartCommand = new Helpers.RelayCommand(changeStatus);
            ShowSettingCommand = new Helpers.RelayCommand(showSetting);

            _windowStore = windowsStore;

            pcAction = new PcActionService();

            Status = eStatus.Stop;

            SetTimeValue = DateTime.Now;
            Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public ICommand LogTheUserOutCommnad { get; set; }
        public ICommand RestartCommand { get; private set; }
        public ICommand ShowSettingCommand { get; private set; }
        public ICommand ShutdownCommand { get; private set; }
        public ICommand SleepModeCommand { get; private set; }
        public ICommand StartCommand { get; private set; }
        private void changeStatus(object parameter)
        {
            if (Status == eStatus.Run)
            {
                Status = eStatus.Stop;
            }
            else
            {
                Status = eStatus.Run;
            }
        }

        private void logTheUserOut(object parameter)
        {
            pcAction.LogOff();
            //Message = pcAction.Message;
        }

        private void restart(object parameter)
        {
            pcAction.Reboot();
            //Message = pcAction.Message;
        }

        private void showSetting(object parameter)
        {
            _windowStore.ShowSettigWindow();
            //pcAction.SleepMode();
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
    }
}