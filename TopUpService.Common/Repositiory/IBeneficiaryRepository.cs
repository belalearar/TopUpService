using TopUpService.Common.Entities;
using TopUpService.Common.ResponseModel;

namespace TopUpService.Common.Repositiory
{
    public interface IBeneficiaryRepository
    {
        AddNewBeneficiaryResponseModel AddBeneficiary(AddNewBeneficiaryModel model);
        List<Beneficiary> GetByUserId(int userId);
    }
}