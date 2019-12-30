using System.Collections.Generic;
using System.ComponentModel;

using Prism.Mvvm;
using Prism.Regions;

namespace MasterUTM.BarcodeScannerEmulator.Mvvm.ViewModels
{
    public class BaseViewModel : BindableBase, INavigationAware, IDataErrorInfo
    {
        public bool IsInitialNavigationDone { get; set; } = false;

        private string title;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        #region DataError

        private bool _isDataCorrect;
        public bool IsDataCorrect
        {
            get { return _isDataCorrect; }
            set { SetProperty(ref _isDataCorrect, value); }
        }

        public string Error
        {
            get { return null; }
        }

        protected readonly Dictionary<string, string> errorCol = new Dictionary<string, string>();

        public string this[string columnName]
        {
            get
            {
                string error = null;

                ErrorLogic(columnName, ref error);

                if (error != null && !errorCol.ContainsKey(columnName))
                {
                    errorCol.Add(columnName, error);
                }
                else if (error == null && errorCol.ContainsKey(columnName))
                {
                    errorCol.Remove(columnName);
                }

                IsDataCorrect = errorCol.Count == 0;

                return error;
            }
        }

        public virtual void ErrorLogic(string columnName, ref string error)
        {

        }

        #endregion DataError
    }
}
