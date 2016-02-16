using System.Collections.Generic;
using Plugin.Connectivity.Abstractions;

namespace EducationApp
{
    public static class Constants
    {
        public static class Logging
        {
            public const string LogType = "ParentType";
            public const string ExtraInfo = "ExtraInfo";
        }

        public static class Authentication
        {
            public const string Scope = "not provided";
            public const string ResponseType = "not provided";
            public const string ClientId = "not provided";
            public const string DefaultRedirectUri = DefaultRedirectUriScheme + "://auth/";
            public const string DefaultRedirectUriScheme = "not provided";
        }

        public static class Pages
        {
            public const string MainKey = "Main";
            public const string CategoryDetailsKey = "CategoryDetails";
            public const string SubcategoryDetailsKey = "SubcategoryDetails";
            public const string CourseDetailsKey = "CourseDetails";
            public const string SessionsKey = "Sessions";
            public const string SessionKey = "Session";
        }

        public static class Internet
        {
            public const int DefaultResultsPerPage = 50;
            public const byte DefaultCacheDays = 10;

            public static readonly HashSet<ConnectionType> FastConnectionTypes = new HashSet<ConnectionType>
            {
                ConnectionType.Desktop,
                ConnectionType.WiFi
            };

            public static readonly HashSet<ConnectionType> SlowConnectionTypes = new HashSet<ConnectionType>
            {
                ConnectionType.Cellular,
                ConnectionType.Other,
                ConnectionType.Wimax
            };
        }
    }
}