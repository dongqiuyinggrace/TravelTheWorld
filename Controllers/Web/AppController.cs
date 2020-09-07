using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System.Threading.Tasks;

namespace TheWorld.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailService _mailService;
        private IConfiguration _config;
        private readonly IWorldRepository _repo;
        private readonly ILogger<AppController> _logger;
        private readonly IMapper _mapper;

        public AppController(IMailService mailService, 
                            IConfiguration config, 
                            IWorldRepository repo, 
                            ILogger<AppController> logger,
                            IMapper mapper)
        {
            _repo = repo;
            _logger = logger;
            _mapper = mapper;
            _mailService = mailService;
            _config = config;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpGet(Name="GetTrips")]
        public IActionResult Trips()
        {
            try
            {
                var trips = _repo.GetAllTrips();
                return View(trips);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error happened in trips: {ex.Message}");
                return BadRequest("Fail to get trips");
            }

        }

        [HttpGet("/App/trips/{id}/stops")]
        public IActionResult Stops(int id)
        {
            try
            {
                var trip = _repo.GetTripById(id);
                return View(trip.Stops);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error happened in trips: {ex.Message}");
                return BadRequest("Fail to get trips");
            }

        }

        [HttpPost("/App/trips/{id}/stops")]
        public async Task<IActionResult> Stops(int id, StopViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var newStop = _mapper.Map<Stop>(vm);
                _repo.AddStop(id, newStop);

                if (await _repo.SaveChangesAsync())
                {
                    return Created($"app/trips/{id}/stops/{newStop.Id}", _mapper.Map<TripViewModel>(newStop));
                }
                else
                {
                    return BadRequest("Fail to save the stop");
                }

            }
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