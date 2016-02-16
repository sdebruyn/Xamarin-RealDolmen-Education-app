using System;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace EducationApp.WinPhone.Common
{
    /// <summary>
    ///     ContinuationManager is used to detect if the most recent activation was due
    ///     to a continuation such as the FileOpenPicker or WebAuthenticationBroker
    /// </summary>
    internal sealed class ContinuationManager
    {
        private IContinuationActivatedEventArgs _args;
        private bool _handled;
        
        /// <summary>
        ///     Sets the ContinuationArgs for this instance. Using default Frame of current Window
        ///     Should be called by the main activation handling code in App.xaml.cs
        /// </summary>
        /// <param name="args">The activation args</param>
        internal void Continue(IContinuationActivatedEventArgs args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            if (args.Kind != ActivationKind.WebAuthenticationBrokerContinuation) return;

            var rootFrame = Window.Current.Content as Frame;
            
            if (_args != null && !_handled)
                throw new InvalidOperationException("Can't set args more than once");

            _args = args;
            _handled = false;

            if (rootFrame == null)
                return;
            
            var wabPage = rootFrame.Content as IWebAuthenticationContinuable;
            wabPage?.ContinueWebAuthentication(args as WebAuthenticationBrokerContinuationEventArgs);
        }
    }

    /// <summary>
    ///     Implement this interface if your page invokes the web authentication
    ///     broker
    /// </summary>
    internal interface IWebAuthenticationContinuable
    {
        /// <summary>
        ///     This method is invoked when the web authentication broker returns
        ///     with the authentication result
        /// </summary>
        /// <param name="args">Activated event args object that contains returned authentication token</param>
        void ContinueWebAuthentication(WebAuthenticationBrokerContinuationEventArgs args);
    }
}