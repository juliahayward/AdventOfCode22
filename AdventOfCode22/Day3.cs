using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode22
{
    internal class Day3
    {
        internal static void DoDay3A()
        {
            int totalPriority = 0;
            var reader = new StreamReader(new FileStream("C:\\src\\juliahayward\\AdventOfCode22\\RawData\\3.txt",
                FileMode.Open));
            List<char> TypesThatAreDuplicated = new List<char>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var firstCompartment = line.Substring(0, line.Length / 2).ToCharArray().OrderBy(x => x).ToList();
                var secondCompartment = line.Substring(line.Length / 2).ToCharArray().OrderBy(x => x).ToList();

                while (firstCompartment.Any() && secondCompartment.Any())
                {
                    if (firstCompartment.First() == secondCompartment.First())
                    {
                        if (!TypesThatAreDuplicated.Contains(firstCompartment.First()))
                            TypesThatAreDuplicated.Add(firstCompartment.First());
                        firstCompartment.RemoveAt(0);
                        secondCompartment.RemoveAt(0);
                    }
                    else if (firstCompartment.First() <= secondCompartment.First())
                    {
                        firstCompartment.RemoveAt(0);
                    }
                    else
                    {
                        secondCompartment.RemoveAt(0);
                    }
                }

                totalPriority += TypesThatAreDuplicated.Sum(x => Priority(x));
                TypesThatAreDuplicated = new List<char>();
            }

            Console.WriteLine(totalPriority);
            reader.Close();
        }

        internal static void DoDay3B()
        {
            int totalPriority = 0;
            var reader = new StreamReader(new FileStream("C:\\src\\juliahayward\\AdventOfCode22\\RawData\\3.txt",
                FileMode.Open));
            List<char> TypesThatAreDuplicated = new List<char>();
            while (!reader.EndOfStream)
            {
                var line1 = reader.ReadLine();
                var line2 = reader.ReadLine();
                var line3 = reader.ReadLine();
                var firstElfContents = line1.ToCharArray().OrderBy(x => x).ToList();
                var secondElfContent = line2.ToCharArray().OrderBy(x => x).ToList();
                var thirdElfContent = line3.ToCharArray().OrderBy(x => x).ToList();

                while (firstElfContents.Any() && secondElfContent.Any() && thirdElfContent.Any())
                {
                    // Only one badge type per group
                    if (firstElfContents.First() == secondElfContent.First() && firstElfContents.First() == thirdElfContent.First())
                    {
                        totalPriority += Priority(firstElfContents.First());
                        firstElfContents.RemoveAt(0);
                        secondElfContent.RemoveAt(0);
                        thirdElfContent.RemoveAt(0);
                        break;
                    }
                    else if (firstElfContents.First() <= secondElfContent.First() && firstElfContents.First() <= thirdElfContent.First())
                    {
                        firstElfContents.RemoveAt(0);
                    }
                    else if (secondElfContent.First() <= thirdElfContent.First())
                    {
                        secondElfContent.RemoveAt(0);
                    }
                    else
                    {
                        thirdElfContent.RemoveAt(0);
                    }
                }
            }

            Console.WriteLine(totalPriority);
        }

        private static int Priority(char c)
        {
            if ('a' <= c && c <= 'z')
                return c - 'a' + 1;
            return c - 'A' + 27;
        }

    }
}
