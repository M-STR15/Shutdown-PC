using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shutdown_PC
{
    public enum TypeModification
    {
        InTime,
        AfterTime
    }
    public enum TypeAction
    {
        Shutdown,
        Restart,
        LogTheUserOut,
        SleapMode
    }
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private TypeModification _typeModification;

        [ObservableProperty]
        private TypeAction _typeAction;
    }
}
