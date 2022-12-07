using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode22
{
    internal class Day6
    {
        // DoDay6(4);
        // DoDay6(14);

        private static void DoDay6(int numberOfDistinctToFind)
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(new FileStream("C:\\src\\juliahayward\\AdventOfCode22\\RawData\\6.txt",
                       FileMode.Open)))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }
            // Should only be one line
            var chars = lines[0].ToCharArray();
            for (int i = 0; i < chars.Length - numberOfDistinctToFind + 1; i++)
            {
                if (chars.Skip(i).Take(numberOfDistinctToFind).Distinct().Count() == numberOfDistinctToFind)
                {
                    Console.WriteLine(i + numberOfDistinctToFind);
                    break;
                }
            }
        }
    }
}
