using Renci.SshNet;
using SFTPFileCheckerWithHostedService.Model;

namespace SFTPFileCheckerWithHostedService.Services
{
    public interface ISFTPService
    {
       void DownloadFiles();
    }
    class SFTPService:ISFTPService
    {
        private ILogger<SFTPService> _logger;
        private IFileHistoryService _historyService;
        private readonly IConfiguration _configuration;

        public SFTPService(ILogger<SFTPService> logger, IFileHistoryService historyService, IConfiguration configuration)
        {
            _logger = logger;
            _historyService = historyService;
            _configuration = configuration;
        }
        public void DownloadFiles()
        {
            try
            {
                //if we need avoid already saved file will not pick again. we will use this list to check.
                var files = _historyService.GetAllFileHistory().Result.ToList();

                var client = new SftpClient(_configuration["SFTPServer:server"], _configuration["SFTPServer:username"], _configuration["SFTPServer:password"]);
                client.Connect();
                var list=client.ListDirectory(_configuration["SFTPServer:path"]);
                foreach (var file in list)
                {
                    var fileName = Path.GetFileName(file.FullName);
                    if (fileName.Length < 3)
                    {
                        continue;
                    }
                    var filePath= _configuration["LocalServer:path"] + DateTime.Now.Millisecond.ToString() + "_" + fileName;
                    using (Stream fileStream = File.OpenWrite(filePath))
                    {
                        client.DownloadFile(file.FullName, fileStream);
                    }
                    //save history in database
                    FileHistory fileHistory = new FileHistory();
                    fileHistory.FileName = filePath;
                    fileHistory.CreatedDate = DateTime.Now;

                    var res=_historyService.SaveFileHistory(fileHistory).IsCompletedSuccessfully;
                    if (res)
                    {
                        _logger.LogInformation(file + " saved sucessfully");
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message + " |"+ex.InnerException);
                throw ex;
            }

        }
    }
}
