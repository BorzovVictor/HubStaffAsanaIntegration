using System.Threading.Tasks;
using Flurl.Http;
using HI.SharedKernel.Models;
using Microsoft.Extensions.Options;

namespace HI.Hubstaff
{
    public class HubstaffService : IHubstaffService
    {
        private readonly HubstaffSettings _settings;

        public HubstaffService(HubstaffSettings settings)
        {
            _settings = settings;
        }

        public async Task<HubstaffAuthModel> GenAuth()
        {
            var url = $"{_settings.BaseUrl}/auth";
            var result = await url.WithHeader("app-token", _settings.AppToken)
                .PostUrlEncodedAsync(new
                {
                    email = _settings.Email,
                    password = _settings.Password
                }).ReceiveJson<HubstaffAuthModel>();
            HubstaffSettings.Current.AuthToken = result.User.AuthToken;
            return result;
        }
    }
}