using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace WebNet23Online.Models.CustomValidatioAttributes
{
    public class CheckForEnglishLettersAttribute : ValidationAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return string.IsNullOrEmpty(ErrorMessage)
               ? "Возможно написание только латиницей"
               : ErrorMessage;
        }
        public override bool IsValid(object? value)
        {
            if (value is not string)
            {
                throw new Exception("Вы ввели некорректные данные, введите текст");
            }
            var textValue = (string)value;
            if (textValue.IsNullOrEmpty())
            {

            }
            return Regex.IsMatch(textValue, @"^[a-zA-Z0-9\-\s]+$");
        }
    }
}