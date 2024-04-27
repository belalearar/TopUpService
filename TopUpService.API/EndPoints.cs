using FluentValidation;
using TopUpService.Common.RequestModel;
using TopUpService.Common.Service;
using TopUpService.Common.Validator;

namespace TopUpService.API
{
    public static class EndPoints
    {
        public static void MapApiEndpoints(this WebApplication app)
        {
            app.MapPost("/api/beneficiary", AddNewBeneficiary);
            app.MapGet("/api/get-all-by-user", GetAllUserBeneficiaries);
            app.MapGet("/api/get-top-up-options", GetTopUpOptions);
        }

        public static async Task<IResult> AddNewBeneficiary(AddNewBeneficiaryRequestModel model, IValidator<AddNewBeneficiaryRequestModel> validator, IBeneficiaryService beneficiaryService)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.Errors.Any())
                return Results.BadRequest(validationResult.ToResponse());
            var result = beneficiaryService.AddNewBeneficiary(model);
            if (result.IsSuccess)
            {
                return Results.Created();
            }
            else
            {
                return TypedResults.BadRequest(result.Message);
            }
        }

        public static async Task<IResult> GetAllUserBeneficiaries(int userId, IBeneficiaryService beneficiaryService)
        {
            var result = beneficiaryService.GetAllUserBeneficiaries(userId);
            return Results.Ok(result);
        }

        public static async Task<IResult> GetTopUpOptions(IBeneficiaryService beneficiaryService)
        {
            var result = beneficiaryService.GetAllTopUpOptions();
            return Results.Ok(result);
        }

    }
}