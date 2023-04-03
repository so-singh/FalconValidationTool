using FalconValidation.Models;
using FalconValidation.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Claims;

namespace FalconValidation.Controllers
{
    public class ValidationController : Controller
    {
        private readonly ILogger<ValidationController> _logger;
        private readonly IFalconValidationRepository _falconValidationRepository;
        private readonly AppSettings _appSettings;

        public ValidationController(ILogger<ValidationController> logger, IFalconValidationRepository falconValidationRepository, IOptions<AppSettings> appSettings)
        {
            
            _logger = logger;
            _falconValidationRepository = falconValidationRepository;
            _appSettings = appSettings.Value;
           
        }

        public IActionResult Index(string validationId)
        {
            string _username = HttpContext.User.Identity.Name;
            _logger.LogInformation("User in controller - " + _username);
            _logger.LogInformation("ValidationID to fetch  - " + validationId);
            try
            {
                var result = _falconValidationRepository.GetFalconValidations(validationId , _username );
                string path = Path.Combine(_appSettings.PDFFilePath, result.PDFFileName);
                _logger.LogError("File to check - " + path);

                if (System.IO.File.Exists(path))
                    result.FileExist = "true";
                else
                {
                    #region LogicIF - File not found 

                     result.FileExist = "false";

                    _logger.LogDebug("Exception DB Entry");
                    var conn = new SqlConnection(_appSettings.OriginsConnectionString);
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(_appSettings.Proc_Exception, conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.Add("@ValidationId", SqlDbType.VarChar, 250).Value = validationId;
                    cmd.Parameters.Add("@ModifiedBy", SqlDbType.VarChar, 250).Value = _username;
                    cmd.Parameters.Add("@ExceptionMsg", SqlDbType.VarChar, 250).Value = "Could not find a part of the path " + path;
                    cmd.Parameters.Add("@FileName", SqlDbType.VarChar, 250).Value = result.PDFFileName.Trim();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    #endregion
                }
                return View(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured - " + ex.Message);
                _logger.LogDebug("DB entry for exception - ");

                #region Exception - DB Entry
                var conn = new SqlConnection(_appSettings.OriginsConnectionString);
                conn.Open();

                SqlCommand cmd = new SqlCommand(_appSettings.Proc_Exception, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add("@ValidationId", SqlDbType.VarChar, 250).Value = validationId;
                cmd.Parameters.Add("@ModifiedBy", SqlDbType.VarChar, 250).Value = _username;
                cmd.Parameters.Add("@ExceptionMsg", SqlDbType.VarChar, 250).Value = ex.Message.Trim();
                cmd.Parameters.Add("@FileName", SqlDbType.VarChar, 250).Value = string.Empty;
                cmd.ExecuteNonQuery();
                conn.Close();
                #endregion
                throw;
            }
        }

        [HttpPost]
        public IActionResult Save(IFormCollection collection)
        {
            string _username = HttpContext.User.Identity.Name;
            _logger.LogInformation("Action Save started");
            var validationFields = new ValidationFields()
            {
                ValidationID = collection["ValidationID"].ToString(),
                CaseNumber = collection["CaseNumber"].ToString(),
                CaseOpenDate = collection["CaseOpenDate"].ToString(),
                CustomerName = collection["CustomerName"].ToString(),
                PhoneNumber = collection["PhoneNumber"].ToString(),
                EmailAddress = collection["EmailAddress"].ToString(),
                TravelIndicatorStatus = collection["TravelIndicatorStatus"].ToString(),
                TransactionHistoryList = new List<TransactionHistory>() {
            new TransactionHistory() {
                TransactionDate = collection["TransactionHistoryList[0].TransactionDate"].ToString(),
                  Amount = collection["TransactionHistoryList[0].Amount"].ToString(),
                  MerchantName = collection["TransactionHistoryList[0].MerchantName"].ToString()
              },
              new TransactionHistory() {
                TransactionDate = collection["TransactionHistoryList[1].TransactionDate"].ToString(),
                  Amount = collection["TransactionHistoryList[1].Amount"].ToString(),
                  MerchantName = collection["TransactionHistoryList[1].MerchantName"].ToString()
              },
              new TransactionHistory() {
                TransactionDate = collection["TransactionHistoryList[2].TransactionDate"].ToString(),
                  Amount = collection["TransactionHistoryList[2].Amount"].ToString(),
                  MerchantName = collection["TransactionHistoryList[2].MerchantName"].ToString()
              },
              new TransactionHistory() {
                TransactionDate = collection["TransactionHistoryList[3].TransactionDate"].ToString(),
                  Amount = collection["TransactionHistoryList[3].Amount"].ToString(),
                  MerchantName = collection["TransactionHistoryList[3].MerchantName"].ToString()
              },
              new TransactionHistory() {
                TransactionDate = collection["TransactionHistoryList[4].TransactionDate"].ToString(),
                  Amount = collection["TransactionHistoryList[4].Amount"].ToString(),
                  MerchantName = collection["TransactionHistoryList[4].MerchantName"].ToString()
              },
          }
            };

            _logger.LogDebug("Saving the Data");

            _falconValidationRepository.SaveFalconValidations(validationFields , _username);
            _logger.LogDebug("Redirect to Controller Authenticate");
            return RedirectToAction("Index", "Authenticate", new
            {
                ActiontoPerform = "Edit"
            });
        }

        [HttpGet]
        public FileStreamResult GetPDF(string pdfFileName)
        {
            _logger.LogInformation("File Name is - " + pdfFileName);
            try
            {
                if (!string.IsNullOrWhiteSpace(pdfFileName))
                {

                    FileStream fs = new FileStream(Path.Combine(_appSettings.PDFFilePath, pdfFileName), FileMode.Open, FileAccess.Read);
                    return File(fs, "application/pdf");
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured - " + ex.Message);
                throw;
            }

        }
    }
}