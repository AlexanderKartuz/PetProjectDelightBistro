using System.ComponentModel.DataAnnotations;

namespace WebNet23Online.Models.Auth
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введите логин")]
        [MinLength(3, ErrorMessage = "Логин должен быть минимум 3 символа")]
        [MaxLength(50, ErrorMessage = "Логин не должен превышать 50 символов")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [MinLength(3, ErrorMessage = "Пароль должен быть минимум 3 символа")]
        [MaxLength(50, ErrorMessage = "Пароль не должен превышать 50 символов")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Подтвердите пароль")]
        [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Подтверждение пароля")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


        [MaxLength(50, ErrorMessage = "Фамилия не должна превышать 50 символов")]
        public string? FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "Имя не должно превышать 50 символов")]
        public string? LastName { get; set; }

        [Phone(ErrorMessage = "Неверный формат телефона")]
        [Display(Name = "Телефон")]
        public string? Mobilephone { get; set; }
    }
}
