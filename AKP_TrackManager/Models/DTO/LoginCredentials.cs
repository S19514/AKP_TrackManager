using System.ComponentModel.DataAnnotations;

namespace AKP_TrackManager.Models.DTO
{
    public class LoginCredentials
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
