using System.Threading.Tasks;
using Flurl.Http;
using HI.SharedKernel.Models;
using Microsoft.Extensions.Options;

namespace HI.Asana
{
    public class AsanaService: IAsanaService
    {
        private readonly AsanaSettings _settings;

        public AsanaService(IOptions<AsanaSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<AsanaTaskModel> GetById(string taskId)
        {
            var url = $"{_settings.BaseUrl}/tasks/{taskId}";
            return await url.WithOAuthBearerToken(_settings.Token)
                .GetJsonAsync<AsanaTaskModel>();
        }
    }
}