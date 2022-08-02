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
        public SFTPService(ILogger<SFTPService> logger, IFileHistoryService historyService)
        {
            _logger = logger;
            _historyService = historyService;
        }
        public void DownloadFiles()
        {
            try
            {
                var client = new SftpClient("test.rebex.net", "demo", "password");
                client.Connect();
                var list=client.ListDirectory("/pub/example/");
                foreach (var file in list)
                {
                    var filePath= @"E:\SFTP-File-Checker-With-Hosted-Service\" + DateTime.Now.Millisecond.ToString() + "_" + Path.GetFileName(file.FullName);
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
