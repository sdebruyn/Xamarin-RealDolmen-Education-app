namespace EducationApp
{
    public static class Configuration
    {
        public const string ApiUri = "not provided";
        public const string StsUri = "not provided";
        public const string AuthorizeUri = StsUri + "/connect/authorize";
        public const string UserInfoUri = StsUri + "/connect/userinfo";
        public const string iOSRedirectUri = "not provided";
#if DEBUG
        public const string InsightsKey = "not provided";
#else
        public const string InsightsKey = "not provided";
#endif
    }
}