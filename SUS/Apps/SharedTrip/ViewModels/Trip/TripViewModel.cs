using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SharedTrip.ViewModels.Trip
{
    public class TripViewModel
    {
        public string Id { get; set; }
        public string StartPoint { get; set; }

        public string EndPoint { get; set; }

        public DateTime DepartureTime { get; set; }

        public string TimeFormatted => this.DepartureTime.ToString(CultureInfo.GetCultureInfo("bg-BG"));

        public int Seats { get; set; }

        public int UsedSeats { get; set; }
        public int AvailableSeats => this.Seats - this.UsedSeats;
    }
}
