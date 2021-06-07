using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CinemaApp.Model;

namespace CinemaApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Welcome();
            CinemaRoom cinema = CreateNewCinema();
            bool customer = AreYouCustomer();
            WhatsNext(cinema, customer);
        }

        private static bool AreYouCustomer()
        {
            Console.WriteLine();
            Console.WriteLine("Are you a customer? [Y/N] Customers cannot see the cinema statistics.");
            var response = Console.ReadKey(true);

            if (response.Key == ConsoleKey.Y)
            {
                return true;
            }
            return false;
        }

        private static void Welcome()
        {
            Console.WriteLine("Welcome to the Cinema App");
            Console.WriteLine("Let's start by creating a cinema room.");
            Console.WriteLine();
        }

        private static CinemaRoom CreateNewCinema()
        {
            Console.WriteLine("How many rows are there in the cinema?");
            int rows = InputNumber();

            Console.WriteLine("How many seats are there per row?");
            int seatsPerRow = InputNumber();

            CinemaRoom cinema = new CinemaRoom(rows, seatsPerRow);

            Console.WriteLine($"There are {rows} rows; each row has {seatsPerRow} seats. " +
                $"In total there are {cinema.Capacity} seats in your cinema.");

            return cinema;
        }

        /// <summary>
        /// Crossroad for when all processes are finished.
        /// </summary>
        /// <param name="cinema"></param>
        private static void WhatsNext(CinemaRoom cinema, bool customer)
        {
            Logic logic = new Logic(cinema);

            Console.WriteLine();
            Console.WriteLine("What do you want to do next?");
            Console.WriteLine("[T] Tickets");
            if (customer == false)
            {
                Console.WriteLine("[S] Statistics");
            }
            Console.WriteLine("[M] Seat Map");
            Console.WriteLine("[L] Log out / Change user");
            Console.WriteLine("[Esc] Exit.");
            Console.WriteLine();

            var response = Console.ReadKey(true);
            Console.WriteLine();

            switch (response.Key)
            {
                case ConsoleKey.T:
                    logic.BuyTickets();
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey(true);
                    WhatsNext(cinema, customer);
                    break;
                case ConsoleKey.S:
                    if (customer)
                    {
                        Console.WriteLine("Unknown command, try again.");
                        WhatsNext(cinema, customer);
                        break;
                    }
                    logic.Statistics();
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey(true);
                    WhatsNext(cinema, customer);
                    break;
                case ConsoleKey.M:
                    cinema.DisplayMap();
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey(true);
                    WhatsNext(cinema, customer);
                    break;
                case ConsoleKey.L:
                    bool newCustomer = AreYouCustomer();
                    WhatsNext(cinema, newCustomer);
                    break;
                case ConsoleKey.Escape:
                    // the end of the app
                    break;
                default:
                    Console.WriteLine("Unknown command, try again.");
                    WhatsNext(cinema, customer);
                    break;
            }
        }

        /// <summary>
        /// Reads the user's input and converts it to integer.
        /// </summary>
        /// <returns></returns>
        private static int InputNumber()
        {
            string numberAsString = Console.ReadLine();

            if (!int.TryParse(numberAsString, out int number))
            {
                Console.WriteLine("Invalid input. Please type an integer number, e.g. 5");
                number = InputNumber();
            }
            else if (number < 1)
            {
                Console.WriteLine("Invalid input. Please type an integer larger or equal to 1.");
                number = InputNumber();
            }

            return number;
        }
    }
}
