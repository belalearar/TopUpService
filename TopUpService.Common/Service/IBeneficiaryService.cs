using TopUpService.Common.ResponseModel;

namespace TopUpService.Common.Service
{
    public interface IBeneficiaryService
    {
        AddNewBeneficiaryResponseModel AddNewBeneficiary(AddNewBeneficiaryModel model);
        List<string> GetAllTopUpOptions();
        List<BeneficiaryResponseModel> GetAllUserBeneficiaries(int userId);
    }
}