namespace FalconValidation.Models
{
    public class ValidationFields
    {
        
        public string ValidationID { get; set; }
        public string PDFFileName { get; set; }
        public string CaseNumber { get; set; }
        public string CustomerName { get; set; }
        public string CaseOpenDate { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string TravelIndicatorStatus { get; set; }
        public string ModifiedBy { get; set; }

        public string FileExist { get; set; }
        public List<TransactionHistory> TransactionHistoryList { get; set; }
    }

    public class TransactionHistory
    {
        public string TransactionDate { get; set; }
        public string Amount { get; set; }
        public string MerchantName { get; set; }
    }
}
