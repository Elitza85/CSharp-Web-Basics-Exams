using SharedTrip.Services;
using SharedTrip.ViewModels.Trips;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

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
            if (!IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var tripsView = this.tripsService.All();
            return this.View(tripsView);
        } 

        public HttpResponse Add()
        {
            if (!IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Add(AddTripInputModel input)
        {
            if (!IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (string.IsNullOrEmpty(input.StartPoint))
            {
                return this.Error("Start point of the trip is required.");
            }

            if (string.IsNullOrEmpty(input.EndPoint))
            {
                return this.Error("End point of the trip is required.");
            }

            if(!DateTime.TryParseExact(input.DepartureTime, "dd.MM.yyyy HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return this.Error("Invalid date format, please use -- dd.MM.yyyy HH:mm --");
            }

            if(input.Seats < 2 || input.Seats > 6)
            {
                return this.Error("Seats number is required and should be in range 2-6.");
            }

            if(string.IsNullOrEmpty(input.Description)
                || input.Description.Length > 80)
            {
                return this.Error("Trip description is required and should be below 80 characters.");
            }

            this.tripsService.AddTrip(input);

            return this.Redirect("/Trips/All");
        }

        public HttpResponse Details(string tripId)
        {
            if (!IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var tripView= this.tripsService.Details(tripId);
            return this.View(tripView);
        }

        public HttpResponse AddUserToTrip(string tripId)
        {
            if (!IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var userId = this.GetUserId();
            if (this.tripsService.HasUserJoinedTrip(userId, tripId))
            {
                return this.Redirect("/Trips/Details?tripId=" + tripId);
            }
            this.tripsService.AddUserToTrip(userId, tripId);

            return this.Redirect("/Trips/All");
        }
    }
}
