using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using EducationApp.Extensions;

namespace EducationApp.WinPhone.Converters
{
    public sealed class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
            =>
                value == null || (value is string && ((string) value).IsNullOrWhiteSpace())
                    ? Visibility.Visible
                    : Visibility.Collapsed;

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}