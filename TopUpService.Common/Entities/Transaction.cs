namespace TopUpService.Common.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public decimal FeeAmount { get; set; }
        public Guid BeneficiaryId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}