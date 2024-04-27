namespace TopUpService.Common.Validator;

public class ValidationErrorResponse
{
    public ValidationErrorResponse(string propertyName, string message)
    {
        PropertyName = propertyName;
        Message = message;
    }

    public string PropertyName { get; }
    public string Message { get; }
}