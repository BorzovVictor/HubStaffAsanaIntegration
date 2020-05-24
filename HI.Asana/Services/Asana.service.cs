using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using HI.SharedKernel.Models;
using HI.SharedKernel.Models.Asana;
using Microsoft.Extensions.Logging;

namespace HI.Asana
{
    public class AsanaService : IAsanaService
    {
        private readonly AsanaSettings _settings;
        private readonly ILogger _logger;

        public AsanaService(AsanaSettings settings, ILogger<AsanaService> logger)
        {
            _settings = settings;
            _logger = logger;
        }

        public async Task<AsanaTaskModel> GetById(string taskId)
        {
            var url = $"{_settings.BaseUrl}/tasks/{taskId}";
            return await url.WithOAuthBearerToken(_settings.Token)
                .GetJsonAsync<AsanaTaskModel>();
        }

        public async Task<HistoryData> UpdateSumFieldTask(HsTeamMemberTask hubTask, long duration)
        {
            if ((string.IsNullOrWhiteSpace(hubTask.RemoteId) || duration == 0))
                return null;

            var url = $"{_settings.BaseUrl}/tasks/{hubTask.RemoteId}";
            var task = await GetById(hubTask.RemoteId);
            var sumHoursField = task?.AsanaTaskData.CustomFields
                .FirstOrDefault(x => x.Name == "sum hours");
            if (sumHoursField == null) return null;

            var result = FillHistory(hubTask, task);

            sumHoursField.NumberValue ??= 0;
            sumHoursField.NumberValue = ((decimal) duration / (decimal) 3600);

            var sumHoursValue = new Dictionary<string, string>
                {{sumHoursField.Gid, sumHoursField.NumberValue.Value.ToString("F1")}};
            var updateModel = new AsanaTaskUpdateModel
            {
                Data = new DataCustomFields
                {
                    CustomFields = sumHoursValue
                }
            };

            await url.WithOAuthBearerToken(_settings.Token).PutJsonAsync(updateModel);

            return result;
        }

        public async Task<HistoryData> UpdateSumField(HsTeamMemberTask hubTask)
        {
            if (string.IsNullOrWhiteSpace(hubTask.RemoteId))
                return null;

            var url = $"{_settings.BaseUrl}/tasks/{hubTask.RemoteId}";
            // get asana task by id
            var task = await GetById(hubTask.RemoteId);
            // get sum hours field from task
            var sumHoursField = task?.AsanaTaskData.CustomFields
                .FirstOrDefault(x => x.Name == "sum hours");
            if (sumHoursField == null) return null;

            var result = FillHistory(hubTask, task);

            sumHoursField.NumberValue ??= 0; // if sum hours hasn't value set to 0

            var newTime = ((decimal) hubTask.Duration.Value / (decimal) 3600);
            _logger.LogInformation(
                $"taskid: {hubTask.RemoteId}, duration: {hubTask.Duration}, sum hours: {sumHoursField.NumberValue}");
            
            // increase sum hours on hubstaff duration
            sumHoursField.NumberValue += newTime;

            var sumHoursValue = new Dictionary<string, string>
                {{sumHoursField.Gid, sumHoursField.NumberValue.Value.ToString("F1")}};
            var updateModel = new AsanaTaskUpdateModel
            {
                Data = new DataCustomFields
                {
                    CustomFields = sumHoursValue
                }
            };

            await url.WithOAuthBearerToken(_settings.Token).PutJsonAsync(updateModel);

            return result;
        }

        public async Task<HistoryData> UpdateSumFieldTaskTest(HsTeamMemberTask hubTask)
        {
            if ((string.IsNullOrWhiteSpace(hubTask.RemoteId) || !hubTask.Duration.HasValue) ||
                hubTask.Duration.Value == 0)
                return null;

            var task = await GetById(hubTask.RemoteId);

            var sumHoursField = task?.AsanaTaskData.CustomFields
                .FirstOrDefault(x => x.Name == "sum hours");
            if (sumHoursField == null) return null;

            var history = FillHistory(hubTask, task);

            return history;
        }

        private HistoryData FillHistory(HsTeamMemberTask hubTask, AsanaTaskModel task)
        {
            return new HistoryData
            {
                HubId = hubTask.Id,
                RemoteId = hubTask.RemoteId,
                Summary = hubTask.Summary,
                Duration = hubTask.Duration,

                AsanaId = task.AsanaTaskData?.Gid,
                Name = task.AsanaTaskData?.Name,
                AssigneeStatus = task.AsanaTaskData?.AssigneeStatus,
                Completed = task.AsanaTaskData?.Completed,
                CompletedAt = task.AsanaTaskData?.CompletedAt,
                CreatedAt = task.AsanaTaskData?.CreatedAt
            };
        }
    }
}