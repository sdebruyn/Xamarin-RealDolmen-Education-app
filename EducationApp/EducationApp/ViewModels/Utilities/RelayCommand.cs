using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using EducationApp.Services;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;

namespace EducationApp.ViewModels.Utilities
{
    public class RelayCommand : GalaSoft.MvvmLight.Command.RelayCommand, IDisposable
    {
        private ISet<string> _properties;

        public RelayCommand(Action execute)
            : base(execute)
        {
            Initialize();
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
            : base(execute, canExecute)
        {
            Initialize();
        }

        public void Dispose()
        {
            Messenger.Default.Unregister(this);
        }

        private void Initialize()
        {
            Messenger.Default.Register<PropertyChangedMessageBase>(this, true, property =>
            {
                if (_properties != null && _properties.Contains(property.PropertyName))
                {
                    ServiceLocator.Current.GetInstance<IDispatcherHelper>().ExecuteOnUiThread(RaiseCanExecuteChanged);
                }
            });
        }

        public RelayCommand DependsOn<T>(Expression<Func<T>> propertyExpression)
        {
            if (_properties == null)
                _properties = new HashSet<string>();

            _properties.Add(GetPropertyName(propertyExpression));
            return this;
        }

        /// <exception cref="ArgumentNullException"><paramref name="propertyExpression" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException"><paramref name="propertyExpression" /> is not a valid expression.</exception>
        private static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

            var body = propertyExpression.Body as MemberExpression;
            if (body == null)
                throw new ArgumentException("Invalid argument", nameof(propertyExpression));

            var property = body.Member as PropertyInfo;
            if (property == null)
                throw new ArgumentException("Argument is not a property",
                    nameof(propertyExpression));

            return property.Name;
        }
    }
}