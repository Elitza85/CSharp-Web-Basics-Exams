using System;
using System.Collections.Generic;
using System.Text;

namespace SharedTrip.ViewModels.Trip
{
    public class TripDetailsViewModel : TripViewModel
    {
        public string Description { get; set; }

        public string ImagePath { get; set; }

        public string DetailsTimeFormatted => this.DepartureTime.ToString("s");
    }
}
