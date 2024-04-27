using TopUpService.Common.RequestModel;
using TopUpService.Common.ResponseModel;

namespace TopUpService.Common.Service
{
    public interface IBeneficiaryService
    {
        AddNewBeneficiaryResponseModel AddNewBeneficiary(AddNewBeneficiaryRequestModel model);
        List<string> GetAllTopUpOptions();
        List<BeneficiaryResponseModel> GetAllUserBeneficiaries(int userId);
    }
}