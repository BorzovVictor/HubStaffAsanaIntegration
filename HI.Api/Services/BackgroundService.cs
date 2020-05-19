using System;
using System.Threading;
using System.Threading.Tasks;
using HI.Api.UseCases;
using HI.Hubstaff;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HI.Api.Services
{
    public class BackgroundService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _services;
        private Timer _updateSumHoursFieldTimer;
        private readonly ILogger _logger;
        private readonly HubstaffSettings _settings;

        private static readonly Mutex UpdateSumHoursFieldMutex = new Mutex();

        public BackgroundService(ILogger<BackgroundService> logger, IServiceProvider services,
            HubstaffSettings settings)
        {
            _logger = logger;
            _services = services;
            _settings = settings;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Background Service is starting.");
            
            _updateSumHoursFieldTimer = new Timer(UpdateSumHoursFieldWork, null, TimeSpan.Zero,
                TimeSpan.FromHours(_settings.HoursBetweenCheck));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _updateSumHoursFieldTimer?.Change(Timeout.Infinite, 0);
            _logger.LogInformation("Background Service is stopping.");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _updateSumHoursFieldTimer?.Dispose();
        }


        private void UpdateSumHoursFieldWork(object state)
        {
            UpdateSumHoursFieldMutex.WaitOne();
            try
            {
                using (var scope = _services.CreateScope())
                {
                    try
                    {
                        var updateService = scope.ServiceProvider.GetRequiredService<IUpdateSumFieldsCase>();
                        var storeService = scope.ServiceProvider.GetRequiredService<IJsonStoreService>();

                        var startDate = DateTime.Today;
                        var endDate = DateTime.Today.AddDays(1);
                        // todo change to real
                        var result = updateService.Execute(startDate, endDate).Result;
                        if (result.Succeded)
                        {
                            storeService.AddRange(result.Success);
                            _logger.LogInformation($"sumFields checked. at: {DateTimeOffset.Now}");   
                        }
                        else
                        {
                            _logger.LogError(result.Failure.Message);
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "error occurs in background service while update sum_hours field");
                        Thread.Sleep(1000);
                    }
                }
            }
            finally
            {
                UpdateSumHoursFieldMutex.ReleaseMutex();
            }
        }
    }
}