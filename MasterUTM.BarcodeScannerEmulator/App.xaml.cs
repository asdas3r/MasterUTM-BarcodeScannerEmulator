using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using System.Threading.Tasks;

using Prism.Ioc;
using Prism.Modularity;
using DevExpress.Xpf.Core;

using MasterUTM.BarcodeScannerEmulator.Infrastructure.Interfaces;
using MasterUTM.BarcodeScannerEmulator.Infrastructure.Services;
using MasterUTM.BarcodeScannerEmulator.Mvvm.Views;
using MasterUTM.BarcodeScannerEmulator.Mvvm.Dialogs.Views;
using MasterUTM.BarcodeScannerEmulator.Infrastructure.Commands;

namespace MasterUTM.BarcodeScannerEmulator
{
    public partial class App
    {
        protected override Window CreateShell()
        {
            DXSplashScreen.Show<SplashScreenView>();
            ApplicationThemeHelper.ApplicationThemeName = "Office2019White";
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialogWindow<CustomDialogWindow>();

            containerRegistry.RegisterDialog<CustomMessageBoxView>("CustomMessageBoxView");

            containerRegistry.RegisterForNavigation<YesNoDialogView>("YesNoDialogView");
            containerRegistry.RegisterForNavigation<OkDialogView>("OkDialogView");

            containerRegistry.RegisterSingleton<IProcessLeaderService, ProcessLeaderService>();
            containerRegistry.RegisterSingleton<ITransferService, TransferService>();
            containerRegistry.RegisterSingleton<IApplicationCommands, ApplicationCommands>();
            containerRegistry.RegisterSingleton<IDataService, OptionsDataService>();

            containerRegistry.Register<IFileService<List<string>>, TextFileService>();
            containerRegistry.Register<IFileDialogService, TextFileDialogService>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<MainModule>();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            RegisterUnhandledGlobalExceptions();
        }

        private void RegisterUnhandledGlobalExceptions()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Dispatcher.UnhandledException += Dispatcher_UnhandledException;
            Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
        #if DEBUG
        #else
            ReportUnhandledException(e.ExceptionObject as Exception);
        #endif
        }

        private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
        #if DEBUG
            e.Handled = false;
        #else
            e.Handled = true;
            ReportUnhandledException(e.Exception);
        #endif
        }

        private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
        #if DEBUG
            e.Handled = false;
        #else
            e.Handled = true;
            ReportUnhandledException(e.Exception);
        #endif
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
        #if DEBUG
        #else
            ReportUnhandledException(e.Exception);
        #endif
        }

        private void ReportUnhandledException(Exception exception)
        {
            MessageBox.Show("Критическая ошибка: " + exception.Message + "\nПриложение будет завершено.");
            Shutdown();
        }
    }
}
