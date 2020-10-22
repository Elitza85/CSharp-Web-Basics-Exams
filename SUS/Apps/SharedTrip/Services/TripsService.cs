using SharedTrip.Data;
using SharedTrip.ViewModels.Trip;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SharedTrip.Services
{
    public class TripsService : ITripsService
    {
        private readonly ApplicationDbContext db;

        public TripsService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void AddTrip(AddTripInputModel input)
        {
            this.db.Trips.Add(new Trip
            {
                StartPoint = input.StartPoint,
                EndPoint = input.EndPoint,
                DepartureTime = DateTime
                .ParseExact(input.DepartureTime, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture),
                Description = input.Description,
                ImagePath = input.ImagePath,
                Seats = input.Seats,
            });
            this.db.SaveChanges();
        }

        public void AddUserToTrip(string tripId, string userId)
        {
            this.db.UserTrips.Add(new UserTrip
            {
                TripId = tripId,
                UserId = userId
            });
            this.db.SaveChanges();
        }

        public IEnumerable<TripViewModel> AllTrips()
        {
            return this.db.Trips.Select(x => new TripViewModel
            {
                Id = x.Id,
                StartPoint = x.StartPoint,
                EndPoint = x.EndPoint,
                DepartureTime = x.DepartureTime,
                Seats = x.Seats,
                UsedSeats = x.UserTrips.Count()
            }).ToList();
        }

        public TripDetailsViewModel GetTripInfoById(string id)
        {
            return this.db.Trips.Where(x => x.Id == id)
                .Select(x => new TripDetailsViewModel
                {
                    ImagePath = x.ImagePath,
                    Id = x.Id,
                    DepartureTime = x.DepartureTime,
                    Description = x.Description,
                    StartPoint = x.StartPoint,
                    EndPoint = x.EndPoint,
                    Seats = x.Seats,
                    UsedSeats = x.UserTrips.Count(),
                }).FirstOrDefault();
        }

        public bool CanUserJoinTrip(string userId, string tripId)
        {
            if(this.db.UserTrips.Any(x => x.UserId == userId
            && x.TripId == tripId))
            {
                return false;
            }
            return true;
        }
    }
}
