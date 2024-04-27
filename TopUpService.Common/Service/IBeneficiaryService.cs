using TopUpService.Common.RequestModel;
using TopUpService.Common.ResponseModel;

namespace TopUpService.Common.Service
{
    public interface IBeneficiaryService
    {
        GenericResponseModel AddNewBeneficiary(AddNewBeneficiaryRequestModel model);
        List<string> GetAllTopUpOptions();
        List<BeneficiaryResponseModel> GetAllUserBeneficiaries(int userId);
        BeneficiaryResponseModel GetBeneficiaryBalance(Guid id);
        GenericResponseModel TopUpBeneficiary(TopUpRequestModel model);
    }
}