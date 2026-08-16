using System.ComponentModel.DataAnnotations;

namespace DelightBistroMinimalApi.Validation
{
    public class EndpointValidator : IEndpointValidator
    {
        public IResult? Validate(object model)
        {
            var context = new ValidationContext(model);
            var result = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(
                model,
                context,
                result,
                validateAllProperties: true);

            if (isValid)
            {
                return null;
            }

            var errors = result
                .GroupBy(r => r.MemberNames.FirstOrDefault() ?? string.Empty)
                .ToDictionary(
                g => g.Key,
                g => g.Select(r => r.ErrorMessage ?? "Invalid value").ToArray());

            return Results.ValidationProblem(errors);
        }
    }
}
