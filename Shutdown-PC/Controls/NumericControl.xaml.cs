using Shutdown_PC.Helpers;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Shutdown_PC.Controls
{
    /// <summary>
    /// Interaction logic for NumericControl.xaml
    /// </summary>
    public partial class NumericControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty MaxTimeValueProperty =
            DependencyProperty.Register
            (nameof(MaxTimeValue),
             typeof(int),
             typeof(NumericControl));

        public static readonly DependencyProperty TimeValueProperty =
            DependencyProperty.Register
            (nameof(TimeValue),
             typeof(int),
             typeof(NumericControl),
             new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty PreviousValueProperty =
            DependencyProperty.Register
            (nameof(PreviousValue),
             typeof(int),
             typeof(NumericControl),
             new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public NumericControl()
        {
            InitializeComponent();

            MaxTimeValue = 60;
            PlusCommand = new RelayCommand(plus, canPlus);
            MinusCommand = new RelayCommand(minus, canMinus);
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int MaxTimeValue
        {
            get => (int)GetValue(MaxTimeValueProperty);
            set => SetValue(MaxTimeValueProperty, value);
        }

        public int PreviousValue
        {
            get => (int)GetValue(PreviousValueProperty);
            set => SetValue(PreviousValueProperty, value);
        }

        public ICommand MinusCommand { get; }

        public ICommand PlusCommand { get; }

        public int TimeValue
        {
            get => (int)GetValue(TimeValueProperty);
            set => SetValue(TimeValueProperty, value);
        }
        public bool canMinus() => TimeValue > -1 && (PreviousValue != 0 || TimeValue > 0);

        public bool canPlus() => TimeValue < MaxTimeValue;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static void onTimeValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as NumericControl;
            uc.onTimeValuePropertyChanged(e);

            if (uc != null)
            {
                uc.OnPropertyChanged(nameof(TimeValue));
            }
        }
        private void minus(object paramter)
        {
            if (canMinus())
                TimeValue -= 1;

            if (TimeValue == -1)
            {
                if (PreviousValue != 0)
                {
                    PreviousValue--;
                    TimeValue = 59;
                }
            }
        }

        private void onTimeValuePropertyChanged(DependencyPropertyChangedEventArgs e)
        {

        }

        private void plus(object parameter)
        {
            if (canPlus())
                TimeValue += 1;

            if (TimeValue == 60)
            {
                PreviousValue++;
                TimeValue = 0;
            }
        }
    }
}
