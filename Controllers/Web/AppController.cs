using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailService _mailService;
        private IConfiguration _config;
        private readonly IWorldRepository _repo;

        public AppController(IMailService mailService, IConfiguration config, IWorldRepository repo)
        {
            _repo = repo;
            _mailService = mailService;
            _config = config;
        }
        public IActionResult Index()
        {
            var trips = _repo.GetAllTrips();
            return View(trips);
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
        public IActionResult Contact(ContactViewModel model)
        {
            if (model.Email.Contains("aol.com")) ModelState.AddModelError("", "We don't support AOL address");
            if (ModelState.IsValid)
            {
                _mailService.SendMail(_config["MailSettings:ToAddress"], model.Email, "From The World", model.Message);
                ViewBag.UserMessage = "Message Sent";
            }

            ModelState.Clear();

            return View();
        }

    }
}