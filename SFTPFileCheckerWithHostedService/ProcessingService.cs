namespace SFTPFileCheckerWithHostedService
{
    public interface IProcessingService
    {
        Task DoWork(CancellationToken stoppingToken);
    }
    internal class ProcessingService : IProcessingService
    {
        private int executionCount = 0;
        private readonly ILogger _logger;

        public ProcessingService(ILogger<ProcessingService> logger)
        {
            _logger = logger;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                executionCount++;

                _logger.LogInformation(
                    "Scoped Processing Service is working. Count: {Count}", executionCount);

                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
