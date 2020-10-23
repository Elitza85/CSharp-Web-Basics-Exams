using System;
using System.Collections.Generic;
using System.Text;

namespace SharedTrip.ViewModels.Trips
{
    public class TripViewModel
    {
        public string Id { get; set; }
        public string StartPoint { get; set; }

        public string EndPoint { get; set; }

        public DateTime DepartureTime { get; set; }

        public string TimeFormatted => this.DepartureTime.ToString("s");

        public string ImagePath { get; set; }

        public int Seats { get; set; }

        public string Description { get; set; }

        public int UsedSeats { get; set; }

        public int AvailableSeats => this.Seats - this.UsedSeats;
    }
}
