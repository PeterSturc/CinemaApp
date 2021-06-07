using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Model
{
    public class Seat
    {
        public Seat(int row, int seatInTheRow, double price)
        {
            SeatNumber = new SeatNumber(row, seatInTheRow);
            Price = price;
            Availability = true;
        }

        public Seat()
        {

        }

        public SeatNumber SeatNumber { get; }
        public double Price { get; }
        public bool Availability { get; set; }
    }
}
