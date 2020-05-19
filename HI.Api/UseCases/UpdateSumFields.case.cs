using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HI.Api.Services;
using HI.Asana;
using HI.Hubstaff;
using HI.SharedKernel;
using HI.SharedKernel.Errors;
using HI.SharedKernel.Models;
using HI.SharedKernel.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HI.Api.UseCases
{
    public interface IUpdateSumFieldsCase
    {
        Task<UseCase<List<HistoryData>>.ExecutionResult> Execute(DateTime start, DateTime end);
        Task<UseCase<List<HistoryData>>.ExecutionResult> ExecuteNoUpdate(DateTime start, DateTime end);
    }

    public class UpdateSumFieldsCase : UseCase<List<HistoryData>>, IUpdateSumFieldsCase
    {
        private readonly IAsanaService _asanaService;
        private readonly IHubstaffService _hubstaffService;
        private readonly AppDbContext _context;
        private ILogger<UpdateSumFieldsCase> _logger;

        public UpdateSumFieldsCase(IAsanaService asanaService, IHubstaffService hubstaffService, AppDbContext context,
            ILogger<UpdateSumFieldsCase> logger)
        {
            _asanaService = asanaService;
            _hubstaffService = hubstaffService;
            _context = context;
            _logger = logger;
        }

        public async Task<ExecutionResult> Execute(DateTime start, DateTime end)
        {
            var asanaUpdatedTasks = new List<AsanaTaskModel>();
            var asanaUpdatedTasksTest = new List<HistoryData>();
            // get all tasks by period from hubstaff
            var hubReq = new HsTeamMemberRequest
            {
                StartDate = start,
                EndDate = end,
                ShowTasks = true,
                ShowActivity = true
            };
            var hubTasks = await _hubstaffService.GetTasksDurations(hubReq);
            foreach (var hubTask in hubTasks.Where(hubTask =>
                (!string.IsNullOrWhiteSpace(hubTask.RemoteId) || hubTask.Duration.HasValue) &&
                hubTask.Duration.Value != 0))
            {
                // update asana task
                var asanaTask = await _asanaService.UpdateSumFieldTaskTest(hubTask);
                if (asanaTask != null)
                    asanaUpdatedTasksTest.Add(asanaTask);
            }

            foreach (var task in asanaUpdatedTasksTest)
            {
                var dbTask = await _context.Histories.FirstOrDefaultAsync(x => x.HubId == task.HubId);
                if (dbTask != null && task.Duration.HasValue && task.Duration.Value == dbTask.Duration)
                    continue; // no need to update
                if (dbTask != null)
                {
                    dbTask.Duration = task.Duration;
                    _context.Histories.Update(dbTask);
                }
                else
                {
                    await _context.Histories.AddAsync(task);
                }
            }

            await _context.SaveChangesAsync();

            return asanaUpdatedTasks.Any()
                ? Success(asanaUpdatedTasksTest)
                : Failure(new UpdateSumFieldsError("tasks not found", 502));
        }

        public async Task<ExecutionResult> ExecuteNoUpdate(DateTime start, DateTime end)
        {
            int updated = 0;
            int inserted = 0;
            int error = 0;
            var asanaUpdatedTasksTest = new List<HistoryData>();
            // get all tasks by period from hubstaff
            var hubReq = new HsTeamMemberRequest
            {
                StartDate = start,
                EndDate = end,
                ShowTasks = true,
                ShowActivity = false
            };
            var hubTasks = await _hubstaffService.GetTasksDurations(hubReq);
            foreach (var hubTask in hubTasks.Where(hubTask =>
                (!string.IsNullOrWhiteSpace(hubTask.RemoteId) || hubTask.Duration.HasValue) &&
                hubTask.Duration.Value != 0))
            {
                var dbTask = await _context.Histories.FirstOrDefaultAsync(x => x.HubId == hubTask.Id);
                if (dbTask != null && hubTask.Duration.HasValue && hubTask.Duration.Value == dbTask.Duration)
                    continue; // no need to update

                // update asana task
                try
                {
                    var asanaTask = await _asanaService.UpdateSumFieldTaskTest(hubTask);
                    if (asanaTask != null)
                    {
                        asanaUpdatedTasksTest.Add(asanaTask);
                        if (dbTask != null)
                        {
                            dbTask.Duration = asanaTask.Duration;
                            dbTask.LastUpdate = DateTime.UtcNow;
                            _context.Histories.Update(dbTask);
                            updated++;
                        }
                        else
                        {
                            await _context.Histories.AddAsync(asanaTask);
                            inserted++;
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e.GetBaseException().Message);
                    error++;
                }
            }

            if (asanaUpdatedTasksTest.Any())
                await _context.SaveChangesAsync();

            _logger.LogInformation($"inserted: {inserted}, updated: {updated}, error: {error}");

            return asanaUpdatedTasksTest.Any()
                ? Success(asanaUpdatedTasksTest)
                : Failure(new UpdateSumFieldsError("tasks not found", 502));
        }
    }
}