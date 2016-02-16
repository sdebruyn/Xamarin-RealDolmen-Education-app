using System;
using Windows.UI.Xaml.Data;

namespace EducationApp.WinPhone.Converters
{
    public sealed class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
            => parameter == null ? value : string.Format((string) parameter, value);

        public object ConvertBack(object value, Type targetType, object parameter, string language) => value;
    }
}