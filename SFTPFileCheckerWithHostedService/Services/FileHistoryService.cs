using SFTPFileCheckerWithHostedService.Data;
using SFTPFileCheckerWithHostedService.Model;
using System.Collections.Generic;

namespace SFTPFileCheckerWithHostedService.Services
{
    public interface IFileHistoryService
    {
        Task<FileHistory> SaveFileHistory(FileHistory  fileHistory);
        Task<IEnumerable<FileHistory>> GetAllFileHistory();

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

        public Task<IEnumerable<FileHistory>> GetAllFileHistory()
        {
            return _fileHistoryRepository.GetAll();
        }

        public async Task<FileHistory> SaveFileHistory(FileHistory fileHistory)
        {
            _fileHistoryRepository.Insert(fileHistory);
            _fileHistoryRepository.Save();
            return fileHistory;
        }
    }
}
