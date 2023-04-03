using FalconValidation.Models;
using FalconValidation.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace FalconValidation.Controllers
{
    public class AuthenticateController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFalconValidationRepository _falconValidationRepository;
        private readonly AppSettings _appSettings;
        public AuthenticateController(ILogger<HomeController> logger, IFalconValidationRepository falconValidationRepository, IOptions<AppSettings> appSettings)
        {

            _logger = logger;
            _falconValidationRepository = falconValidationRepository;
            _appSettings = appSettings.Value;

        }
        public IActionResult Index(string ActionToPerform)
        {
            try
            {
                _logger.LogInformation("Action returned by user is - " + ActionToPerform);

                if (ActionToPerform.Contains("Edit"))
                    return Edit();
                else
                    return ViewOutput();
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured - " + ex.Message);
                return View("Error");
            }
        }

        public IActionResult Edit()
        {
            try
            {
                
                _logger.LogInformation("Getting all the Records to Edit");

                var result = _falconValidationRepository.GetAllFalconValidations();
                ViewBag.ActionToPerform = "Edit";
                
                return View(result);

            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured while fetching edit records - " + ex.Message);
                return View("Error");
            }

        }

        public IActionResult ViewOutput()
        {
            try
            {
                _logger.LogInformation("Fetch all the Resubmitted records");
                var result = _falconValidationRepository.GetResubmitFalconValidations();
                ViewBag.ActionToPerform = "View";
                return View(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured which fetching resubmit records - " + ex.Message);
                return View("Error");
            }

        }
    }
}
