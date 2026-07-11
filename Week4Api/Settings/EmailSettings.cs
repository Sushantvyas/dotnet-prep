using System.ComponentModel.DataAnnotations;

namespace Week4Api.Settings
{
    public class EmailSettings
    {
        [Required]
        public string SmtpHost { get; set; } = string.Empty;
        public int Port { get; set; } = 587;
        public bool UseSsl { get; set; } = true;
        public string Username { get; set; } = string.Empty;
    }
}
