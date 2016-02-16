using System;
using Windows.UI.Xaml.Data;
using EducationApp.Services;
using Microsoft.Practices.ServiceLocation;

namespace EducationApp.WinPhone.Converters
{
    public sealed class LanguageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var code = (LanguageCode) value;
            return ServiceLocator.Current.GetInstance<ILocalizedStringProvider>().GetLocalizedString(code);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}