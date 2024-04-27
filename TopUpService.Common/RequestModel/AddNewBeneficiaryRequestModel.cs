namespace TopUpService.Common.RequestModel
{
    public class AddNewBeneficiaryRequestModel
    {
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
    }
}