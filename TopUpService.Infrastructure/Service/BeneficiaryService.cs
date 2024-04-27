using Microsoft.Extensions.Logging;
using TopUpService.Common;
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

        public AddNewBeneficiaryResponseModel AddNewBeneficiary(AddNewBeneficiaryRequestModel model)
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
            return response.Select(a=>(BeneficiaryResponseModel)a).ToList();
        }

    }
}