using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Model
{
    public class SeatFactory
    {
        public SeatFactory(int rows, int seatsPerRow)
        {
            Rows = rows;
            SeatsPerRow = seatsPerRow;
        }

        public int Rows { get; set; }
        public int SeatsPerRow { get; set; }

        public List<Seat> CreateSeats()
        {
            List<Seat> seats = new List<Seat>();

            for (int row = 1; row <= Rows; row++)
            {
                double price = CalculatePrice(row);

                for (int seat = 1; seat <= SeatsPerRow; seat++)
                {
                    seats.Add(new Seat(row, seat, price));
                }
            }

            return seats;
        }

        private double CalculatePrice(int row)
        {
            int capacity = Rows * SeatsPerRow;
            double price = 10;

            if (capacity > 50)
            {
                if (row <= Rows/2) // if uneven number of rows, the smaller "half" is for $12 -> 5 rows - 2 rows for $12, 3 rows for $10
                {
                    price = 12;
                }
            }

            return price;
        }
    }
}
