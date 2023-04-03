using FalconValidation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Web;

namespace FalconValidation.Repositories
{
    public class FalconValidationRepository : IFalconValidationRepository
    {
        private readonly AppSettings _appSettings;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public FalconValidationRepository(IOptions<AppSettings> appSettings )
        {
            _appSettings = appSettings.Value;
        }


        //get the record to populate on front end for editing PDF
        public ValidationFields GetFalconValidations(string validationId , string _username )
        {
            log.Info("GetFalconValidations() started for ValidationID - "+validationId);
            using (var conn = new SqlConnection(_appSettings.OriginsConnectionString))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(_appSettings.Proc_GetValidation, conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.Add("@ValidationId", SqlDbType.VarChar, 100).Value = validationId;

                    var reader = cmd.ExecuteReader();
                    var resultTable = new DataTable();
                    resultTable.Load(reader);

                    var validationFields = (resultTable == null || resultTable.Rows.Count == 0) ? new ValidationFields() : resultTable.Select().Select((dtrow, rateDetails) => new ValidationFields()
                    {
                        CustomerName = dtrow["CustomerName"]?.ToString().Trim(),
                        CaseOpenDate = dtrow["CreatedDate"]?.ToString().Trim(),
                        CaseNumber = dtrow["CaseNumber"]?.ToString().Trim(),
                        PhoneNumber = dtrow["PhoneNo"]?.ToString().Trim(),
                        EmailAddress = dtrow["Email"]?.ToString().Trim(),
                        TravelIndicatorStatus = dtrow["TravelIndicator"]?.ToString().Trim(),
                        PDFFileName = dtrow["PDFFileName"]?.ToString().Trim(),
                        ValidationID = dtrow["ValidationID"]?.ToString().Trim(),
                    }).ToList().First();

                    log.Info("GetFalconValidations() ended");
                    return validationFields;
                }
                catch (Exception ex)
                {
                    log.Error("Exception while fetching GetFalconValidation() - " + ex.Message);

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

                    throw ;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public List<ValidationFields> GetAllFalconValidations()
        {
            log.Info("GetAllFalconValidations() started");
            using (var conn = new SqlConnection(_appSettings.OriginsConnectionString))
            {
                try
                {
                    conn.Open();

                   
                        SqlCommand cmd = new SqlCommand(_appSettings.Proc_GetValidation, conn)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        var reader = cmd.ExecuteReader();
                        var resultTable = new DataTable();
                        resultTable.Load(reader);

                        var validationFields = (resultTable == null || resultTable.Rows.Count == 0) ? new List<ValidationFields>() : resultTable.Select().Select((dtrow, rateDetails) => new ValidationFields()
                        {
                            CustomerName = dtrow["CustomerName"]?.ToString().Trim(),
                            CaseOpenDate = dtrow["CreatedDate"]?.ToString().Trim(),
                            CaseNumber = dtrow["CaseNumber"]?.ToString().Trim(),
                            PhoneNumber = dtrow["PhoneNo"]?.ToString().Trim(),
                            EmailAddress = dtrow["Email"]?.ToString().Trim(),
                            TravelIndicatorStatus = dtrow["TravelIndicator"]?.ToString().Trim(),
                            ValidationID = dtrow["ValidationID"]?.ToString().Trim(),
                         //   ModifiedBy = dtrow["ValidationID"]?.ToString().Trim(),
                            PDFFileName = dtrow["PDFFileName"]?.ToString().Trim(),
                           
                        }).ToList();

                    log.Info("GetAllFalconValidations() ended");
                    return validationFields;

                }
                catch (Exception ex)
                {
                    log.Error("Exception while GetAllFalconValidations() - "+ex.Message);
                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public List<ValidationFields> GetResubmitFalconValidations()
        {
            log.Info("GetResubmitFalconValidations() started");
            using (var conn = new SqlConnection(_appSettings.OriginsConnectionString))
            {
  
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(_appSettings.Proc_GetResubmitValidation, conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    var reader = cmd.ExecuteReader();
                    var resultTable = new DataTable();
                    resultTable.Load(reader);

                    var validationFields = (resultTable == null || resultTable.Rows.Count == 0) ? new List<ValidationFields>() : resultTable.Select().Select((dtrow, rateDetails) => new ValidationFields()
                    {
                        CustomerName = dtrow["CustomerName"]?.ToString().Trim(),
                        CaseOpenDate = dtrow["CreatedDate"]?.ToString().Trim(),
                        CaseNumber = dtrow["CaseNumber"]?.ToString().Trim(),
                        PhoneNumber = dtrow["PhoneNo"]?.ToString().Trim(),
                        EmailAddress = dtrow["Email"]?.ToString().Trim(),
                        TravelIndicatorStatus = dtrow["TravelIndicator"]?.ToString().Trim(),
                        PDFFileName = dtrow["PDFFileName"]?.ToString().Trim(),
                        ModifiedBy = dtrow["ModifiedBy"]?.ToString().Trim(),

                    }).ToList();

                    log.Info("GetResubmitFalconValidations() ended");

                    return validationFields;
                }
                catch (Exception ex)
                {
                    log.Error("Exception while GetResubmitFalconValidations() - "+ex.Message);
                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public void SaveFalconValidations(ValidationFields validationFields , string _username )
        {
            log.Info("SaveFalconValidations() started");
            using (var conn = new SqlConnection(_appSettings.OriginsConnectionString))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(_appSettings.Proc_SaveValidation, conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.Add("@ValidationId", SqlDbType.VarChar, 250).Value = validationFields.ValidationID;
                    cmd.Parameters.Add("@CustomerName", SqlDbType.VarChar, 250).Value = validationFields.CustomerName.Trim();
                    cmd.Parameters.Add("@CaseOpenDate", SqlDbType.VarChar, 250).Value = validationFields.CaseOpenDate.Trim();
                    cmd.Parameters.Add("@CaseNumber", SqlDbType.VarChar, 250).Value = validationFields.CaseNumber.Trim();
                    cmd.Parameters.Add("@PhoneNumber", SqlDbType.VarChar, 250).Value = validationFields.PhoneNumber.Trim();
                    cmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar, 250).Value = validationFields.EmailAddress.Trim();
                    cmd.Parameters.Add("@TravelIndicatorStatus", SqlDbType.VarChar, 250).Value = validationFields.TravelIndicatorStatus.Trim();
                    cmd.Parameters.Add("@ModifiedBy", SqlDbType.VarChar, 250).Value =_username;
                    cmd.Parameters.Add("@ExceptionMsg", SqlDbType.VarChar, 250).Value = string.Empty;
                    cmd.Parameters.Add("@ResubmissionStatus", SqlDbType.VarChar, 250).Value = "RESUBMIT";

                    decimal total=0.0M;
                    decimal maxAmount =0;
                    string Finalmerchant = string.Empty;
                    int count = 0;
                    DateTime FinalTransDate = new DateTime();
                    foreach (TransactionHistory x in validationFields.TransactionHistoryList)
                    {
                        //compare amount
                        if (!string.IsNullOrEmpty(x.Amount))
                        {                    
                            Debug.WriteLine(x.Amount + " - " + total);
                            Debug.WriteLine(x.TransactionDate + " - " + Finalmerchant);
                            total = Convert.ToDecimal(x.Amount) + total; // SUM OF AMOUNT
                            if (Convert.ToDecimal(x.Amount) > maxAmount)
                            {
                                maxAmount = Convert.ToDecimal(x.Amount);
                                Finalmerchant = x.MerchantName; //ASSIGN MAX AMOUNT MERCHANT NAME
                            }

                        }

                        //compare date
                        if(!string.IsNullOrEmpty(x.TransactionDate))
                        {
                           
                            DateTime recordDate = Convert.ToDateTime(x.TransactionDate,CultureInfo.InvariantCulture);

                            if (count == 0)
                                FinalTransDate = recordDate;
                            else
                            {
                                int res = DateTime.Compare(recordDate, FinalTransDate);

                                if (res <= 0)
                                    FinalTransDate = recordDate;
                                
                            }
                            count++;
                        }

                    }
                    string FinalAmount = String.Empty;
                    if (total != 0)
                        FinalAmount = total.ToString();

                    Debug.WriteLine(total.ToString());

                    
                    cmd.Parameters.Add("@FinalAmount", SqlDbType.VarChar, 250).Value = FinalAmount;
                    cmd.Parameters.Add("@FinalMerchantName", SqlDbType.VarChar, 250).Value = Finalmerchant;
                    cmd.Parameters.Add("@FinalTransactionDate", SqlDbType.VarChar, 250).Value = FinalTransDate;



                    int counter = 1;
                    foreach (TransactionHistory history in validationFields.TransactionHistoryList)
                    {
                        cmd.Parameters.Add("@TransactionDate" + counter, SqlDbType.VarChar, 250).Value = string.IsNullOrWhiteSpace(validationFields.TransactionHistoryList[counter - 1].TransactionDate.Trim()) ? DBNull.Value : validationFields.TransactionHistoryList[counter-1].TransactionDate.Trim();
                        cmd.Parameters.Add("@TransactionAmount" + counter, SqlDbType.VarChar, 250).Value = string.IsNullOrWhiteSpace(validationFields.TransactionHistoryList[counter - 1].Amount.Trim()) ? DBNull.Value : validationFields.TransactionHistoryList[counter - 1].Amount.Trim();
                        cmd.Parameters.Add("@MerchantName" + counter, SqlDbType.VarChar, 250).Value = string.IsNullOrWhiteSpace(validationFields.TransactionHistoryList[counter - 1].MerchantName.Trim()) ? DBNull.Value : validationFields.TransactionHistoryList[counter - 1].MerchantName.Trim();
                        counter++;
                    }

                    var result = cmd.ExecuteNonQuery();

                    log.Info("SaveFalconValidations() ended");
                }
                catch (Exception ex)
                {
                    log.Error("Exception while SaveFalconValidations() - "+ex.Message);

                    conn.Open();

                    SqlCommand cmd = new SqlCommand(_appSettings.Proc_Exception, conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.Add("@ValidationId", SqlDbType.VarChar, 250).Value = validationFields.ValidationID;
                    cmd.Parameters.Add("@ModifiedBy", SqlDbType.VarChar, 250).Value = _username;
                    cmd.Parameters.Add("@ExceptionMsg", SqlDbType.VarChar, 250).Value = ex.Message.Trim();
                    cmd.Parameters.Add("@FileName", SqlDbType.VarChar, 250).Value = string.Empty;
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    throw;

                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}