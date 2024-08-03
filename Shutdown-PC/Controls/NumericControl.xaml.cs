using System.Windows;
using System.Windows.Controls;

namespace Shutdown_PC.Controls
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

        public Visibility VisibilityButtons
        {
            get => (Visibility)GetValue(VisibilityButtonsProperty);
            set => SetValue(VisibilityButtonsProperty, value);
        }

        public NumericControl()
        {
            InitializeComponent();

            MaxTimeValue = 60;
        }

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

        public event EventHandler TimeValueChanged;

        private void onTimeValueChanged() => TimeValueChanged?.Invoke(this, EventArgs.Empty);

        public bool canMinus() => TimeValue > 0;

        public bool canPlus() => TimeValue < MaxTimeValue;

        private static void oVisibilityButtonsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as NumericControl;
            uc.oVisibilityButtonsPropertyChanged(e);
        }

        private void oVisibilityButtonsPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            stcBtns.Visibility = VisibilityButtons;
        }

        private static void onTimeValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uc = d as NumericControl;
            uc.onTimeValuePropertyChanged(e);
        }

        private void minus()
        {
            if (canMinus())
                TimeValue -= 1;
        }

        private void onTimeValuePropertyChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        private void plus()
        {
            if (canPlus())
                TimeValue += 1;
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

        private void setLbl()
        {
            lblTimeValue.Content = (TimeValue < 10 ? "0" : "") + TimeValue.ToString();
        }
    }
}