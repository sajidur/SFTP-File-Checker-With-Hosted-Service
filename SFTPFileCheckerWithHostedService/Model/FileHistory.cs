using System.ComponentModel.DataAnnotations;

namespace SFTPFileCheckerWithHostedService.Model
{
    public class FileHistory
    {
        [Key]
        public int Id { get; set; }
        public string FileName { get; internal set; }
        public DateTime CreatedDate { get; internal set; }
    }
}
