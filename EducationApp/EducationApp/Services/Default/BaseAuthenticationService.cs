using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using EducationApp.Exceptions;
using EducationApp.Messaging;
using EducationApp.Models;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using IdentityModel.Client;
using ModernHttpClient;
using Newtonsoft.Json.Linq;
using PCLCrypto;

namespace EducationApp.Services.Default
{
    public abstract class BaseAuthenticationService : IAuthenticationService
    {
        private readonly IBlobCache _cache;
        protected readonly IDialogService DialogService;
        protected readonly ILocalizedStringProvider Loc;

        protected BaseAuthenticationService(IDialogService dialogService, ILocalizedStringProvider loc)
        {
            DialogService = dialogService;
            Loc = loc;
            _cache = BlobCache.Secure;
        }

        public async Task LoginAsync(object viewReference)
        {
            await LogoutAsync().ConfigureAwait(true);
            StartLoginUi(viewReference);
        }

        public async Task StoreAndUpdateIdentityAsync(Identity identity)
        {
            await UpdateUserInfoAsync(identity).ConfigureAwait(false);
            Messenger.Default.Send(new AuthenticationChangedMessage(identity));
            await StoreIdentityAsync(identity).ConfigureAwait(false);
        }

        public async Task LogoutAsync()
        {
            await Task.Run(() => _cache.InvalidateAll()).ConfigureAwait(false);
            Messenger.Default.Send(new AuthenticationChangedMessage(null));
        }

        public async Task StoreIdentityAsync(Identity identity)
            => await _cache.InsertObject(nameof(Identity), identity, new TimeSpan(2, 0, 0, 0, 0));

        /// <exception cref="AuthenticationException"><paramref name="identity" /> not valid or not found.</exception>
        /// <exception cref="ArgumentNullException">Configuration of <see cref="Configuration.UserInfoUri" /> incorrect.</exception>
        public async Task UpdateUserInfoAsync(Identity identity = null)
        {
            string content;

            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                client.DefaultRequestHeaders.Authorization =
                    AuthenticationHeaderValue.Parse("Bearer " +
                                                    await GetAuthorizationHeaderAsync(identity).ConfigureAwait(false));
                var response = await client.GetAsync(Configuration.UserInfoUri).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    await LogoutAsync().ConfigureAwait(false);
                    throw new AuthenticationException(response.StatusCode, response.ReasonPhrase);
                }

                content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }

            var jObj = JObject.Parse(content);
            var claims = new List<Claim>();
            foreach (var jPair in jObj)
            {
                claims.Add(new Claim(jPair.Key, jPair.Value.ToString()));
            }

            var fromCache = false;
            if (identity == null)
            {
                identity = await GetIdentityAsync().ConfigureAwait(false);
                if (identity == null)
                {
                    throw new AuthenticationException("No cached identity found, but user info update requested.");
                }
                fromCache = true;
            }

            identity.AddClaims(claims.ToArray());

            if (fromCache)
            {
                await StoreIdentityAsync(identity).ConfigureAwait(false);
                Messenger.Default.Send(new AuthenticationChangedMessage(identity));
            }
        }

        public virtual Uri GetRedirectUri() => new Uri(Constants.Authentication.DefaultRedirectUri);

        public Task<Identity> GetIdentityAsync() => GetIdentityStaticAsync();

        public static async Task<string> GetAuthorizationHeaderAsync(Identity identity = null)
        {
            if (identity == null)
            {
                identity = await GetIdentityStaticAsync().ConfigureAwait(false);
            }
            var header = identity.GetClaim(Claim.AccessTokenName);
            return header.Value;
        }

        private static async Task<Identity> GetIdentityStaticAsync()
        {
            var cache = BlobCache.Secure;
            await cache.Flush();
            Identity identity;
            try
            {
                identity = await cache.GetObject<Identity>(nameof(Identity));
            }
            catch (KeyNotFoundException)
            {
                identity = null;
            }
            return identity;
        }

        protected abstract void StartLoginUi(object viewReference);

        protected Uri GetAuthenticationUri()
        {
            var authRequest = new AuthorizeRequest(Configuration.AuthorizeUri);
            var redirectUri = GetRedirectUri().AbsoluteUri;
            var authUrl = authRequest.CreateAuthorizeUrl(Constants.Authentication.ClientId,
                Constants.Authentication.ResponseType, Constants.Authentication.Scope,
                redirectUri, nonce: GenerateNOnce());
            return new Uri(authUrl);
        }

        /// <summary>
        ///     Generates a secure random string.
        /// </summary>
        /// <param name="length">Length of the requested string.</param>
        /// <returns>Secure random string</returns>
        /// <remarks>
        ///     Source:
        ///     <see cref="https://github.com/wcabus/MADN-oAuth/blob/master/Timesheet.App/ViewModels/LoginViewModel.cs" />
        /// </remarks>
        private static string GenerateNOnce(int length = 16)
        {
            var rndBuffer = WinRTCrypto.CryptographicBuffer.GenerateRandom(length);
            return Convert.ToBase64String(rndBuffer.ToArray());
        }

        protected void AuthenticatorOnError(object sender = null,
            EventArgs eventArgs = null)
        {
            DialogService.ShowError(Loc.GetLocalizedString(Localized.AuthenticationError), null, null,
                null);
        }
    }
}