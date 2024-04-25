using Dean_Resume.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Text;
using Dean_Resume.Data;
using FluentEmail.Core.Models;
using Microsoft.Extensions.Configuration;

namespace Dean_Resume.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DeanContext _context;
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, DeanContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        #region Download file
        public IActionResult DownloadFile()
        {
            var memory = DownloadSinghFile("dean.pdf", "wwwroot\\file");
            return File(memory.ToArray(), "application/pdf", "dean.pdf");
        }
        private MemoryStream DownloadSinghFile(string filename, string uploadPath)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), uploadPath, filename);
            var memory = new MemoryStream();
            if (System.IO.File.Exists(path))
            {
                var net = new System.Net.WebClient();
                var data = net.DownloadData(path);
                var content = new System.IO.MemoryStream(data);
                memory = content;
            }
            memory.Position = 0;
            return memory;
        }
        #endregion



        #region Contact me

        [HttpPost]
        public IActionResult SubmitForm(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Send email
                SendEmail(model);
                return RedirectToAction("ThankYou");
            }
            return View("~/Views/Home/Index.cshtml", model);
        }

        private void SendEmail(ContactViewModel model)
        {
            string emailTo = _configuration["EmailSettings:ToAddress"];
            string emailFrom = _configuration["EmailSettings:FromAddress"];
            string emailSubject = "New Contact Form Submission";
            string emailBody = $"Name: {model.Name}\nEmail: {model.Email}\nSubject: {model.Subject}\nMessage: {model.Message}";

            using (var message = new MailMessage(emailFrom, emailTo))
            {
                message.Subject = emailSubject;
                message.Body = emailBody;

                using (var client = new SmtpClient())
                {
                    // Configure SMTP settings from appsettings.json
                    client.Host = _configuration["EmailSettings:SmtpServer"];
                    client.Port = int.Parse(_configuration["EmailSettings:SmtpPort"]);
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(emailFrom, _configuration["EmailSettings:SmtpPassword"]);
                    client.EnableSsl = true;

                    client.Send(message);
                }
            }
        }

        public IActionResult ThankYou()
        {
            return View();
        }

        #endregion



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}