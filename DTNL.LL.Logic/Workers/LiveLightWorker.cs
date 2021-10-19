using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DTNL.LL.Logic.Analytics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DTNL.LL.Logic.Workers
{
    public class LiveLightWorker : IHostedService, IDisposable
    {
        //Todo: Make config value
        public const int TickDelayInSeconds = 60;


        private Timer _timer;

        private readonly ILogger<LiveLightWorker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        
        public LiveLightWorker(ILogger<LiveLightWorker> logger, IServiceScopeFactory scopeFactory, V3Analytics v3, V4Analytics v4)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        private async void ProcessLiveLights(object _)
        {
            using var scope = _scopeFactory.CreateScope();
            var liveLightService = scope.ServiceProvider.GetRequiredService<LiveLightService>();
            await liveLightService.ProcessLiveLights();

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(ProcessLiveLights, null, TimeSpan.Zero, TimeSpan.FromSeconds(TickDelayInSeconds));

            return Task.CompletedTask;
        }
        
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}