using FalconValidation.Models;
using FalconValidation.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Security.Principal;
using log4net;

namespace FalconValidation.Controllers
{
 
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFalconValidationRepository _falconValidationRepository;
        private readonly AppSettings _appSettings;

        public HomeController(ILogger<HomeController> logger, IFalconValidationRepository falconValidationRepository, IOptions<AppSettings> appSettings)
        {

            _logger = logger;
            _falconValidationRepository = falconValidationRepository;
            _appSettings = appSettings.Value;
           

        }

        public IActionResult Index()
        {
            try
            {

                _logger.LogInformation("User LoggedIn -"+ User.Identity.Name);
                _logger.LogInformation("Is User Authenticated - " + User.Identity.IsAuthenticated);

                //Debug.WriteLine(User.FindFirst(ClaimTypes.Name).Value);
               // Debug.WriteLine(HttpContext.User.Identity.IsAuthenticated);
            
              
                if (User.Identity.IsAuthenticated.Equals(true))
                {
                    ViewBag.IsAuthenticated = "true";
                }
                else ViewBag.IsAuthenticated = "false";

                return View();
            }
            catch(Exception ex)
            {
                _logger.LogError("Exception occured - " + ex.Message);
                return View("Error");
            }

        }


    }

}