namespace TopUpService.Common.ResponseModel
{
    public class AddNewBeneficiaryResponseModel
    {
        public AddNewBeneficiaryResponseModel(bool isSuccess, string message = "")
        {
            IsSuccess = isSuccess;
            Message = message;
        }
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}