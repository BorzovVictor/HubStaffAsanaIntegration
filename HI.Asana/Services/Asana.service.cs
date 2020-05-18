using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using HI.SharedKernel.Models;
using HI.SharedKernel.Models.Asana;
using HI.SharedKernel.Models.Responses;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

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

        public async Task<UpdateSumFieldsResult> UpdateSumFieldTask(string taskId, long sumSeconds)
        {
            var result = new UpdateSumFieldsResult();
            var url = $"{_settings.BaseUrl}/tasks/{taskId}";
            var task = await GetById(taskId);
            var sumHoursField = task?.AsanaTaskData.CustomFields
                .FirstOrDefault(x => x.Name == "sum hours");
            if (sumHoursField == null) return null;

            result.AsanaTaskId = task.AsanaTaskData.Gid;
            sumHoursField.NumberValue ??= 0;
            result.SumHoursPreviousValue = sumHoursField.NumberValue.Value;
            result.HubstaffSumHoursValue = ((decimal) sumSeconds / (decimal) 3600);
            sumHoursField.NumberValue += result.HubstaffSumHoursValue;
            result.SumHoursNewValue = sumHoursField.NumberValue;
            
            
            var sumHoursValue = new Dictionary<string, string> {{sumHoursField.Gid, result.SumHoursNewValue.Value.ToString("F1")}};
            var updateModel = new AsanaTaskUpdateModel
            {
                Data = new DataCustomFields
                {
                    CustomFields = sumHoursValue
                }
            };
            
            await url.WithOAuthBearerToken(_settings.Token).PutJsonAsync(updateModel);
            
            result.UpdateAt = DateTime.UtcNow;
            return result;
        }
        
        public async Task<UpdateSumFieldsResult> UpdateSumFieldTaskTest(string taskId, long sumSeconds)
        {
            var result = new UpdateSumFieldsResult();
            var task = await GetById(taskId);
            var sumHoursField = task?.AsanaTaskData.CustomFields
                .FirstOrDefault(x => x.Name == "sum hours");
            if (sumHoursField == null) return null;

            result.AsanaTaskId = task.AsanaTaskData.Gid;
            sumHoursField.NumberValue ??= 0;
            result.SumHoursPreviousValue = sumHoursField.NumberValue.Value;
            result.HubstaffSumHoursValue = ((decimal) sumSeconds / (decimal) 3600);
            sumHoursField.NumberValue += result.HubstaffSumHoursValue;
            result.SumHoursNewValue = sumHoursField.NumberValue;
            result.UpdateAt = DateTime.UtcNow;
            return result;
        }
    }
}