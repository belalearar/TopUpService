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
        private List<TopUpUser> users;
        private readonly ILogger<BeneficiaryRepository> _logger;

        public BeneficiaryRepository(ILogger<BeneficiaryRepository> logger)
        {
            _logger = logger;
            beneficiaries = new List<Beneficiary>();
            transactions = new List<Transaction>();
            users = new List<TopUpUser>
            {
                new TopUpUser { Id = 1, Balance = 2000, Name = "User1" },
                new TopUpUser { Id = 2, Balance = 3000, Name = "User2" },
                new TopUpUser { Id = 3, Balance = 5000, Name = "User3" },
            };
        }

        public GenericResponseModel AddBeneficiary(AddNewBeneficiaryRequestModel model)
        {
            try
            {
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
                var beneficiary = GetBeneficiaryById(model.BeneficiaryId);
                if (beneficiary == null)
                {
                    return new GenericResponseModel(false, "beneficiary not found");
                }
                transactions.Add(new Transaction
                {
                    Id = Guid.NewGuid(),
                    Amount = model.TopUpValue,
                    UserId = model.UserId,
                    BeneficiaryId = model.BeneficiaryId,
                    FeeAmount = 1
                });
                beneficiary.Balance += model.TopUpValue;

                return new(true);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error While Top Up Beneficiary : {error}", ex.Message);
                return new(false, "Error While Top Up Beneficiary");
            }
        }

        public BeneficiaryResponseModel? GetBeneficiaryBalance(Guid id)
        {
            try
            {
                return beneficiaries.FirstOrDefault(a => a.Id == id) ?? null;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error While Getting Beneficiary Balance : {error}", ex.Message);
                throw;
            }
        }

        public Beneficiary? GetBeneficiaryById(Guid beneficiaryId)
        {
            return beneficiaries.FirstOrDefault(a => a.Id.Equals(beneficiaryId)) ?? null;
        }

        public List<Transaction> GetByUserTransactions(int userId, DateTime fromTime, DateTime toTime, Guid? beneficiaryId = null)
        {
            return transactions.Where(a => a.UserId.Equals(userId) &&
                                                           a.CreatedAt >= fromTime &&
                                                           a.CreatedAt <= toTime &&
                                                           (!beneficiaryId.HasValue || a.BeneficiaryId == beneficiaryId)).ToList();
        }

        public bool CheckBeneficiaryExistance(string name)
        {
            return beneficiaries.Any(a => a.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        public TopUpUser? GetUserById(int id)
        {
            return users.FirstOrDefault(a => a.Id == id);
        }

        public GenericResponseModel WithdrawUserBalance(int id, decimal topUpValue, int fee)
        {
            try
            {
                var user = GetUserById(id);
                if (user == null)
                {
                    return new GenericResponseModel(false, "User Not Found");
                }
                user.Balance -= topUpValue + fee;
                return new(true);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error While Withdraw From User Balance : {error}", ex.Message);
                return new(false, "Error While Withdraw From User Balance");
            }
        }
    }
}