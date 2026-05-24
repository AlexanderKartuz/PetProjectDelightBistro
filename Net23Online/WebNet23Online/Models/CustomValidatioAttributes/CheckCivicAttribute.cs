using System.ComponentModel.DataAnnotations;

namespace WebNet23Online.Models.CustomValidatioAttributes
{
    public class CheckCivicAttribute : ValidationAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return string.IsNullOrEmpty(ErrorMessage)
                ? "Никаких цивиков на моем сайте."
                : ErrorMessage;
        }
        public override bool IsValid(object? value)
        {
            if (value is not string)
            {
                throw new Exception("Вы ввели некорректные данные, введите текст");
            }
            var model = (string)value;
            return model != "Civic" && model != "civic";
        }
    }
}