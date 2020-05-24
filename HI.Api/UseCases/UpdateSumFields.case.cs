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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HI.Api.UseCases
{
    public interface IUpdateSumFieldsCase
    {
        Task<UseCase<List<HistoryData>>.ExecutionResult> Execute(DateTime start, DateTime end);
        Task<UseCase<List<HistoryData>>.ExecutionResult> ExecuteNoUpdate(DateTime start, DateTime end);
        Task<UseCase<List<HistoryData>>.ExecutionResult> ExecuteNoSave(DateTime start, DateTime end);
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
            throw new NotImplementedException();
            int updated = 0;
            int inserted = 0;
            int error = 0;
            var asanaUpdatedTasks = new List<HistoryData>();
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
                if (dbTask != null && hubTask.Duration.HasValue && hubTask.Duration.Value == dbTask.Duration &&
                    dbTask.LastUpdate.Date == DateTime.UtcNow.Date)
                    continue; // no need to update

                // update asana task
                try
                {
                    long newDuration = hubTask.Duration.Value;
                    if (dbTask != null)
                    {
                        // if a new day we added time
                        if (dbTask.LastUpdate.Date < DateTime.UtcNow.Date)
                        {
                            dbTask.Duration = hubTask.Duration;
                            dbTask.YesterdayDuration = dbTask.TotalDuration;
                            dbTask.TotalDuration += dbTask.Duration.Value;
                        }
                        else // else we set new
                        {
                            dbTask.Duration = hubTask.Duration;
                            dbTask.TotalDuration = dbTask.YesterdayDuration + dbTask.Duration.Value;
                        }

                        newDuration = dbTask.TotalDuration;
                    }
                    
                    var asanaTask = await _asanaService.UpdateSumFieldTask(hubTask, newDuration);
                    
                    if (asanaTask != null)
                    {
                        asanaUpdatedTasks.Add(asanaTask);
                        if (dbTask != null)
                        {
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

            if (asanaUpdatedTasks.Any())
                await _context.SaveChangesAsync();

            _logger.LogInformation($"inserted: {inserted}, updated: {updated}, error: {error}");

            return asanaUpdatedTasks.Any()
                ? Success(asanaUpdatedTasks)
                : Failure(new UpdateSumFieldsError("tasks not found", 502));
        }

        public async Task<ExecutionResult> ExecuteNoSave(DateTime start, DateTime end)
        {
            int updated = 0;
            int error = 0;
            var asanaUpdatedTasks = new List<HistoryData>();
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
                // update asana task
                try
                {
                    var asanaTask = await _asanaService.UpdateSumField(hubTask);
                    
                    if (asanaTask != null)
                    {
                        asanaUpdatedTasks.Add(asanaTask);
                        updated++;
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e.GetBaseException().Message);
                    error++;
                }
            }

            _logger.LogInformation($"updated: {updated}, error: {error}");

            return asanaUpdatedTasks.Any()
                ? Success(asanaUpdatedTasks)
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