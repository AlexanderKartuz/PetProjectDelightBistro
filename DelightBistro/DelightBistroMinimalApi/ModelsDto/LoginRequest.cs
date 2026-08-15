using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace DelightBistroMinimalApi.ModelsDto
{
    public class LoginRequest
    {
        [Required]
        public string Login { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
