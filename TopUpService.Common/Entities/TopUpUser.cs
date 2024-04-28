namespace TopUpService.Common.Entities
{
    public class TopUpUser
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Balance { get; set; }
    }
}