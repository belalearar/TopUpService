﻿using TopUpService.Common.Entities;

namespace TopUpService.Common.ResponseModel
{
    public class BeneficiaryResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;


        public static implicit operator BeneficiaryResponseModel(Beneficiary source)
        {
            return new BeneficiaryResponseModel
            {
                Id = source.Id,
                Name = source.Name,
            };
        }
    }
}