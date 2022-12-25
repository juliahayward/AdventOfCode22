using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode22
{
    internal class Day25
    {
        internal static void DoDay25()
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(new FileStream(
                       "C:\\src\\juliahayward\\AdventOfCode22\\RawData\\25.txt", FileMode.Open)))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }


            long total = 0;
            foreach (var line in lines)
            {
                total += FromSnafu(line);
            }
            Console.WriteLine(ToSnafu(total));
        }

        public static long FromSnafu(string snafu)
        {
            long total = 0;
            long power = 1;
            for (int i=snafu.Length-1; i>=0; i--)
            {
                int digit = SnafuDigitToInt[snafu[i]];
                total += power * digit;
                power *= 5;
            }
            return total;
        }

        public static string ToSnafu(long number)
        {
            string snafu = "";
            while (number > 0)
            {
                long lastDigit = number % 5;
                char nextChar = IntToSnafuDigitOfSameModulus[lastDigit];
                snafu = nextChar + snafu;
                number = (number - SnafuDigitToInt[nextChar]) / 5;
            }
            return snafu;
        }

        public static Dictionary<char, int> SnafuDigitToInt = new Dictionary<char, int>() { { '=', -2 }, { '-', -1 }, { '0', 0 }, { '1', 1 }, { '2', 2 } };

        public static Dictionary<long, char> IntToSnafuDigitOfSameModulus = new Dictionary<long, char>() { { 3, '=' }, { 4, '-' }, { 0, '0' }, { 1, '1' }, { 2, '2' } };
    }
}
