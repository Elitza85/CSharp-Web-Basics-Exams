using SharedTrip.ViewModels.Trips;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedTrip.Services
{
    public interface ITripsService
    {
        void AddTrip(AddTripInputModel input);

        IEnumerable<AllTripsViewModel> All();

        TripViewModel Details(string tripId);

        bool HasUserJoinedTrip(string userId, string tripId);

        void AddUserToTrip(string userId, string tripId);
    }
}
