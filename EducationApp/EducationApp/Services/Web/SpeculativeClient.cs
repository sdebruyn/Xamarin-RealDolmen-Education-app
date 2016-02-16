using System;
using EducationApp.Services.Web.Utilities;
using Fusillade;

namespace EducationApp.Services.Web
{
    public class SpeculativeClient : ISpeculativeClient
    {
        public SpeculativeClient()
        {
            LazyClient = PrioritizedClientFactory.GetClient(Priority.Speculative);
        }

        public Lazy<IEducationApi> LazyClient { get; }
        public IEducationApi Client => LazyClient.Value;
    }
}