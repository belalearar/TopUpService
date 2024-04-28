using TopUpService.Common;
using TopUpService.Common.Service;
using TopUpService.Common.Entities;
using Microsoft.Extensions.Logging;
using TopUpService.Common.Repositiory;
using TopUpService.Common.RequestModel;
using TopUpService.Common.ResponseModel;

namespace TopUpService.Infrastructure.Service
{
    public class BeneficiaryService : IBeneficiaryService
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
            bool isExist = _beneficiaryRepository.CheckBeneficiaryExistance(model.Name);

            if (isExist)
            {
                return new(false, "User Already Exists.");
            }
            var userBeneficiaries = _beneficiaryRepository.GetByUserId(model.UserId);
            if (userBeneficiaries.Count >= 5)//max user beneficiaries 5
            {
                return new(false, "User Exceed Number Of Available Beneficiaries.");
            }

            var response = _beneficiaryRepository.AddBeneficiary(model);
            return response;
        }

        public List<string> GetAllTopUpOptions()
        {
            return Constants.GetTopUpOptions();
        }

        public List<BeneficiaryResponseModel?> GetAllUserBeneficiaries(int userId)
        {
            var response = _beneficiaryRepository.GetByUserId(userId);
            return response.Select(a => (BeneficiaryResponseModel?)a).ToList();
        }

        public BeneficiaryResponseModel? GetBeneficiaryBalance(Guid id)
        {
            var response = _beneficiaryRepository.GetBeneficiaryBalance(id);
            return response;
        }

        public GenericResponseModel TopUpBeneficiary(TopUpRequestModel model)
        {
            var beneficiary = _beneficiaryRepository.GetBeneficiaryById(model.BeneficiaryId);
            if (beneficiary == null)
            {
                return new(false, "Beneficiary Not Found.");
            }
            var user = _beneficiaryRepository.GetUserById(model.UserId);
            if (user == null)
            {
                return new GenericResponseModel(false, "User Not Found");
            }

            if (user.Balance < model.TopUpValue + 1)
            {
                return new GenericResponseModel(false, "Top Up Value Should Be Less Than Or Equal Balance.");
            }

            var fromTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var toTime = fromTime.AddMonths(1).AddDays(-1);

            //Get All Beneficiary Transactions This Month
            var userTransactions = _beneficiaryRepository.GetByUserTransactions(model.UserId, fromTime, toTime, beneficiary.Id);

            //Get All User Transactions For All Beneficiaries This Month
            var userMonthTransactions = _beneficiaryRepository.GetByUserTransactions(model.UserId, fromTime, toTime);

            if (model.IsVerified)//if user verified max 500 AED per beneficiary per month
            {
                if (userTransactions.Sum(a => a.Amount) + model.TopUpValue > 500)
                {
                    return new(false, "User Is Verified, Exceed The Max Top Up Value.");
                }
            }
            else//if user not verified max 1000 AED per beneficiary per month
            {
                if (userTransactions.Sum(a => a.Amount) + model.TopUpValue > 1000)
                {
                    return new(false, "User Is Not Verified, Exceed The Max Top Up Value.");
                }
            }

            //max user top up for all beneficiaries in month
            if ((userMonthTransactions.Sum(a => a.Amount) + model.TopUpValue) > 3000)
            {
                return new(false, "User Exceed The Max Top Up Limit Per All Beneficiaries.");
            }
            //withdraw amount from user before top up the beneficiary
            var withdrawUserBalace = _beneficiaryRepository.WithdrawUserBalance(user.Id, model.TopUpValue, 1);
            if (!withdrawUserBalace.IsSuccess)
            {
                return withdrawUserBalace;
            }
            //top up beneficiary
            var response = _beneficiaryRepository.TopUpBeneficiary(model);
            if (!response.IsSuccess)
            {
                _beneficiaryRepository.WithdrawUserBalance(user.Id, model.TopUpValue * -1, -1);
            }
            return response;
        }

        public TopUpUser? GetTopUpUser(int userId)
        {
            return _beneficiaryRepository.GetUserById(userId) ?? null;
        }
    }
}