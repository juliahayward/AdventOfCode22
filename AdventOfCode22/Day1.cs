using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode22
{
    internal class Day1
    {
        internal static void DoDay1()
        {
            int runningTotal = 0;
            var reader = new StreamReader(new FileStream("C:\\src\\juliahayward\\AdventOfCode22\\RawData\\1.txt", FileMode.Open));
            var elvesCalories = new List<int>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line == "")
                {
                    elvesCalories.Add(runningTotal);
                    runningTotal = 0;
                }
                else
                {
                    runningTotal += int.Parse(line);
                }
            }
            elvesCalories.Add(runningTotal);
            Console.WriteLine(elvesCalories.Max());

            var sorted = elvesCalories.OrderByDescending(x => x);
            Console.WriteLine(sorted.Take(3).Sum());
        }
    }
}
