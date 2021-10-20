using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DTNL.LL.Logic.Analytics;
using DTNL.LL.Logic.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DTNL.LL.Logic.Workers
{
    public class LiveLightWorker : IHostedService, IDisposable
    {
        private readonly int _tickDelayInSeconds;


        private Timer _timer;

        //Todo: Add Logging in case request errors
        private readonly ILogger<LiveLightWorker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        
        public LiveLightWorker(ILogger<LiveLightWorker> logger, IServiceScopeFactory scopeFactory, IOptions<ServiceWorkerOptions> options)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _tickDelayInSeconds = options.Value.TickTimeInSeconds;
        }

        private async void ProcessLiveLights(object _)
        {
            using var scope = _scopeFactory.CreateScope();
            var liveLightService = scope.ServiceProvider.GetRequiredService<LiveLightService>();

            await liveLightService.ProcessLiveLights();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(ProcessLiveLights, null, TimeSpan.Zero, TimeSpan.FromSeconds(_tickDelayInSeconds));

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