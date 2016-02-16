using System;
using System.Threading.Tasks;
using EducationApp.Models;

namespace EducationApp.Services.Fakes
{
    public class FakeAuthenticationService : IAuthenticationService
    {
        private Identity _identity;

        public Task LoginAsync(object viewReference) => new Task(() => { });

        public Task StoreAndUpdateIdentityAsync(Identity identity) => Task.FromResult(identity);

        public Task LogoutAsync() => new Task(() => { });

        public Task StoreIdentityAsync(Identity identity) => new Task(() => _identity = identity);

        public Task UpdateUserInfoAsync(Identity identity = null) => new Task(() => { });

        /// <exception cref="ArgumentNullException">
        ///     The configuration of <see cref="Constants.Authentication.DefaultRedirectUri" />
        ///     is incorrect.
        /// </exception>
        public Uri GetRedirectUri() => new Uri(Constants.Authentication.DefaultRedirectUri);

        public Task<Identity> GetIdentityAsync() => Task.FromResult(_identity);
    }
}