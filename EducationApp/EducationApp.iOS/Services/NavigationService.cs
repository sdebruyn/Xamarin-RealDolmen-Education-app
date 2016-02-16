using System;
using System.Collections.Generic;
using System.Linq;
using EducationApp.iOS.Utilities;
using EducationApp.iOS.ViewControllers;
using GalaSoft.MvvmLight.Views;
using UIKit;

namespace EducationApp.iOS.Services
{
    /// <summary>
    ///     Modified version of Laurent Bugnion's <see cref="GalaSoft.MvvmLight.Views.NavigationService" /> with support for
    ///     animation and tab bar navigation.
    /// </summary>
    public class NavigationService : INavigationService
    {
        private const string UnknownPageKey = "-- UNKNOWN --";
        private const string RootPageKey = "-- ROOT --";
        private readonly Dictionary<string, Type> _pagesByKey = new Dictionary<string, Type>();
        private readonly Dictionary<TabBarItemType, string> _tabBarPages = new Dictionary<TabBarItemType, string>();
        private UINavigationController _navigation;
        private string _rootKey;

        private string RootKey => _rootKey ?? RootPageKey;

        /// <exception cref="InvalidCastException">Requested controller is not of type <see cref="BaseViewController" />.</exception>
        /// <exception cref="ArgumentException">The requested page key has not been configured.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, null);
        }

        /// <exception cref="ArgumentException">The requested page key has not been configured.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        /// <exception cref="InvalidCastException">Requested controller is not of type <see cref="BaseViewController" />.</exception>
        public void NavigateTo(string pageKey, object parameter)
        {
            NavigateTo(pageKey, parameter, true);
        }

        /// <summary>
        ///     The key corresponding to the currently displayed page.
        /// </summary>
        public string CurrentPageKey
        {
            get
            {
                lock (_pagesByKey)
                {
                    if (_navigation.ViewControllers.Length == 0)
                    {
                        return UnknownPageKey;
                    }
                    if (_navigation.ViewControllers.Length == 1)
                    {
                        return RootKey;
                    }
                    return GetPageKeyByType();
                }
            }
        }

        /// <summary>
        ///     If possible, discards the current page and displays the previous page
        ///     on the navigation stack.
        /// </summary>
        public void GoBack()
        {
            GoBack(true);
        }

        /// <exception cref="InvalidCastException">Requested controller is not of type <see cref="BaseViewController" />.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        /// <exception cref="ArgumentException">The requested page key has not been configured.</exception>
        public void NavigateTo(string pageKey, bool animated)
        {
            NavigateTo(pageKey, null, animated);
        }

        /// <exception cref="ArgumentException">The requested page has not been configured.</exception>
        /// <exception cref="InvalidCastException">Requested controller is not of type <see cref="BaseViewController" />.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        public void NavigateTo(TabBarItemType tabBarItemType)
        {
            if (!_tabBarPages.ContainsKey(tabBarItemType))
            {
                throw new ArgumentException("The requested page has not been configured.", nameof(tabBarItemType));
            }
            NavigateTo(_tabBarPages[tabBarItemType], null, false, true);
        }

        private string GetPageKeyByType(Type type = null)
        {
            if (type == null && _navigation != null)
            {
                type = _navigation.ViewControllers.LastOrDefault()?.GetType();
            }
            if (type == null)
            {
                return null;
            }
            if (_pagesByKey.Values.FirstOrDefault(t => t == type) == null)
            {
                return null;
            }
            return _pagesByKey.First(p => p.Value == type).Key;
        }

        public void Configure(string key, Type viewControllerType, bool isRoot = false)
        {
            lock (_pagesByKey)
            {
                if (_pagesByKey.ContainsKey(key))
                {
                    _pagesByKey[key] = viewControllerType;
                }
                else
                {
                    _pagesByKey.Add(key, viewControllerType);
                }
                if (isRoot)
                {
                    _rootKey = key;
                }
            }
        }

        public void Configure(TabBarItemType tabBarItemType, Type viewControllerType, string key = null,
            bool isRoot = false)
        {
            lock (_pagesByKey)
            {
                var pageKey = key ?? tabBarItemType.ToString();
                if (_tabBarPages.ContainsKey(tabBarItemType))
                {
                    _tabBarPages[tabBarItemType] = pageKey;
                }
                else
                {
                    _tabBarPages.Add(tabBarItemType, pageKey);
                }
                Configure(pageKey, viewControllerType, isRoot);
            }
        }

        /// <summary>
        ///     If possible, discards the current page and displays the previous page
        ///     on the navigation stack.
        /// </summary>
        public void GoBack(bool animated)
        {
            if (CanGoBack())
            {
                _navigation.PopViewController(animated);
            }
        }

        private bool CanGoBack() => _navigation.ViewControllers.Length > 1;

        /// <exception cref="ArgumentException">The requested page key has not been configured.</exception>
        /// <exception cref="InvalidOperationException">This instance has not been initialized.</exception>
        /// <exception cref="InvalidCastException">Requested controller is not of type <see cref="BaseViewController" />.</exception>
        public void NavigateTo(string pageKey, object parameter, bool animated, bool replace = false)
        {
            lock (_pagesByKey)
            {
                if (pageKey == RootKey)
                {
                    while (CanGoBack())
                    {
                        GoBack(false);
                    }
                    return;
                }

                if (!_pagesByKey.ContainsKey(pageKey))
                {
                    throw new ArgumentException(
                        $"No such page: {pageKey}. Did you forget to call NavigationService.Configure?", nameof(pageKey));
                }

                if (_navigation == null)
                {
                    throw new InvalidOperationException(
                        "Unable to navigate: Did you forget to call NavigationService.Initialize?");
                }

                var item = _pagesByKey[pageKey];
                var controller = _navigation.Storyboard.InstantiateViewController(item.Name);

                if (parameter != null)
                {
                    var casted = controller as INavigationPage;
                    if (casted != null)
                    {
                        casted.NavigationParameter = parameter;
                    }
                    else
                    {
                        throw new InvalidCastException(
                            $"Cannot cast {controller.GetType().FullName} to {nameof(INavigationPage)}");
                    }
                }

                if (replace)
                {
                    _navigation.PopViewController(animated);
                }
                _navigation.PushViewController(controller, animated);
            }
        }

        public void Initialize(UINavigationController navigation)
        {
            _navigation = navigation;
        }
    }
}