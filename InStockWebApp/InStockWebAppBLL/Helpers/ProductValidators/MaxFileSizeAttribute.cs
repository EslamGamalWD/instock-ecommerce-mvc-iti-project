using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

public class MaxFileSizeAttribute : ValidationAttribute
{
    private readonly long _maxFileSize;

    public MaxFileSizeAttribute(long maxFileSize)
    {
        _maxFileSize = maxFileSize;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            if (file.Length > _maxFileSize)
            {
                return new ValidationResult($"The file size cannot exceed {_maxFileSize / 1024 / 1024} MB.");
            }
        }

        return ValidationResult.Success;
    }
}
