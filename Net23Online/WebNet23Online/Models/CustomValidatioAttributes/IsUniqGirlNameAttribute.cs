using System.ComponentModel.DataAnnotations;
using WebNet23Online.Data.Repositories.Interfaces;

namespace WebNet23Online.Models.CustomValidatioAttributes
{
    public class IsUniqGirlNameAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var name = value as string;

            var repository = validationContext.GetRequiredService<IAnimeGirlRepository>();
            if (!repository.IsNameFree(name))
            {
                return new ValidationResult(Localizations.AnimeGirl.Validation_NameAlreadyUsed);
            }

            return ValidationResult.Success;
        }
    }
}
