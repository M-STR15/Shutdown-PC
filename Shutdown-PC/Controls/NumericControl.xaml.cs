using System.Windows;
using System.Windows.Controls;

namespace ShutdownPC.Controls
{
    /// <summary>
    /// Interaction logic for NumericControl.xaml
    /// </summary>
    public partial class NumericControl : UserControl
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

        public static readonly DependencyProperty VisibilityButtonsProperty =
            DependencyProperty.Register
            (nameof(VisibilityButtons),
             typeof(Visibility),
             typeof(NumericControl),
             new FrameworkPropertyMetadata(Visibility.Hidden, new PropertyChangedCallback(oVisibilityButtonsPropertyChanged)));

        public NumericControl()
        {
            InitializeComponent();

            MaxTimeValue = 60;
        }

        public event EventHandler TimeValueChanged;

        public int MaxTimeValue
        {
            get => (int)GetValue(MaxTimeValueProperty);
            set => SetValue(MaxTimeValueProperty, value);
        }

        public int TimeValue
        {
            get => (int)GetValue(TimeValueProperty);
            set
            {
                if (TimeValue != value)
                {
                    SetValue(TimeValueProperty, value);
                    onTimeValueChanged();
                    setLbl();
                }
            }
        }

        public Visibility VisibilityButtons
        {
            get => (Visibility)GetValue(VisibilityButtonsProperty);
            set => SetValue(VisibilityButtonsProperty, value);
        }

        public bool canMinus() => TimeValue > 0;

        public bool canPlus() => TimeValue < MaxTimeValue;

        private static void onTimeValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as NumericControl;
            if (uc != null)
                uc.onTimeValuePropertyChanged(e);
        }

        private static void oVisibilityButtonsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as NumericControl;
            if (uc != null)
                uc.oVisibilityButtonsPropertyChanged(e);
        }

        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            minus();
            //btnMinus.IsEnabled = canMinus();
            setLbl();
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            plus();
            //btnPlus.IsEnabled = canPlus();
            setLbl();
        }

        private void minus()
        {
            if (canMinus())
                TimeValue -= 1;
        }

        private void onTimeValueChanged() => TimeValueChanged?.Invoke(this, EventArgs.Empty);

        private void onTimeValuePropertyChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        private void oVisibilityButtonsPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            stcBtns.Visibility = VisibilityButtons;
        }

        private void plus()
        {
            if (canPlus())
                TimeValue += 1;
        }

        private void setLbl()
        {
            var resultText = "00";
            var timeVal = Math.Abs(TimeValue);

            if (TimeValue < -9)
            {
                resultText = timeVal.ToString();
            }
            else if (TimeValue < 0)
            {
                resultText = "0" + timeVal.ToString();
            }
            else if (TimeValue < 10)
            {
                resultText = "0" + timeVal.ToString();
            }
            else
            {
                resultText = timeVal.ToString();
            }

            lblTimeValue.Content = resultText;
        }
    }
}