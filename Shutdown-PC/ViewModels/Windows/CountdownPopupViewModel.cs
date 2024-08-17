﻿using CommunityToolkit.Mvvm.ComponentModel;
using ShutdownPC.Stores;

namespace ShutdownPC.ViewModels.Windows
{
    public partial class CountdownPopupViewModel : BaseWindowViewModel
    {
        private readonly DateTime _setTimeValue;

        [ObservableProperty]
        private string _hours;

        [ObservableProperty]
        private string _minutes;

        [ObservableProperty]
        private string _seconds;
        public CountdownPopupViewModel(WindowStore windowStore, MainViewModel mainViewModel) : base(windowStore)
        {
            _setTimeValue = mainViewModel.SetTimeValue;
            mainViewModel.SetTimeValueChange += new EventHandler(updateTime_SetTimeValue);
        }

        private string convert(int value) => (value < 10 ? "0" : "") + value.ToString();

        private void updateTime_SetTimeValue(object sender, EventArgs args)
        {
            TimeSpan timeToEnd = _setTimeValue - DateTime.Now;

            Hours = convert(timeToEnd.Hours);
            Minutes = convert(timeToEnd.Minutes);
            Seconds = convert(timeToEnd.Seconds);
        }
    }
}