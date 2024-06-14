using System.Windows.Controls;

namespace Shutdown_PC.Controls.Clock
{
    /// <summary>
    /// Interaction logic for ClockView.xaml
    /// </summary>
    public partial class ClockView : UserControl
    {
        public ClockView()
        {
            InitializeComponent();
            this.DataContext = new ClockViewModel();
        }
    }
}
