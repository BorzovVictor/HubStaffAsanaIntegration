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
            if (_settings.AuthToken == null)
                _settings.AuthToken = "QonAICDuVYa8Kvg4_T0gqP8rWMqFPQBKygW011p3R1c";
            return new {App_token = _settings.AppToken, Auth_Token = _settings.AuthToken};
        }

        public async Task<List<HubstaffTaskModel>> Tasks(string projects = "", int offset = 0)
        {
            var url = _settings.BaseUrl.AppendPathSegment("tasks");

            if (!string.IsNullOrWhiteSpace(projects))
                url = url.SetQueryParam("projects", projects);
            if (offset > 0)
                url = url.SetQueryParam("offset", offset);

            
            var result = await url
                .WithHeaders(GetHeaders())
                .GetJsonAsync<HubstaffTasksModel>();
            return result?.Tasks ?? new List<HubstaffTaskModel>();
        }
    }
}