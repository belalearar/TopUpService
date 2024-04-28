using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using TopUpService.Common.RequestModel;
using TopUpService.Common.ResponseModel;
using TopUpService.Common.Service;
using TopUpService.Common.Validator;

namespace TopUpService.API
{
    [ExcludeFromCodeCoverage]
    public static class EndPoints
    {
        public static void MapApiEndpoints(this WebApplication app)
        {
            app.MapPost("/api/beneficiary", AddNewBeneficiary);
            app.MapGet("/api/get-all-by-user", GetAllUserBeneficiaries);
            app.MapGet("/api/balance", GetBeneficiaryBalance);
            app.MapGet("/api/get-top-up-options", GetTopUpOptions);
            app.MapPost("/api/top-up", TopUp);
            app.MapGet("/api/get-user-info", GetUserInfo);
        }

        public static async Task<IResult> AddNewBeneficiary(AddNewBeneficiaryRequestModel model, IValidator<AddNewBeneficiaryRequestModel> validator, IBeneficiaryService beneficiaryService)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.Errors.Any())
            {
                return Results.BadRequest(validationResult.ToResponse());
            }

            var result = beneficiaryService.AddNewBeneficiary(model);
            return result.IsSuccess ? Results.Created() : TypedResults.BadRequest(result.Message);
        }

        public static IResult GetAllUserBeneficiaries(int userId, IBeneficiaryService beneficiaryService)
        {
            var result = beneficiaryService.GetAllUserBeneficiaries(userId);
            return Results.Ok(result);
        }

        public static IResult GetTopUpOptions(IBeneficiaryService beneficiaryService)
        {
            var result = beneficiaryService.GetAllTopUpOptions();
            return Results.Ok(result);
        }

        public static IResult GetBeneficiaryBalance(Guid id, IBeneficiaryService beneficiaryService)
        {
            var result = beneficiaryService.GetBeneficiaryBalance(id);
            return result == null ? Results.NotFound() : Results.Ok(result);
        }

        public static async Task<IResult> TopUp(TopUpRequestModel model, IValidator<TopUpRequestModel> validator, IBeneficiaryService beneficiaryService)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.Errors.Any())
            {
                return Results.BadRequest(validationResult.ToResponse());
            }

            var result = beneficiaryService.TopUpBeneficiary(model);
            return result.IsSuccess ? Results.Created() : TypedResults.BadRequest(result.Message);
        }

        public static IResult GetUserInfo(int userId, IBeneficiaryService beneficiaryService)
        {
            var result = beneficiaryService.GetTopUpUser(userId);
            return result != null ? Results.Ok(result) : TypedResults.NotFound();
        }

    }
}