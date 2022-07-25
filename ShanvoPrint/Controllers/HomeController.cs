using AspNetCoreHero.ToastNotification.Abstractions;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ShanvoPrint.Helper;
using ShanvoPrint.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ShanvoPrint.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly IWebHostEnvironment _webHostEnvironment;
        public INotyfService _notifyService;
        private readonly IDNTCaptchaValidatorService _validatorService;
        private readonly DNTCaptchaOptions _captchaOptions;
        public HomeController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment, INotyfService notifyService, IDNTCaptchaValidatorService validatorService, IOptions<DNTCaptchaOptions> options)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _notifyService = notifyService;
            _validatorService = validatorService;
            _captchaOptions = options == null ? throw new ArgumentNullException(nameof(options)) : options.Value;
        }

    public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Send(Contact contact)
        {
            if (!_validatorService.HasRequestValidCaptchaEntry(Language.English, DisplayMode.SumOfTwoNumbersToWords))
            {
                this.ModelState.AddModelError(_captchaOptions.CaptchaComponent.CaptchaInputName, "Please enter the security code as a number.");
               
            }
            var body = "This is a request for contact from Shanvo Print website. Please find the details below from the user requesting to be contacted " + "</br>Name: " + contact.Name + "<br>Address: " + "<br>Email: " + contact.Email + "<br>Subject of contact: " + contact.Subject + "<br>Message: " + contact.Content + "<br>";
            var mailHelper = new MailHelper2(_configuration);

            if (mailHelper.Send(contact.Email, _configuration["Gmail:Username"], contact.Subject, body))
            {
                _notifyService.Success("Thank you, our team will contact you soon.");
                return Redirect("~/");

            }
            else
            {
                _notifyService.Error("Error sending email, Try later");
                return Redirect("~/");
            }
        }

        [HttpPost]
        public IActionResult Quote(Quote contact)
        {
            if (!_validatorService.HasRequestValidCaptchaEntry(Language.English, DisplayMode.SumOfTwoNumbersToWords))
            {
                this.ModelState.AddModelError(_captchaOptions.CaptchaComponent.CaptchaInputName, "Please enter the security code as a number.");

            }
            var body = "This is a request for contact from Shanvo Print website. Please find the details below from the user requesting to be contacted " + "</br>Name: " + contact.Name + "<br>Address: " + "<br>Email: " + contact.Email + "<br>Subject of contact: " + contact.Subject + "<br>Message: " + contact.Content + "<br>";
            var mailHelper = new MailHelper2(_configuration);

            if (mailHelper.Send(contact.Email, _configuration["Gmail:Username"], contact.Subject, body))
            {
                _notifyService.Success("Thank you, our team will contact you soon.");
                return Redirect("~/");

            }
            else
            {
                _notifyService.Error("Error sending email, Try later");
                return Redirect("~/");
            }
        }

        public IActionResult GetQuote()
        {
            return View();
        }
        public IActionResult Services()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
