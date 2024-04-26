namespace TopUpService.Common.Entities
{
    public class Beneficiary
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public decimal Balance { get; set; }
    }
}