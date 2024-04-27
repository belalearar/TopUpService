using Microsoft.Extensions.Logging;
using TopUpService.Common;
using TopUpService.Common.Entities;
using TopUpService.Common.Repositiory;
using TopUpService.Common.RequestModel;
using TopUpService.Common.ResponseModel;
using TopUpService.Common.Service;

namespace TopUpService.Infrastructure.Service
{
    internal class BeneficiaryService : IBeneficiaryService
    {
        private readonly ILogger<BeneficiaryService> _logger;
        private readonly IBeneficiaryRepository _beneficiaryRepository;

        public BeneficiaryService(ILogger<BeneficiaryService> logger, IBeneficiaryRepository beneficiaryRepository)
        {
            _logger = logger;
            _beneficiaryRepository = beneficiaryRepository;
        }

        public GenericResponseModel AddNewBeneficiary(AddNewBeneficiaryRequestModel model)
        {
            var response = _beneficiaryRepository.AddBeneficiary(model);
            return response;
        }

        public List<string> GetAllTopUpOptions()
        {
            return Constants.GetTopUpOptions();
        }

        public List<BeneficiaryResponseModel> GetAllUserBeneficiaries(int userId)
        {
            var response = _beneficiaryRepository.GetByUserId(userId);
            return response.Select(a => (BeneficiaryResponseModel)a).ToList();
        }

        public BeneficiaryResponseModel GetBeneficiaryBalance(Guid id)
        {
            var response = _beneficiaryRepository.GetBeneficiaryBalance(id);
            return (BeneficiaryResponseModel)response;
        }

        public GenericResponseModel TopUpBeneficiary(TopUpRequestModel model)
        {
            Beneficiary beneficiary = _beneficiaryRepository.GetBeneficiaryById(model.BeneficiaryId);
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

            List<Transaction> userTransactions = _beneficiaryRepository.GetByUserTransactions(model.UserId, fromTime, toTime,beneficiary.Id);


            var userMonthTransactions = _beneficiaryRepository.GetByUserTransactions(model.UserId, fromTime, toTime);
            
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

            GenericResponseModel response = _beneficiaryRepository.TopUpBeneficiary(model);
            return response;
        }
    }
}