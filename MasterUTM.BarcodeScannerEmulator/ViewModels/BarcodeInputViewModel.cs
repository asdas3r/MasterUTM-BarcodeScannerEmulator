using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections.Generic;

using Prism.Commands;
using Prism.Services.Dialogs;

using MasterUTM.BarcodeScannerEmulator.Infrastructure.Interfaces;
using MasterUTM.BarcodeScannerEmulator.Mvvm.ViewModels;
using MasterUTM.BarcodeScannerEmulator.Core.Common;

namespace MasterUTM.BarcodeScannerEmulator.ViewModels
{
    public class BarcodeInputViewModel : DialogBaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IFileDialogService _fileDialogService;
        private readonly IFileService<List<string>> _textFileService;

        private readonly BarcodeLogic barcodeLogic;

        public ObservableCollection<string> BarcodeCollection { get; }

        public BarcodeInputViewModel(IDialogService dialogService, IFileDialogService fileDialogService, IFileService<List<string>> textFileService)
        {
            _dialogService = dialogService;
            _fileDialogService = fileDialogService;
            _textFileService = textFileService;

            Title = "Пользовательские штрихкоды";
            
            barcodeLogic = new BarcodeLogic();

            BarcodeCollection = new ObservableCollection<string>();
            BarcodeCollection.CollectionChanged += BarcodeCollection_CollectionChanged;

            AddBarcodeCommand = new DelegateCommand<string>(AddBarcode, CanAddBarcode).ObservesProperty(() => InputFieldText);
            EditBarcodeCommand = new DelegateCommand(EditBarcode, CanRemoveOrEditBarcode);
            CancelEditBarcodeCommand = new DelegateCommand(CancelEdit);
            ConfirmEditBarcodeCommand = new DelegateCommand<string>(ConfirmEdit);
            RemoveBarcodeCommand = new DelegateCommand(RemoveBarcode, CanRemoveOrEditBarcode);
            ImportCommand = new DelegateCommand(ImportFromFile, CanImport).ObservesProperty(() => IsEditMode);
        }

        public DelegateCommand<string> AddBarcodeCommand { get; }
        public DelegateCommand CancelEditBarcodeCommand { get; }
        public DelegateCommand<string> ConfirmEditBarcodeCommand { get; }
        public DelegateCommand EditBarcodeCommand { get; }
        public DelegateCommand RemoveBarcodeCommand { get; }
        public DelegateCommand ImportCommand { get; }

        private string _inputFieldText;
        public string InputFieldText
        {
            get { return _inputFieldText; }
            set { SetProperty(ref _inputFieldText, value); }
        }

