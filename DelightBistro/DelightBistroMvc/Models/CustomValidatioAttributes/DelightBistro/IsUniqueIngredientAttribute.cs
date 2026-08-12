using System.ComponentModel.DataAnnotations;
using DelightBistroMvc.Data.Repositories.Interfaces.DelightBistro;

namespace DelightBistroMvc.Models.CustomValidatioAttributes.DelightBistro
{
    public class IsUniqueIngredientAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is not string)
            {
                return new ValidationResult("Use letters");
            }

            var name = value as string;

            var repository=validationContext.GetRequiredService<IIngredientsRepository>();
            if (!repository.IsNameFree(name))
            {
                return new ValidationResult("Name is already used");

            }
            return ValidationResult.Success;
        }
    }
}
