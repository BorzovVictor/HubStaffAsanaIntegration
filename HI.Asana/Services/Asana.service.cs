using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using HI.SharedKernel.Models;
using HI.SharedKernel.Models.Asana;

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

        public async Task<HistoryData> UpdateSumFieldTask(HsTeamMemberTask hubTask)
        {
            if ((string.IsNullOrWhiteSpace(hubTask.RemoteId) || !hubTask.Duration.HasValue) ||
                hubTask.Duration.Value == 0)
                return null;
            
            var url = $"{_settings.BaseUrl}/tasks/{hubTask}";
            var task = await GetById(hubTask.RemoteId);
            var sumHoursField = task?.AsanaTaskData.CustomFields
                .FirstOrDefault(x => x.Name == "sum hours");
            if (sumHoursField == null) return null;

            var result = FillHistory(hubTask, task);
            
            sumHoursField.NumberValue ??= 0;
            sumHoursField.NumberValue = ((decimal) hubTask.Duration / (decimal) 3600);
            
            var sumHoursValue = new Dictionary<string, string> {{sumHoursField.Gid, sumHoursField.NumberValue.Value.ToString("F1")}};
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