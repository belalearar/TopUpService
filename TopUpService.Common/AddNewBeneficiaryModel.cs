using System.ComponentModel.DataAnnotations;

namespace TopUpService.Common
{
    public class AddNewBeneficiaryModel
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public bool IsVerified { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Beneficiary Name should be less than 20 characters")]
        public string Name { get; set; } = null!;
    }
}