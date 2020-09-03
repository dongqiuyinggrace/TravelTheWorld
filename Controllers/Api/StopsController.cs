using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [ApiController]
    [Route("/api/trips/{tripName}/stops")]
    public class StopsController : ControllerBase
    {
        private readonly IWorldRepository _repo;
        private readonly WorldContext _context;
        private readonly IMapper _mapper;
        private readonly GeoCoordsService _coordService;
        public StopsController(IWorldRepository repo, WorldContext context, IMapper mapper, GeoCoordsService coordService)
        {
            _coordService = coordService;
            _mapper = mapper;
            _context = context;
            _repo = repo;
        }

        [HttpGet]
        public IActionResult Get(string tripName)
        {
            try
            {
                var trip = _repo.GetTripByName(tripName);
                return Ok(_mapper.Map<IEnumerable<StopViewModel>>(trip.Stops.OrderBy(s => s.Order).ToList()));
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return BadRequest("Fail to get trip");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Post(string tripName, StopViewModel stopViemModel)
        {
            try
            {
                var newStop = _mapper.Map<Stop>(stopViemModel);

                var result = await _coordService.GetCoordsAsync(newStop.Name);
                if (!result.Success)
                {
                    System.Console.WriteLine(result.Message);
                    return BadRequest("Fail to get Coordinate");
                }
                else
                {
                    newStop.Latitude = result.Latitude;
                    newStop.Longitude = result.Longitude;

                    _repo.AddStop(tripName, newStop);

                    if (await _repo.SaveChangesAsync())
                    {
                        return Created($"api/trips/{tripName}/stops/{newStop.Name}", _mapper.Map<StopViewModel>(newStop));
                    }
                    return BadRequest("Fail to save stop");
                }

            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return BadRequest("Fail to save stop");
            }
        }
    }
}