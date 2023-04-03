namespace FalconValidation.Models
{
    public class AppSettings
    {
        public string OriginsConnectionString { get; set; }
        public string PDFFilePath { get; set; }

        public string Proc_Exception { get; set; }
        public string Proc_GetResubmitValidation { get; set; }
        public string Proc_GetValidation { get; set; }

        public string Proc_SaveValidation { get; set; }
    }
}
