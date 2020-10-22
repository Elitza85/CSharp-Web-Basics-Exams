using SharedTrip.ViewModels.Trip;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedTrip.Services
{
    public interface ITripsService
    {
        void AddTrip(AddTripInputModel input);
        IEnumerable<TripViewModel> AllTrips();

        TripDetailsViewModel GetTripInfoById(string id);

        void AddUserToTrip(string tripId, string userId);

        bool CanUserJoinTrip(string userId, string tripId);
    }
}
