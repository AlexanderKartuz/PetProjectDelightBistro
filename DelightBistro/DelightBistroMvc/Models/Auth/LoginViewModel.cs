using System.ComponentModel.DataAnnotations;

namespace DelightBistroMvc.Models.Auth
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите логин")]
        [MinLength(3, ErrorMessage = "Логин должен быть минимум 3 символа")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [MinLength(3, ErrorMessage = "Пароль должен быть минимум 3 символа")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
