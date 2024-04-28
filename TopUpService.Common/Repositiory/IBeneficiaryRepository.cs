using TopUpService.Common.Entities;
using TopUpService.Common.RequestModel;
using TopUpService.Common.ResponseModel;

namespace TopUpService.Common.Repositiory
{
    public interface IBeneficiaryRepository
    {
        GenericResponseModel AddBeneficiary(AddNewBeneficiaryRequestModel model);
        bool CheckBeneficiaryExistance(string name);
        BeneficiaryResponseModel? GetBeneficiaryBalance(Guid id);
        Beneficiary? GetBeneficiaryById(Guid beneficiaryId);
        List<Beneficiary> GetByUserId(int userId);
        List<Transaction> GetByUserTransactions(int userId, DateTime fromTime, DateTime toTime, Guid? beneficiaryId = null);
        TopUpUser? GetUserById(int id);
        GenericResponseModel TopUpBeneficiary(TopUpRequestModel model);
        GenericResponseModel WithdrawUserBalance(int id, decimal topUpValue, int v);
    }
}