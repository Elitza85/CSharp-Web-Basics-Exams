using SharedTrip.Data;
using SharedTrip.ViewModels.Trips;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
                DepartureTime = DateTime.ParseExact(input.DepartureTime,
                "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture),
                Description = input.Description,
                ImagePath = input.ImagePath,
                Seats = input.Seats,
            });
            this.db.SaveChanges();
        }


        public IEnumerable<AllTripsViewModel> All()
        {
            return this.db.Trips.Select(x => new AllTripsViewModel
            {
                StartPoint = x.StartPoint,
                EndPoint = x.EndPoint,
                DepartureTime = x.DepartureTime,
                ImagePath = x.ImagePath,
                UsedSeats = this.db.UserTrips.Count(),
                Seats = x.Seats,
                Description = x.Description,
                Id = x.Id
            }).ToList();
        }

        public TripViewModel Details(string tripId)
        {
            return this.db.Trips
                .Where(x => x.Id == tripId)
                .Select(x => new TripViewModel
                {
                    Id = x.Id,
                    StartPoint = x.StartPoint,
                    EndPoint = x.EndPoint,
                    Description = x.Description,
                    ImagePath = x.ImagePath,
                    Seats = x.Seats,
                    UsedSeats = this.db.UserTrips.Count(),
                    DepartureTime = x.DepartureTime
                }).FirstOrDefault();
        }
        public void AddUserToTrip(string userId, string tripId)
        {
            this.db.UserTrips.Add(new UserTrip
            {
                UserId = userId,
                TripId = tripId
            });
            this.db.SaveChanges();
        }

        public bool HasUserJoinedTrip(string userId, string tripId)
        {
            return this.db.UserTrips.Any(x => x.UserId == userId
            && x.TripId == tripId);
        }
    }
}
