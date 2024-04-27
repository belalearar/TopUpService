namespace TopUpService.Common.RequestModel
{
    public class TopUpRequestModel
    {
        public int UserId { get; set; }
        public Guid BeneficiaryId { get; set; }
        public decimal TopUpValue { get; set; }
        public bool IsVerified { get; set; }
    }
}