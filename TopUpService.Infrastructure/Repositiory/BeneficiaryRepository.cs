using TopUpService.Common;
using Microsoft.Extensions.Logging;
using TopUpService.Common.Entities;
using TopUpService.Common.Repositiory;
using TopUpService.Common.ResponseModel;

namespace TopUpService.Infrastructure.Repositiory
{
    internal class BeneficiaryRepository : IBeneficiaryRepository
    {
        private List<Beneficiary> beneficiaries;
        private readonly ILogger<BeneficiaryRepository> _logger;

        public BeneficiaryRepository(ILogger<BeneficiaryRepository> logger)
        {
            _logger = logger;
            beneficiaries = new List<Beneficiary>();
        }

        public AddNewBeneficiaryResponseModel AddBeneficiary(AddNewBeneficiaryModel model)
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
                _logger.LogError("Error While Creating Beneficiary : {error}", ex.Message);
                throw;
            }
        }
    }
}