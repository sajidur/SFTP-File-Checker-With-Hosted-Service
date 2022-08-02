using SFTPFileCheckerWithHostedService.Services;

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
        private ISFTPService _sFTPService;
        private readonly IConfiguration _configuration;

        public ProcessingService(ILogger<ProcessingService> logger, ISFTPService sFTPService, IConfiguration configuration)
        {
            _logger = logger;
            _sFTPService = sFTPService;
            _configuration = configuration;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                executionCount++;

                _logger.LogInformation(
                    "Scoped Processing Service is working. Count: {Count}", executionCount);
                _sFTPService.DownloadFiles();
                await Task.Delay(Convert.ToInt32(_configuration["Schedule:timeInterVal"]), stoppingToken);
            }
        }
    }
}
