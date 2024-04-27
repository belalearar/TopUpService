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
                var beneficiary = beneficiaries.FirstOrDefault(a => a.Id.Equals(model.BeneficiaryId));
                if (beneficiary == null)
                {
                    return new(false, "Beneficiary Not Found.");
                }

                if (beneficiary.Balance != 0 && model.TopUpValue > beneficiary.Balance)
                {
                    return new(false, "Top Up Value Should Be Less Than Or Equal Balance.");
                }

                var fromTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var toTime = fromTime.AddMonths(1).AddDays(-1);

                var userTransactions = transactions.Where(a => a.UserId.Equals(model.UserId) &&
                                                               a.CreatedAt >= fromTime &&
                                                               a.CreatedAt <= toTime &&
                                                               a.BeneficiaryId == model.BeneficiaryId);

                var userMonthTransactions = transactions.Where(a => a.UserId.Equals(model.UserId) &&
                                                               a.CreatedAt >= fromTime &&
                                                               a.CreatedAt <= toTime);

                if (model.IsVerified)
                {
                    if (userTransactions.Sum(a => a.Amount) + model.TopUpValue > 500)
                    {
                        return new(false, "User Is Verified, Exceed The Max Top Up Value.");
                    }
                }
                else
                {
                    if (userTransactions.Sum(a => a.Amount) + model.TopUpValue > 1000)
                    {
                        return new(false, "User Is Not Verified, Exceed The Max Top Up Value.");
                    }
                }

                if (userMonthTransactions.Sum(a => a.Amount) + model.TopUpValue > 3000)
                {
                    return new(false, "User Exceed The Max Top Up Limit Per All Beneficiaries.");
                }

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
    }
}