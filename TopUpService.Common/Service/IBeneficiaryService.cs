using TopUpService.Common.ResponseModel;

namespace TopUpService.Common.Service
{
    public interface IBeneficiaryService
    {
        AddNewBeneficiaryResponseModel AddNewBeneficiary(AddNewBeneficiaryModel model);
        List<BeneficiaryResponseModel> GetAllUserBeneficiaries(int userId);
    }
}