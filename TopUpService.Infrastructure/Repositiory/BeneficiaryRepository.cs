using Microsoft.Extensions.Logging;
using TopUpService.Common.Entities;
using TopUpService.Common.Repositiory;
using TopUpService.Common.ResponseModel;
using TopUpService.Common.RequestModel;

namespace TopUpService.Infrastructure.Repositiory
{
    internal class BeneficiaryRepository : IBeneficiaryRepository
    {
        private List<Beneficiary> beneficiaries;
        private List<Transaction> transactions;
        private readonly ILogger<BeneficiaryRepository> _logger;

        public BeneficiaryRepository(ILogger<BeneficiaryRepository> logger)
        {
            _logger = logger;
            beneficiaries = new List<Beneficiary>();
            transactions = new List<Transaction>();
        }

        public GenericResponseModel AddBeneficiary(AddNewBeneficiaryRequestModel model)
        {
            try
            {
                var isExist = beneficiaries.Any(a => a.Name.Equals(model.Name, StringComparison.CurrentCultureIgnoreCase));
                if (isExist)
                {
                    return new(false, "User Already Exists.");
                }
                var userBeneficiaries = beneficiaries.Count(a => a.UserId == model.UserId);
                if (userBeneficiaries >= 5)
                {
                    return new(false, "User Exceed Number Of Available Beneficiaries.");
                }
                beneficiaries.Add(new Beneficiary
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name.Trim(),
                    UserId = model.UserId
                });
                return new(true);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error While Creating Beneficiary : {error}", ex.Message);
                return new(false, "Error While Creating Beneficiary");

            }
        }

        public List<Beneficiary> GetByUserId(int userId)
        {
            try
            {
                return beneficiaries.Where(a => a.UserId == userId).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error While Getting User Beneficiaries : {error}", ex.Message);
                throw;
            }
        }

        public GenericResponseModel TopUpBeneficiary(TopUpRequestModel model)
        {
            try
            {
                Beneficiary beneficiary = GetBeneficiaryById(model.BeneficiaryId);
                transactions.Add(new Transaction
                {
                    Id = Guid.NewGuid(),
                    Amount = model.TopUpValue,
                    UserId = model.UserId,
                    BeneficiaryId = model.BeneficiaryId,
                    FeeAmount = 1
                });
                beneficiary.Balance += model.TopUpValue - 1;

                return new(true);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error While Top Up Beneficiary : {error}", ex.Message);
                return new(false, "Error While Top Up Beneficiary");
            }
        }

        public BeneficiaryResponseModel GetBeneficiaryBalance(Guid id)
        {
            try
            {
                return beneficiaries.FirstOrDefault(a => a.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error While Getting Beneficiary Balance : {error}", ex.Message);
                throw;
            }
        }

        public Beneficiary GetBeneficiaryById(Guid beneficiaryId)
        {
            return beneficiaries.FirstOrDefault(a => a.Id.Equals(beneficiaryId));
        }

        public List<Transaction> GetByUserTransactions(int userId, DateTime fromTime, DateTime toTime, Guid? beneficiaryId = null)
        {
            return transactions.Where(a => a.UserId.Equals(userId) &&
                                                           a.CreatedAt >= fromTime &&
                                                           a.CreatedAt <= toTime &&
                                                           (!beneficiaryId.HasValue || a.BeneficiaryId == beneficiaryId)).ToList();
        }
    }
}