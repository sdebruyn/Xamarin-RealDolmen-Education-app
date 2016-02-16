using System;
using EducationApp.Services.Web.Utilities;
using Fusillade;

namespace EducationApp.Services.Web
{
    public class BackgroundClient : IBackgroundClient
    {
        public BackgroundClient()
        {
            LazyClient = PrioritizedClientFactory.GetClient(Priority.Background);
        }

        public Lazy<IEducationApi> LazyClient { get; }
        public IEducationApi Client => LazyClient.Value;
    }
}