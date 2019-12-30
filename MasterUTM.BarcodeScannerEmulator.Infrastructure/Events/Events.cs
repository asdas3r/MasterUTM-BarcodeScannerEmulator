using System.Collections.ObjectModel;

using Prism.Events;
using Prism.Services.Dialogs;

namespace MasterUTM.BarcodeScannerEmulator.Infrastructure.Events
{
    public class UpdateActiveProcessEvent : PubSubEvent<string> { }

    public class ActiveProcessExitedEvent : PubSubEvent<string> { }

    public class TransferedBarcodeIndexEvent : PubSubEvent<int> { }

    public class NotifyAllowStartEvent : PubSubEvent<ObservableCollection<string>> { }

    public class CustomMessageBoxButtonEvent : PubSubEvent<ButtonResult> { }
}
