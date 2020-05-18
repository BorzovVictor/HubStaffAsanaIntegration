using System;
using System.Threading;
using System.Threading.Tasks;
using HI.Api.UseCases;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HI.Api.Services
{
    public class BackgroundService: IHostedService, IDisposable
    {
        private Timer _updateSumHoursFieldTimer;
        private ILogger<BackgroundService> _logger;
        private IUpdateSumFieldsCase _sumFieldsCase;

        private static readonly Mutex UpdateSumHoursFieldMutex = new Mutex();

        public BackgroundService(ILogger<BackgroundService> logger, IUpdateSumFieldsCase sumFieldsCase)
        {
            _logger = logger;
            _sumFieldsCase = sumFieldsCase;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // todo change hours execute value - to get from configurations
            _updateSumHoursFieldTimer = new Timer(UpdateSumHoursFieldWork, null, TimeSpan.Zero, TimeSpan.FromHours(2));
            
            return Task.CompletedTask;
        }
        
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _updateSumHoursFieldTimer?.Change(Timeout.Infinite, 0);
            
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
                try
                {
                    // todo exec update sumHours field case
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "error occurs in background service while update sum_hours field");
                    Thread.Sleep(1000);
                }
            }
            finally
            {
                UpdateSumHoursFieldMutex.ReleaseMutex();
            }
        }
    }
}