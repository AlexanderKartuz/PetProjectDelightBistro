using System.ComponentModel.DataAnnotations;
using WebNet23Online.Models.AnimeGirl;

namespace WebNet23Online.Models.CustomValidatioAttributes
{
    public class DoubleCheckAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var viewModel = validationContext.ObjectInstance as CreateAnimeGirlViewModel;
            if (viewModel is null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            if (viewModel.Name == viewModel.DoubleCheckName)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(Localizations.AnimeGirl.Validation_NamesMustMatch);
        }
    }
}
