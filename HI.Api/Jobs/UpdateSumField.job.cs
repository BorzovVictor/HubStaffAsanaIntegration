using System;
using System.Threading;
using System.Threading.Tasks;
using CronScheduler.Extensions.Scheduler;
using HI.Api.UseCases;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HI.Api.Jobs
{
    public class UpdateSumFieldJob: IScheduledJob
    {
        private UpdateSumFieldJobOptions _options;
        private readonly IServiceProvider _provider;
        private readonly ILogger _logger;
        public string Name { get; } = nameof(UpdateSumFieldJob);

        public UpdateSumFieldJob(IOptionsMonitor<UpdateSumFieldJobOptions> options, 
            IServiceProvider provider, ILogger<UpdateSumFieldJob> logger)
        {
            _options = options.Get(Name);
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _logger = logger;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using var scope = _provider.CreateScope();
            try
            {
                var updateService = scope.ServiceProvider.GetRequiredService<IUpdateSumFieldsCase>();

                var startDate = DateTime.Today.AddDays(-1);
                var endDate = startDate;

                var result = await updateService.ExecuteNoSave(startDate, endDate);
                if (result.Succeded)
                {
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
}