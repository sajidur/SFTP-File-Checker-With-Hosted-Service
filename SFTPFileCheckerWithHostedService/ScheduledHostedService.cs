namespace SFTPFileCheckerWithHostedService
{
    public class ScheduledHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<ScheduledHostedService> _logger;
        private Timer? _timer = null;

        public ScheduledHostedService(ILogger<ScheduledHostedService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Schedule Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            _logger.LogInformation(
      "Consume Scoped Service Hosted Service is working.");

            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<IProcessingService>();

                await scopedProcessingService.DoWork(stoppingToken);
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Schedule Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
