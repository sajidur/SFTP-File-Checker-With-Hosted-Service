using Renci.SshNet;

namespace SFTPFileCheckerWithHostedService.Services
{
    public interface ISFTPService
    {
        string DownloadFiles();
    }
    public class SFTPService:ISFTPService
    {
        public SFTPService()
        {

        }
        public string DownloadFiles()
        {
            var client = new SftpClient("192.168.0.1", 80, "username", "password");
            client.Connect();
            using (Stream fileStream = File.OpenWrite("c:\\temp\\file.tmp"))
            {
                client.DownloadFile("", fileStream);
            }
            return "";
        }
    }
}
