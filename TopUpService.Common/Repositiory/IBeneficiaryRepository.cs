using TopUpService.Common.Entities;
using TopUpService.Common.RequestModel;
using TopUpService.Common.ResponseModel;

namespace TopUpService.Common.Repositiory
{
    public interface IBeneficiaryRepository
    {
        GenericResponseModel AddBeneficiary(AddNewBeneficiaryRequestModel model);
        BeneficiaryResponseModel GetBeneficiaryBalance(Guid id);
        List<Beneficiary> GetByUserId(int userId);
        GenericResponseModel TopUpBeneficiary(TopUpRequestModel model);
    }
}