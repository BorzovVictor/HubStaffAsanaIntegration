using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using HI.SharedKernel.Models;

namespace HI.Hubstaff
{
    public class HubstaffService : IHubstaffService
    {
        private readonly HubstaffSettings _settings;
        private object _headers = null;

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
            _settings.AuthToken = result.User.AuthToken;
            return result;
        }

        private object GetHeaders()
        {
            return new {App_token = _settings.AppToken, Auth_Token = _settings.AuthToken};
        }
        
        public async Task<List<HubstaffTaskModel>> Tasks(string projects = "", int offset = 0)
        {
            var url = $"{_settings.BaseUrl}/tasks";
            var queryParams = new Dictionary<string, object>();
            if(!string.IsNullOrWhiteSpace(projects))
                queryParams.Add(nameof(projects), projects.Trim());
            if(offset>0)
                queryParams.Add(nameof(offset), offset);

            if (queryParams.Any())
                url.SetQueryParams(queryParams);
            
            var result = await url
                .WithHeaders(GetHeaders())
                .GetJsonAsync();
            return result?.Tasks ?? new List<HubstaffTaskModel>();
        }
    }
}