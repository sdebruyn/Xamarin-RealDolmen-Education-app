using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using EducationApp.Models;

namespace EducationApp.WinPhone.Converters
{
    public sealed class SearchStatusFaultedVisibileConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var status = value as SearchStatus? ?? SearchStatus.Inactive;
            return status == SearchStatus.Faulted ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}