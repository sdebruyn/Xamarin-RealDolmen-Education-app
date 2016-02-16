using System;
using System.Threading.Tasks;
using EducationApp.Models;

namespace EducationApp.Services
{
    public interface IAuthenticationService
    {
        Task LoginAsync(object viewReference);
        Task StoreAndUpdateIdentityAsync(Identity identity);
        Task LogoutAsync();
        Task StoreIdentityAsync(Identity identity);
        Task UpdateUserInfoAsync(Identity identity = null);
        Uri GetRedirectUri();
        Task<Identity> GetIdentityAsync();
    }
}