using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using EducationApp.Extensions;

namespace EducationApp.WinPhone.Converters
{
    public sealed class NotNullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var visibility = value == null || (value is string && ((string) value).IsNullOrWhiteSpace())
                ? Visibility.Collapsed
                : Visibility.Visible;
            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}