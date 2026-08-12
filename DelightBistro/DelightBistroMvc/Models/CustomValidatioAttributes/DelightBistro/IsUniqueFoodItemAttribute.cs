using System.ComponentModel.DataAnnotations;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;
using DelightBistroMvc.Models.DelightBistro;

namespace DelightBistroMvc.Models.CustomValidatioAttributes.DelightBistro
{
    public class IsUniqueFoodItemAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            //Нужен ли FormatErrorMessage
            if (value is not string)
            {
                return new ValidationResult("Use letters");
            }

            if (validationContext.ObjectInstance is CreateFoodItemViewModel viewModel && viewModel.Id > 0)
            {
                return ValidationResult.Success;
            }

            var name = value as string;

            var repository = validationContext.GetRequiredService<IFoodItemRepository>();
            if (!repository.IsNameFree(name))
            {
                return new ValidationResult("Name is already used");

            }
            return ValidationResult.Success;
        }
    }
}
