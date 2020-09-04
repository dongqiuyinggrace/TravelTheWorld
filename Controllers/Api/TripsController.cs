using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripsController : ControllerBase
    {
        private readonly IWorldRepository _repo;
        private readonly IMapper _mapper;

        public TripsController(IWorldRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        //[Authorize]
        public IActionResult Get()
        {
            try
            {
                var trips = _repo.GetAllTrips();
                return Ok(_mapper.Map<IEnumerable<TripViewModel>>(trips));
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return BadRequest("Error occurred");
            }


        }

        [HttpPost]
        public async Task<IActionResult> Post(TripViewModel trip)
        {
            if (ModelState.IsValid)
            {
                var newTrip = _mapper.Map<Trip>(trip);
                _repo.AddTrip(newTrip);

                if (await _repo.SaveChangesAsync())
                {
                    return Created($"api/trips/{trip.Name}", _mapper.Map<TripViewModel>(newTrip));
                }
                else
                {
                    return BadRequest("Fail to save the trip");
                }

            }

            return BadRequest(ModelState);

        }
    }
}