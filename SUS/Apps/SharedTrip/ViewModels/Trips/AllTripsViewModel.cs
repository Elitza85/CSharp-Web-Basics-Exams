using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SharedTrip.ViewModels.Trips
{
    public class AllTripsViewModel : TripViewModel
    {
        public string Id { get; set; }
        
        public string DepartureTimeFormatted => DepartureTime
            .ToString(CultureInfo.GetCultureInfo("bg-BG"));

        
    }
}
