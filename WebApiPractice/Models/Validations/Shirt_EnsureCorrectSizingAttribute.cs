﻿using System.ComponentModel.DataAnnotations;

namespace WebApiPractice.Models.Validations
{
    public class Shirt_EnsureCorrectSizingAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value,ValidationContext context)
        {
            var shirt = context.ObjectInstance as Shirt;
            if (shirt != null& !string.IsNullOrWhiteSpace(shirt.Gender)) {
                if (shirt.Gender.Equals("male", StringComparison.OrdinalIgnoreCase)&& shirt.Size<8) {
                    return new ValidationResult("For men's shirt, the size has to be greater or equal to 8");
                }
                else if(shirt.Gender.Equals("female", StringComparison.OrdinalIgnoreCase) && shirt.Size < 6)
                {
                    return new ValidationResult("For women's shirt, the size has to be greater or equal to 6");
                }
            }
            return ValidationResult.Success;
        }
    }
}
