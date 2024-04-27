using FluentValidation;
using TopUpService.Common.RequestModel;

namespace TopUpService.Common.Validator
{
    public class AddNewBeneficiaryModelValidator : AbstractValidator<AddNewBeneficiaryRequestModel>
    {
        public AddNewBeneficiaryModelValidator()
        {
            RuleFor(a => a.Name).NotEmpty().MaximumLength(20);
            RuleFor(a => a.UserId).NotEmpty();
        }
    }
}