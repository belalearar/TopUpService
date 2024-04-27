using FluentValidation.Results;

namespace TopUpService.Common.Validator;

public static class ValidationErrorResponseMapper
{
    public static ValidationErrorResponse Map(this ValidationFailure source)
    {
        var message = string.IsNullOrWhiteSpace(source.ErrorCode)
            ? source.ErrorMessage
            : $"{source.ErrorCode} - {source.ErrorMessage}";

        var model = new ValidationErrorResponse(source.PropertyName, message);

        return model;
    }
    
    public static IEnumerable<ValidationErrorResponse> ToResponse(this ValidationResult result)
    {
        var source = result.Errors;
        return source.Select(Map);
    }
}