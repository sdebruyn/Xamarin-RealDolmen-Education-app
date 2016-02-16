using System;
using System.Linq;
using System.Reflection;
using Android.Content;
using Android.Content.Res;
using EducationApp.Exceptions;
using EducationApp.Services;

namespace EducationApp.Droid.Services
{
    public class LocalizationProvider : ILocalizedStringProvider
    {
        private readonly ILoggingService _loggingService;
        private readonly Resources _resources;

        public LocalizationProvider(ILoggingService loggingService, Context appContext)
        {
            _loggingService = loggingService;
            _resources = appContext.Resources;
        }

        public string GetLocalizedString(Localized identifier)
        {
            var strIdent = identifier.ToString();
            var translation = GetLocalizedString(strIdent);
            if (translation == strIdent)
            {
                _loggingService.Report(new LocalizationException(identifier), this);
            }
            return translation;
        }

        public string GetLocalizedString(LanguageCode identifier) => GetLocalizedString(identifier.ToString());

        private string GetLocalizedString(string strIdent)
        {
            var stringType = typeof (Resource.String);

            // source: http://stackoverflow.com/questions/10261824/how-can-i-get-all-constants-of-a-type-by-reflection
            var field = stringType
                .GetFields(BindingFlags.Public | BindingFlags.Static |
                           BindingFlags.FlattenHierarchy)
                .FirstOrDefault(fi => fi.IsLiteral && !fi.IsInitOnly && fi.Name == strIdent);

            if (field == null)
            {
                return strIdent;
            }

            var objIdent = field.GetRawConstantValue();
            try
            {
                var numIdent = Convert.ToInt32(objIdent);
                return _resources.GetText(numIdent);
            }
            catch (Exception)
            {
                return strIdent;
            }
        }
    }
}