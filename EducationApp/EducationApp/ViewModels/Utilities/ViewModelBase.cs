using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EducationApp.Services;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace EducationApp.ViewModels.Utilities
{
    public abstract class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase, IDisposable
    {
        protected readonly IAuthenticationService AuthService;
        protected readonly IDialogService DialogService;
        protected readonly IDispatcherHelper DispatcherHelper;
        protected readonly ILocalizedStringProvider Loc;
        protected readonly ILoggingService LoggingService;
        protected readonly INavigationService NavigationService;
        private bool _isDisposed;
        private bool _isLoading;

        private RelayCommand<object> _loginCommand;

        private GalaSoft.MvvmLight.Command.RelayCommand _logoutCommand;

        protected ViewModelBase(IDialogService dialogService, ILocalizedStringProvider loc,
            ILoggingService loggingService, IAuthenticationService authService, IDispatcherHelper dispatcherHelper,
            INavigationService navigationService)
        {
            DialogService = dialogService;
            Loc = loc;
            LoggingService = loggingService;
            AuthService = authService;
            DispatcherHelper = dispatcherHelper;
            NavigationService = navigationService;
        }

        /// <summary>
        ///     Gets the LoginCommand.
        /// </summary>
        public RelayCommand<object> LoginCommand
            =>
                _loginCommand ??
                (_loginCommand =
                    new RelayCommand<object>(async o => await AuthService.LoginAsync(o).ConfigureAwait(true)));

        /// <summary>
        ///     Gets the LogoutCommand.
        /// </summary>
        public GalaSoft.MvvmLight.Command.RelayCommand LogoutCommand
            =>
                _logoutCommand ??
                (_logoutCommand =
                    new GalaSoft.MvvmLight.Command.RelayCommand(
                        async () => await AuthService.LogoutAsync().ConfigureAwait(true)));


        /// <summary>
        ///     Sets and gets the IsLoading property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool IsLoading
        {
            get { return _isLoading; }
            set { Set(ref _isLoading, value, true); }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ViewModelBase()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            if (disposing)
            {
                Cleanup();
            }

            _isDisposed = true;
        }

        protected bool Set<T>(ref T field, T newValue, IEqualityComparer<T> comparer,
            [CallerMemberName] string propertyName = null) => Set(propertyName, ref field, newValue, comparer);

        protected bool Set<T>(string propertyName, ref T field, T newValue, IEqualityComparer<T> comparer)
        {
            if (comparer.Equals(field, newValue))
            {
                return false;
            }
            field = newValue;
            RaisePropertyChanged(propertyName);
            return true;
        }
    }
}