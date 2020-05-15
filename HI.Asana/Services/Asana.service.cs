using System;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using HI.SharedKernel.Models;
using Microsoft.Extensions.Options;

namespace HI.Asana
{
    public class AsanaService : IAsanaService
    {
        private readonly AsanaSettings _settings;

        public AsanaService(AsanaSettings settings)
        {
            _settings = settings;
        }

        public async Task<AsanaTaskModel> GetById(string taskId)
        {
            var url = $"{_settings.BaseUrl}/tasks/{taskId}";
            return await url.WithOAuthBearerToken(_settings.Token)
                .GetJsonAsync<AsanaTaskModel>();
        }

        public async Task<AsanaTaskModel> UpdateSumFieldTask(string taskId, long sumHours)
        {
            var url = $"{_settings.BaseUrl}/tasks/{taskId}";
            var task = await GetById(taskId);
            if (task != null)
            {
                var sumHoursField = task.AsanaTaskData
                    .CustomFields
                    .FirstOrDefault(x => x.Name == "sum hours");
                if (sumHoursField != null)
                    sumHoursField.NumberValue = sumHours;
            }

            throw new NotImplementedException();
        }
    }
}