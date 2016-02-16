using System;
using System.Net;
using System.Net.Http;
using EducationApp.Services.Default;
using Fusillade;
using ModernHttpClient;
using Refit;

namespace EducationApp.Services.Web.Utilities
{
    public static class PrioritizedClientFactory
    {
        private static readonly Uri BaseAddress;

        static PrioritizedClientFactory()
        {
            BaseAddress = new Uri(Configuration.ApiUri);
        }

        public static Lazy<IEducationApi> GetClient(Priority priority)
        {
            var nativeHandler = new NativeMessageHandler();
            if (nativeHandler.SupportsAutomaticDecompression)
            {
                nativeHandler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }
            var rateLimitedHandler = new RateLimitedHttpMessageHandler(nativeHandler, priority);
            var authEnabledHandler =
                new AuthenticatedHttpClientHandler(() => BaseAuthenticationService.GetAuthorizationHeaderAsync(),
                    rateLimitedHandler);
            var rateLimitedClient = new HttpClient(authEnabledHandler)
            {
                BaseAddress = BaseAddress
            };
            return new Lazy<IEducationApi>(() => RestService.For<IEducationApi>(rateLimitedClient));
        }
    }
}