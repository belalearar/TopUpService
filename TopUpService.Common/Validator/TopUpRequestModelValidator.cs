using FluentValidation;
using TopUpService.Common.RequestModel;

namespace TopUpService.Common.Validator
{
    public class TopUpRequestModelValidator : AbstractValidator<TopUpRequestModel>
    {
        public TopUpRequestModelValidator()
        {
            RuleFor(a => a.TopUpValue).NotEmpty();
            RuleFor(a => a.BeneficiaryId).NotEmpty();
            RuleFor(a => a.UserId).NotEmpty();
        }
    }
}