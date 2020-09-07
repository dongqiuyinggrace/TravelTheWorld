using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TheWorld.Models
{
    public class WorldRepository : IWorldRepository
    {
        private readonly WorldContext _context;
        public WorldRepository(WorldContext context)
        {
            _context = context;
        }

        public void AddStop(string tripName, Stop stop)
        {
            var trip = GetTripByName(tripName);
            if (trip != null)
            {
                trip.Stops.Add(stop);
                _context.Stops.Add(stop);
            }
            
        }

        public void AddStop(int id, Stop stop)
        {
            var trip = GetTripById(id);
            if (trip != null)
            {
                trip.Stops.Add(stop);
                _context.Stops.Add(stop);
            }
        }

        public void AddTrip(Trip trip)
        {
            _context.Trips.Add(trip);
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            var trips = _context.Trips.ToList();
            return trips;
        }

        public Trip GetTripById(int id)
        {
            var trip = _context.Trips.Include(t => t.Stops).FirstOrDefault(t => t.Id == id);
            return trip;
        }

        public Trip GetTripByName(string name)
        {
            var trip = _context.Trips.Include(t => t.Stops).FirstOrDefault(t => t.Name == name);
            return trip;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}