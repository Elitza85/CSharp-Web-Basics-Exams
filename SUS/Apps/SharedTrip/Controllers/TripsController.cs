using SharedTrip.Services;
using SharedTrip.ViewModels.Trip;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Globalization;

namespace SharedTrip.Controllers
{
    public class TripsController : Controller
    {
        private readonly ITripsService tripsService;

        public TripsController(ITripsService tripsService)
        {
            this.tripsService = tripsService;
        }
        public HttpResponse All()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }
            var tripView = this.tripsService.AllTrips();
            return this.View(tripView);
        }

        public HttpResponse Add()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }
            return this.View();
        }

        [HttpPost]
        public HttpResponse Add(AddTripInputModel input)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }
            if (string.IsNullOrEmpty(input.StartPoint))
            {
                return this.Error("Start point is required.");
            }

            if (string.IsNullOrEmpty(input.EndPoint))
            {
                return this.Error("End point is required.");
            }

            if(!DateTime.TryParseExact(input.DepartureTime, "dd.MM.yyyy HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return this.Error("Invalid date time format");
            }

            if(input.Seats <2 || input.Seats > 6)
            {
                return this.Error("Seats count should be between 2 and 6");
            }

            if(string.IsNullOrEmpty(input.Description) 
                || input.Description.Length > 80)
            {
                return this.Error("Description is invalid.");
            }
            this.tripsService.AddTrip(input);
            return this.Redirect("/Trips/All");
        }

        public HttpResponse Details(string tripId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }
            var detailsView = this.tripsService.GetTripInfoById(tripId);
            return this.View(detailsView);
        }

        public HttpResponse AddUserToTrip(string tripId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }
            var userId = this.GetUserId();
            if (!this.tripsService.CanUserJoinTrip(userId, tripId))
            {
                return this.Redirect("/Trips/Details?tripId=" + tripId);
            }
            else
            {
                this.tripsService.AddUserToTrip(tripId, userId);
                return this.Redirect("/Trips/All");
            }
        }
    }
}
