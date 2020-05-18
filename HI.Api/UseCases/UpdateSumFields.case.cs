using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HI.Asana;
using HI.Hubstaff;
using HI.SharedKernel;
using HI.SharedKernel.Errors;
using HI.SharedKernel.Models;
using HI.SharedKernel.Models.Responses;

namespace HI.Api.UseCases
{
    public interface IUpdateSumFieldsCase
    {
        Task<UseCase<List<UpdateSumFieldsResult>>.ExecutionResult> Execute(DateTime start, DateTime end);
        Task<UseCase<List<UpdateSumFieldsResult>>.ExecutionResult> ExecuteNoUpdate(DateTime start, DateTime end);
    }

    public class UpdateSumFieldsCase : UseCase<List<UpdateSumFieldsResult>>, IUpdateSumFieldsCase
    {
        private readonly IAsanaService _asanaService;
        private readonly IHubstaffService _hubstaffService;

        public UpdateSumFieldsCase(IAsanaService asanaService, IHubstaffService hubstaffService)
        {
            _asanaService = asanaService;
            _hubstaffService = hubstaffService;
        }

        public async Task<ExecutionResult> Execute(DateTime start, DateTime end)
        {
            var asanaUpdatedTasks = new List<AsanaTaskModel>();
            var asanaUpdatedTasksTest = new List<UpdateSumFieldsResult>();
            // get all tasks by period from hubstaff
            var hubReq = new HsTeamMemberRequest
            {
                StartDate = DateTime.Today.AddDays(-1),
                EndDate = DateTime.Today,
                ShowTasks = true,
                ShowActivity = true
            };
            var hubTasks = await _hubstaffService.GetTasksDurations(hubReq);
            foreach (var hubTask in hubTasks.Where(hubTask =>
                (!string.IsNullOrWhiteSpace(hubTask.RemoteId) || hubTask.Duration.HasValue) &&
                hubTask.Duration.Value != 0))
            {
                // update asana task
                var asanaTask = await _asanaService.UpdateSumFieldTaskTest(hubTask.RemoteId, hubTask.Duration.Value);
                if(asanaTask!=null)
                    asanaUpdatedTasksTest.Add(asanaTask);
            }

            return asanaUpdatedTasks.Any()
                ? Success(asanaUpdatedTasksTest)
                : Failure(new UpdateSumFieldsError("tasks not found", 502));
        }
        
        public async Task<ExecutionResult> ExecuteNoUpdate(DateTime start, DateTime end)
        {
            var asanaUpdatedTasksTest = new List<UpdateSumFieldsResult>();
            // get all tasks by period from hubstaff
            var hubReq = new HsTeamMemberRequest
            {
                StartDate = DateTime.Today.AddDays(-1),
                EndDate = DateTime.Today,
                ShowTasks = true,
                ShowActivity = true
            };
            var hubTasks = await _hubstaffService.GetTasksDurations(hubReq);
            foreach (var hubTask in hubTasks.Where(hubTask =>
                (!string.IsNullOrWhiteSpace(hubTask.RemoteId) || hubTask.Duration.HasValue) &&
                hubTask.Duration.Value != 0))
            {
                // update asana task
                var asanaTask = await _asanaService.UpdateSumFieldTaskTest(hubTask.RemoteId, hubTask.Duration.Value);
                if(asanaTask!=null)
                    asanaUpdatedTasksTest.Add(asanaTask);
            }
        
            return asanaUpdatedTasksTest.Any()
                ? Success(asanaUpdatedTasksTest)
                : Failure(new UpdateSumFieldsError("tasks not found", 502));
        }
    }
}