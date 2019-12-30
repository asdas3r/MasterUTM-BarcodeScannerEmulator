using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;

using MasterUTM.BarcodeScannerEmulator.Infrastructure;
using MasterUTM.BarcodeScannerEmulator.Infrastructure.Events;
using MasterUTM.BarcodeScannerEmulator.Mvvm.ViewModels;

namespace MasterUTM.BarcodeScannerEmulator.Mvvm.Dialogs.ViewModels
{
    public class CustomMessageBoxViewModel : DialogBaseViewModel
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;

        public CustomMessageBoxViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<CustomMessageBoxButtonEvent>().Subscribe(OnCustomMessageBoxButtonEvent);
        }

        private void OnCustomMessageBoxButtonEvent(ButtonResult result)
        {
            RaiseRequestClose(new DialogResult(result));
        }

        public DelegateCommand OkCommand { get; }

        private bool _imageIxists;
        public bool ImageExists
        {
            get { return _imageIxists; }
            set { SetProperty(ref _imageIxists, value); }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private string _recievedIcon;
        public string RecievedIcon
        {
            get { return _recievedIcon; }
            set 
            {
                SetProperty(ref _recievedIcon, value);
                
                RaisePropertyChanged("MessageImageSource");
            }
        }

        public ImageSource MessageImageSource
        {
            get
            {
                if (DialogIcon != null)
                    return Imaging.CreateBitmapSourceFromHIcon(DialogIcon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                else
                    return null;
            }
        }

        public Icon DialogIcon
        {
            get
            {
                Icon icon;

                if (RecievedIcon == null)
                    return null;

                switch (RecievedIcon.ToLower())
                {
                    case "error":
                        icon = SystemIcons.Error;
                        break;
                    case "warning":
                        icon = SystemIcons.Warning;
                        break;
                    case "information":
                        icon = SystemIcons.Information;
                        break;
                    case "exclamation":
                        icon = SystemIcons.Exclamation;
                        break;
                    case "question":
                        icon = SystemIcons.Question;
                        break;
                    case "asterisk":
                        icon = SystemIcons.Asterisk;
                        break;
                    default:
                        icon = null;
                        break;
                }

                return icon;
            }
        }

        public void DialogButtons(string buttons)
        {
            switch (buttons.ToLower())
            {
                case "yesno":
                    _regionManager.RequestNavigate(RegionNames.DialogButtonsRegion, "YesNoDialogView", new NavigationParameters("yesString=Подтвердить&noString=Отменить"));
                    break;
                case "ok":
                    _regionManager.RequestNavigate(RegionNames.DialogButtonsRegion, "OkDialogView");
                    break;
                default:
                    _regionManager.RequestNavigate(RegionNames.DialogButtonsRegion, "OkDialogView");
                    break;
            }
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            Title = parameters.GetValue<string>("title");
            string message = parameters.GetValue<string>("message");
            string systemIcon = parameters.GetValue<string>("systemIcon");
            string dialogButtons = parameters.GetValue<string>("dialogButtons");

            Message = message;
            RecievedIcon = systemIcon;
            ImageExists = (systemIcon == null) ? false : true;
            DialogButtons(dialogButtons);
            System.Media.SystemSounds.Beep.Play();
        }

        public override void OnDialogClosed()
        {
            _regionManager.Regions.Remove(RegionNames.DialogButtonsRegion);
        }

    }
}