        private string _selectedItem;
        public string SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                SetProperty(ref _selectedItem, value);
                if (IsEditMode)
                {
                    InputFieldText = string.Empty;
                    IsEditMode = !IsEditMode;
                }
                RaiseCanRemoveOrEdit();
            }
        }

        private bool _isEditMode = false;
        public bool IsEditMode
        {
            get { return _isEditMode; }
            set 
            { 
                if (SetProperty(ref _isEditMode, value))
                {
                    RaiseCanRemoveOrEdit();
                } 
            }
        }

        private void BarcodeCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaiseCanRemoveOrEdit();
        }

        private void AddBarcode(string barcode)
        {
            string error = GetValidationError(barcode);

            if (string.IsNullOrWhiteSpace(error))
            {
                BarcodeCollection.Add(barcode);
                InputFieldText = string.Empty;
            }
            else
            {
                _dialogService.ShowDialog("CustomMessageBoxView", new DialogParameters($"message={error}&systemIcon=Error&dialogButtons=Ok&title=Ошибка"), null);
            } 
        }

        private bool CanAddBarcode(string arg)
        {
            return !string.IsNullOrWhiteSpace(InputFieldText);
        }

        private void RemoveBarcode()
        {
            string message = "Вы действительно хотите удалить данный элемент? Восстановление будет невозможно!";
            _dialogService.ShowDialog("CustomMessageBoxView", new DialogParameters($"message={message}&systemIcon=Warning&dialogButtons=YesNo&title=Удаление"), OnDelete);
        }

        private void OnDelete(IDialogResult result)
        {
            if (result.Result == ButtonResult.Yes)
                BarcodeCollection.Remove(SelectedItem);
        }

        private void EditBarcode()
        {
            InputFieldText = SelectedItem;
            IsEditMode = true;
        }

        private bool CanRemoveOrEditBarcode()
        {
            return BarcodeCollection != null && BarcodeCollection.Count > 0 && !IsEditMode && !string.IsNullOrWhiteSpace(SelectedItem);
        }

        private void RaiseCanRemoveOrEdit()
        {
            EditBarcodeCommand.RaiseCanExecuteChanged();
            RemoveBarcodeCommand.RaiseCanExecuteChanged();
        }

        private void ConfirmEdit(string inputString)
        {
            string error = GetValidationError(inputString);

            if (string.IsNullOrWhiteSpace(error))
            {
                BarcodeCollection[BarcodeCollection.IndexOf(SelectedItem)] = inputString;
                InputFieldText = string.Empty;
                IsEditMode = false;
            }
            else
            {
                _dialogService.ShowDialog("CustomMessageBoxView", new DialogParameters($"message={error}&systemIcon=Error&dialogButtons=Ok&title=Ошибка"), null);
            }
        }

        private void CancelEdit()
        {
            InputFieldText = string.Empty;
            IsEditMode = false;
        }

        private void ImportFromFile()
        {
            if (_fileDialogService.OpenFileDialog())
            {
                const string noError = "Нет ошибки";
                List<string> barcodeList = _textFileService.GetFromFile(_fileDialogService.FilePath) as List<string>;
                List<Tuple<string, string>> barcodeToImportResult = new List<Tuple<string, string>>();
                int errorAmount = barcodeList.Count;

                foreach (var bc in barcodeList)
                {
                    string error = GetValidationError(bc);

                    if (string.IsNullOrEmpty(error))
                    {
                        error = noError;
                        errorAmount--;
                    }

                    barcodeToImportResult.Add(new Tuple<string, string>(bc, error));
                }

                if (errorAmount > 0 && errorAmount < barcodeList.Count && barcodeToImportResult.Count > 0)
                {
                    string msg = $"Импорт был совершен не полностью. Условиям удовлетворяют {barcodeList.Count-errorAmount} из {barcodeList.Count} штрихкодов. Добавить их?"; 
                    _dialogService.ShowDialog("CustomMessageBoxView", new DialogParameters($"message={msg}&systemIcon=Warning&dialogButtons=YesNo&title=Результат"), r =>
                    {
                        if (r.Result == ButtonResult.Yes)
                        {
                            foreach (var bc in barcodeToImportResult)
                            {
                                if (bc.Item2.Equals(noError))
                                    BarcodeCollection.Add(bc.Item1);
                            }
                        }
                    });
                }
                else if (errorAmount == 0 && barcodeToImportResult.Count > 0)
                {
                    foreach (var bc in barcodeToImportResult)
                    {
                        BarcodeCollection.Add(bc.Item1);
                    }
                    string msg = $"Импорт был завершен успешно! Добавлены все ({barcodeList.Count}) штрихкоды.";
                    _dialogService.ShowDialog("CustomMessageBoxView", new DialogParameters($"message={msg}&systemIcon=Asterisk&dialogButtons=Ok&title=Результат"), null);
                }
                else
                {
                    string msg = $"Импорт был завершен с ошибкой. Не удалось добавить штрихкоды ({barcodeList.Count}) из файла.";
                    _dialogService.ShowDialog("CustomMessageBoxView", new DialogParameters($"message={msg}&systemIcon=Error&dialogButtons=Ok&title=Результат"), null);
                }
            }
        }

        private bool CanImport()
        {
            return !IsEditMode;
        }

        private string GetValidationError(string barcode)
        {
            barcodeLogic.ValidateBarcode(barcode, out string error, true);

            if (BarcodeCollection.Contains(barcode))
                error = "Данный штрихкод уже добавлен в список";

            return error;
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            ObservableCollection<string> recievedCollection = parameters.GetValue<ObservableCollection<string>>("barcodeCollection");
            if (recievedCollection == null)
                return;

            BarcodeCollection.Clear();
            foreach (var bc in recievedCollection)
                BarcodeCollection.Add(bc);
        }

        public override void CloseDialog()
        {
            DialogParameters parameters = new DialogParameters();
            parameters.Add("barcodeCollection", BarcodeCollection);
            RaiseRequestClose(new DialogResult(ButtonResult.None, parameters));
        }
    }
}
