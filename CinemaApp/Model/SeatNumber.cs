using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Model
{
    public class SeatNumber
    {
        public SeatNumber(int row, int seatInTheRow)
        {
            RowNumerical = row;
            SeatInTheRow = seatInTheRow;
        }

        public SeatNumber(string row, int seatInTheRow)
        {
            RowNumerical = Row2Numeric(row);
            SeatInTheRow = seatInTheRow;
        }

        public SeatNumber()
        {

        }

        public String Row
        {
            get
            {
                return Numeric2Row(RowNumerical);
            }
        }
        public int RowNumerical { get; }
        public int SeatInTheRow { get; }

        const int LetterAinASCII = 'A'; //65

        /// <summary>
        /// Given an integer, the method returns a letter of alphabet placed on n-th place.
        /// If the integer is higher than length of the alphabet, letter are combined.
        /// E.g.
        /// Numeric2RowZeroBased(0) = A
        /// Numeric2RowZeroBased(26) = AA
        /// </summary>
        /// <param name="zeroBasedNumber">Base 0</param>
        /// <returns></returns>
        private string Numeric2RowZeroBased(int zeroBasedNumber)
        {
            int lettersInAlphabet = 26;
            //int letterAinASCII = (int)'A';
            string letter = "";

            if (zeroBasedNumber < lettersInAlphabet)
            {
                letter = ((char)(zeroBasedNumber + LetterAinASCII)).ToString();
            }
            else
            {
                string letter1, letter2;

                letter1 = Numeric2RowZeroBased((int)(Math.Truncate((decimal)(zeroBasedNumber / lettersInAlphabet - 1))));
                letter2 = Numeric2RowZeroBased(zeroBasedNumber % lettersInAlphabet);

                letter = letter1 + letter2;
            }

            return letter;
        }

        /// <summary>
        /// Given an integer, the method returns a letter of alphabet placed on n-th place.
        /// If the integer is higher than the length of the alphabet, letters are combined.
        /// E.g.
        /// Numeric2Row(1) = A
        /// Numeric2Row(27) = AA
        /// </summary>
        /// <param name="number">Base 1</param>
        /// <returns></returns>
        public string Numeric2Row(int number)
        {
            return Numeric2RowZeroBased(number - 1);
        }

        /// <summary>
        /// Given a row as a string, the method return its numeric value (Base 1)
        /// E.g.
        /// Row2Numeric("A") = 1
        /// Row2Numeric("AA") = 27
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private int Row2Numeric(string row)
        {
            int numeric = 0;

            for (int i = 0; i < row.Length; i++)
            {
                double expBase = 26;
                double expExponent = (double)(row.Length - 1 - i);
                int power = (int)Math.Pow(expBase, expExponent);

                char letter = row[i];
                int letterCinemaFormat = ((int)letter - 65 + 1);

                // A*26^0 + B*26^1 + C*26^2 + ...
                numeric += letterCinemaFormat * power;
            }

            return numeric;
        }
    }
}
