using SFTPFileCheckerWithHostedService.Data;
using SFTPFileCheckerWithHostedService.Model;

namespace SFTPFileCheckerWithHostedService.Services
{
    public interface IFileHistoryService
    {
        Task<FileHistory> SaveFileHistory(FileHistory  fileHistory);
    }
    class FileHistoryService : IFileHistoryService
    {
        private IGenericRepository<FileHistory> _fileHistoryRepository;
        private ILogger<ConsumeServiceHostedService> _logger;
        public FileHistoryService(ILogger<ConsumeServiceHostedService> logger,IGenericRepository<FileHistory> fileHistoryRepository)
        {
            _fileHistoryRepository= fileHistoryRepository;
            _logger= logger;
        }
        public async Task<FileHistory> SaveFileHistory(FileHistory fileHistory)
        {
            _fileHistoryRepository.Insert(fileHistory);
            _fileHistoryRepository.Save();
            return fileHistory;
        }
    }
}
