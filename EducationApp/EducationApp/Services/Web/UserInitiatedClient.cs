using System;
using EducationApp.Services.Web.Utilities;
using Fusillade;

namespace EducationApp.Services.Web
{
    public class UserInitiatedClient : IUserInitiatedClient
    {
        public UserInitiatedClient()
        {
            LazyClient = PrioritizedClientFactory.GetClient(Priority.UserInitiated);
        }

        public Lazy<IEducationApi> LazyClient { get; }
        public IEducationApi Client => LazyClient.Value;
    }
}