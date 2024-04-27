using TopUpService.Common.Entities;
using TopUpService.Common.RequestModel;
using TopUpService.Common.ResponseModel;

namespace TopUpService.Common.Repositiory
{
    public interface IBeneficiaryRepository
    {
        GenericResponseModel AddBeneficiary(AddNewBeneficiaryRequestModel model);
        BeneficiaryResponseModel GetBeneficiaryBalance(Guid id);
        Beneficiary GetBeneficiaryById(Guid beneficiaryId);
        List<Beneficiary> GetByUserId(int userId);
        List<Transaction> GetByUserTransactions(int userId, DateTime fromTime, DateTime toTime, Guid? beneficiaryId = null);
        GenericResponseModel TopUpBeneficiary(TopUpRequestModel model);
    }
}