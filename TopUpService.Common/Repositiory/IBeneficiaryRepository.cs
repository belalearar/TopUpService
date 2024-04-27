using TopUpService.Common.Entities;
using TopUpService.Common.RequestModel;
using TopUpService.Common.ResponseModel;

namespace TopUpService.Common.Repositiory
{
    public interface IBeneficiaryRepository
    {
        AddNewBeneficiaryResponseModel AddBeneficiary(AddNewBeneficiaryRequestModel model);
        List<Beneficiary> GetByUserId(int userId);
    }
}