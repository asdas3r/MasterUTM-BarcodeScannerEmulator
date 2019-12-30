using System;

using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace MasterUTM.BarcodeScannerEmulator.Mvvm.ViewModels
{
    public class DialogBaseViewModel : BindableBase, IDialogAware
    {
        private string title;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public event Action<IDialogResult> RequestClose;

        public virtual void CloseDialog()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.None, null));
        }

        public virtual void RaiseRequestClose(IDialogResult result)
        {
            RequestClose?.Invoke(result);
        }

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnDialogClosed()
        {

        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        {

        }
    }
}
