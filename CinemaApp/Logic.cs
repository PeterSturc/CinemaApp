using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CinemaApp.Model;

namespace CinemaApp
{
    public class Logic
    {
        public Logic(CinemaRoom cinema)
        {
            Cinema = cinema;
        }

        public CinemaRoom Cinema { get; }

        public void Statistics()
        {
            int purchasedTickets = Cinema.Seats.Count(x => x.Availability == false);
            double occupancy = (double)purchasedTickets / (double)Cinema.Capacity * 100; // in percent
            double currentIncome = Cinema.Seats.Where(x => x.Availability == false).Sum(x => x.Price);
            double potentialIncome = Cinema.Seats.Sum(x => x.Price);

            Console.WriteLine("**********************************");
            Console.WriteLine("        STATISTICS:");
            Console.WriteLine($" Number of purchased tickets: {purchasedTickets}");
            Console.WriteLine($" Occupancy: {occupancy.ToString("F2")}%");
            Console.WriteLine($" Current income: ${currentIncome.ToString()}");
            Console.WriteLine($" Potential income: ${potentialIncome.ToString()}");
            Console.WriteLine("**********************************");
            Console.WriteLine();
        }

        public CinemaRoom BuyTickets()
        {
            Cinema.DisplayMap();

            Console.WriteLine("Write the seat numbers you wish tu purchase. " +
                "For multiple tickets, write comma in between each seat number.");
            Console.WriteLine("E.g. A1 for 1 ticket, or A1, A2 for 2 tickets");
            string seatsInput = Console.ReadLine();

            List<SeatNumber> seatsToPurchase = MineSeatNumbersFromInput(seatsInput.ToUpper());

            if (seatsToPurchase.Count > 0)
            {
                List<SeatNumber> availableTickets = seatsToPurchase.Where(x => CheckSeatAvailability(x)).ToList();
                List<SeatNumber> unavailableTickets = seatsToPurchase.Where(x => !CheckSeatAvailability(x)).ToList();

                if (availableTickets.Count > 0)
                {
                    string availableSeatNumbers = "";
                    double totalPrice = 0;

                    foreach (SeatNumber seatNumber in availableTickets)
                    {
                        availableSeatNumbers += $"{seatNumber.Row}{seatNumber.SeatInTheRow} ";
                        totalPrice += Cinema.Seats
                                        .Where(x => (x.SeatNumber.Row == seatNumber.Row) && (x.SeatNumber.SeatInTheRow == seatNumber.SeatInTheRow))
                                        .Select(x => x.Price)
                                        .First();
                    }

                    if (unavailableTickets.Count > 0)
                    {
                        string unavailableSeatNumbers = "";
                        foreach (SeatNumber seatNumber in unavailableTickets)
                        {
                            unavailableSeatNumbers += $"{seatNumber.Row}{seatNumber.SeatInTheRow} ";
                        }
                        Console.WriteLine($"Following seats are not available: {unavailableSeatNumbers}");
                    }
                    Console.WriteLine($"Following seats are available: {availableSeatNumbers}");
                    Console.WriteLine($"Total price is ${totalPrice}. Do you wish to purchase? [Y/N]");
                    var response = Console.ReadKey(true);
                    Console.WriteLine();

                    switch (response.Key)
                    {
                        case ConsoleKey.Y:
                            MakeTransaction(availableTickets);
                            break;
                        default:
                            Console.WriteLine("Purchase cancelled.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Unfortunately the tickets you chose are already reserved.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input, seats do not exist in this cinema.");
            }

            return Cinema;
        }

        private void MakeTransaction(List<SeatNumber> availableTickets)
        {
            foreach (SeatNumber ticket in availableTickets)
            {
                string row = ticket.Row;
                int seat = ticket.SeatInTheRow;

                Cinema.Seats.Where(x => (x.SeatNumber.Row == row) && (x.SeatNumber.SeatInTheRow == seat)).First().Availability = false;
            }

            Console.WriteLine("Ticket(s) purchased.");
            Console.WriteLine();
        }

        private bool CheckSeatAvailability(SeatNumber seat)
        {
            return Cinema.Seats
                    .Where(x => (x.SeatNumber.Row == seat.Row) && (x.SeatNumber.SeatInTheRow == seat.SeatInTheRow))
                    .Select(x => x.Availability)
                    .First();
        }

        private List<SeatNumber> MineSeatNumbersFromInput(string userInputUpperCase)
        {
            List<SeatNumber> export = new List<SeatNumber>();

            string[] inputsSeparatedByComma = userInputUpperCase.Split(',');
            foreach (string input in inputsSeparatedByComma)
            {
                Regex regexLetters = new Regex(@"([A-Z])+");
                Regex regexNumeric = new Regex(@"([0-9])+");

                Match row = regexLetters.Match(input);
                Match seat = regexNumeric.Match(input);

                if (row.Success)
                {
                    if (seat.Success)
                    {
                        bool seatExistsInCinema = Cinema.Seats
                            .Any(x => (x.SeatNumber.Row == row.Value) && 
                                      (x.SeatNumber.SeatInTheRow.ToString() == seat.Value) );

                        // if one seat is written more than once in the same command
                        bool seatAlreadyAdded = export.Any(x => (x.Row == row.Value) &&
                                                                (x.SeatInTheRow.ToString() == seat.Value));

                        if (seatExistsInCinema && !seatAlreadyAdded)
                        {
                            export.Add(new SeatNumber(row.Value, int.Parse(seat.Value)));
                        }
                    }
                }
            }
            return export;
        }
    }
}
