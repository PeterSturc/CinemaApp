using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Model
{
    public class CinemaRoom
    {
        public CinemaRoom(int rows, int seatsPerRow)
        {
            Rows = rows;
            SeatsPerRow = seatsPerRow;
            SeatFactory factory = new SeatFactory(rows, seatsPerRow);
            Seats = factory.CreateSeats();
        }
        private int myVar;

        public int MyProperty
        {
            get { return myVar; }
            set { myVar = value; }
        }

        public List<Seat> Seats { get; set; }
        public int Rows { get;  }
        public int SeatsPerRow { get; }
        public int Capacity
        {
            get { return Rows * SeatsPerRow; }
        }

        /// <summary>
        /// Displays the Cinema map - rows are A, B, C, ..., seats 1, 2, 3, ...
        /// </summary>
        public void DisplayMap()
        {
            Console.WriteLine("**********************************");
            Console.WriteLine("        SEAT MAP:");
            Console.WriteLine("A - available, R - reserved");
            Console.WriteLine();

            StringBuilder sb = new StringBuilder();
            SeatNumber sn = new SeatNumber();

            // Header - Rows>26 handles the width of the first column depending on how many rows there are
            string headerRow = Rows > 26 ? "|    ||" : "|   ||";
            string tableRow = Rows > 26 ? "|row ||" : "|row||";

            // header row example: |   || 1 | 2 | 3 |
            // table row example:  |row||===|===|===|
            for (int seat = 1; seat <= SeatsPerRow; seat++)
            {
                headerRow += $" {seat} |";
                tableRow += seat.ToString().Length == 2
                    ? "====|"
                    : "===|";
            }
            sb.AppendLine(headerRow);
            sb.AppendLine(tableRow);

            // Body
            // availability row example: | 1 || A | R | A|
            for (int row = 1; row <= Rows; row++)
            {
                string availabilityRow = Rows > 26 //handling the width of the first column
                    ? row > 26
                        ? $"| {sn.Numeric2Row(row)} ||" 
                        : $"|  {sn.Numeric2Row(row)} ||"
                    : $"| {sn.Numeric2Row(row)} ||";


                for (int seat = 1; seat <= SeatsPerRow; seat++)
                {
                    string availability = Seats
                                          .Where(x => x.SeatNumber.RowNumerical == row)
                                          .Where(x => x.SeatNumber.SeatInTheRow == seat)
                                          .Select(x => x.Availability)
                                          .First() == true ? "A" : "R";

                    availabilityRow += seat.ToString().Length == 2
                        ? $"  {availability} |"
                        : $" {availability} |";
                }
                sb.AppendLine(availabilityRow);
            }

            Console.WriteLine(sb);
            Console.WriteLine("**********************************");
        }
    }
}
