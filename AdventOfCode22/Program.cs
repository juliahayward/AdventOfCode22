using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Schema;

namespace AdventOfCode22
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DoDay2();
            Console.ReadLine();
        }

        private static void DoDay1()
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

            var sorted = elvesCalories.OrderByDescending(x=>x);
            Console.WriteLine(sorted.Take(3).Sum());
        }

        private static void DoDay2()
        {
            int runningTotal = 0, encryptedRunningTotal = 0;
            var reader = new StreamReader(new FileStream("C:\\src\\juliahayward\\AdventOfCode22\\RawData\\2.txt", FileMode.Open));
            var scores = new Dictionary<string, int>
            {
                { "A X", 4 }, // Rocks I get 1 for rock, 2 for paper, 3 for scissors, plus 3 for draw, 6 for win
                { "A Y", 8 },
                { "A Z", 3 },
                { "B X", 1 },
                { "B Y", 5 }, // Papers
                { "B Z", 9 },
                { "C X", 7 },
                { "C Y", 2 },
                { "C Z", 6 } // Scissors
            };
            var encryptedScores = new Dictionary<string, int>
            {
                { "A X", 3 }, // He plays Rock, I need to lose
                { "A Y", 4 },
                { "A Z", 8 },
                { "B X", 1 },
                { "B Y", 5 }, // He plays Paper, I need to draw
                { "B Z", 9 },
                { "C X", 2 },
                { "C Y", 6 },
                { "C Z", 7 } // He plays Scissors, I need to win
            };

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine().Substring(0,3);
                runningTotal += scores[line];
                encryptedRunningTotal += encryptedScores[line];
            }

            Console.WriteLine(runningTotal);
            Console.WriteLine(encryptedRunningTotal);
        }
    }
}
