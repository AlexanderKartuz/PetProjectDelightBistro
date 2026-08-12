using System.ComponentModel.DataAnnotations;

namespace DelightBistroMvc.Models.CustomValidatioAttributes
{
    public class TheWritingIsLongEnoughAttribute : ValidationAttribute
    {
        private readonly int _minWords;
        private readonly int _maxWords;

        public TheWritingIsLongEnoughAttribute(int minWords = 5, int maxWords = 25)
        {
            _minWords = minWords;
            _maxWords = maxWords;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var text = value as string;
            if (string.IsNullOrWhiteSpace(text))
            {
                return ValidationResult.Success;
            }

            var normalized = text.Trim();
            var wordsCount = normalized
                .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)
                .Length;

            if (wordsCount < _minWords || wordsCount > _maxWords)
            {
                return new ValidationResult(
                    ErrorMessage ?? $"The text must contain from {_minWords} to {_maxWords} words.");
            }

            return ValidationResult.Success;
        }
    }
}
