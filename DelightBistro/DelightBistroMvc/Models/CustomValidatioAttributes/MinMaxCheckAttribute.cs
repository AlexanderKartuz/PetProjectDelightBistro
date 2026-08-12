using System.ComponentModel.DataAnnotations;

namespace DelightBistroMvc.Models.CustomValidatioAttributes
{
    public class MinMaxCheckAttribute : ValidationAttribute
    {
        private int _min;
        private int _max;

        /// <summary>
        /// By default min = 1; max = 10
        /// </summary>
        public MinMaxCheckAttribute()
        {
            _min = 1;
            _max = 10;
        }

        public MinMaxCheckAttribute(int min, int max)
        {
            _min = min;
            _max = max;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.IsNullOrEmpty(ErrorMessage)
                ? $"For field {name} value must be [{_min}, {_max}]"
                : ErrorMessage;
        }

        public override bool IsValid(object? value)
        {
            if (value is not int)
            {
                throw new Exception("Min max attribute must be on the int Setting");
            }
            var number = (int)value;

            return number >= _min && number <= _max;
        }
    }
}
