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
        public EventHandler StatusChange;
        private readonly WindowStore _windowStore;

        [ObservableProperty]
        private int _endAfterSeconds;

        [ObservableProperty]
        private string _message;

        [ObservableProperty]
        private DateTime _setTimeValue;

        private eStatus _status;
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

        public MainViewModel(WindowStore windowsStore)
        {
            TypeModification = eTypeModification.AfterTime;
            ShutdownCommand = new Helpers.RelayCommand(cmd_Shutdown);
            RestartCommand = new Helpers.RelayCommand(cmd_Restart);
            LogTheUserOutCommnad = new Helpers.RelayCommand(cmd_LogTheUserOut);
            SleepModeCommand = new Helpers.RelayCommand(cmd_SleepMode);
            ChangeStatusCommnad = new Helpers.RelayCommand(cmd_ChangeStatus);
            ShowSettingCommand = new Helpers.RelayCommand(cmd_ShowSetting);

            _windowStore = windowsStore;

            pcAction = new PcActionService();

            Status = eStatus.Stop;

            SetTimeValue = DateTime.Now;
            Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();


            //StatusChange += onStatusChange;
        }

        public ICommand ChangeStatusCommnad { get; private set; }

        //private void onStatusChange(object sender, EventArgs args)
        //{
        //    changeStatus();
        //}
        public ICommand LogTheUserOutCommnad { get; set; }

        public ICommand RestartCommand { get; private set; }

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
                }
            }
        }
        private void cmd_ChangeStatus(object parameter) => changeStatus();

        private void cmd_LogTheUserOut(object parameter) => logOff();

        private void cmd_Restart(object parameter) => reboot();

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
                    break;
            }
        }

        private void logOff()
        {
            pcAction.LogOff();
            //Message = pcAction.Message;
        }
        private void reboot()
        {
            pcAction.Reboot();
            //Message = pcAction.Message;
        }
        private void showSetting()
        {
            _windowStore.ShowSettigWindow();
            //pcAction.SleepMode();
            //Message = pcAction.Message;
        }
        private void shutdown()
        {
            pcAction.Shutdown();
            //Message=pcAction.Message;
        }
        private void sleepMode()
        {
            //pcAction.SleepMode();
            //Message = pcAction.Message;
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

            _windowStore.ShowInfoWindow();
        }
    }
}