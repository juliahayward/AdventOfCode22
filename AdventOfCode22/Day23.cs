using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode22
{
    internal class Day23
    {
        /*
         * Should get:
         * Part 1: 4123
         * Part 2: 
         */
        internal static void DoDay23()
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(new FileStream(
                       "C:\\src\\juliahayward\\AdventOfCode22\\RawData\\23.txt", FileMode.Open)))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }

            int i = 0;
            List<string> directions = new List<string>() {"N", "S", "W", "E"};
            // Increasing Y is SOUTH
            var elves = new List<Elf>();
            for (int y = 0; y < lines.Count; y++)
            {
                for (int x = 0; x < lines[0].Length; x++)
                {
                    if (lines[y][x] == '#')
                        elves.Add(new Elf {Id = i++, Location = (x, y)});
                }
            }

            int round;
            for (round = 1; round <= 10; round++)
            {
                IterateElves(elves, directions);
              //  DrawElves(elves);
            }

            var area = (elves.Max(e => e.Location.Item1) - elves.Min(e => e.Location.Item1) + 1) *
                       (elves.Max(e => e.Location.Item2) - elves.Min(e => e.Location.Item2) + 1)
                       - elves.Count;

            Console.WriteLine(area);

            int elvesThatMoved = 1;
            for (round = 11; elvesThatMoved > 0; round++)
            {
                elvesThatMoved = IterateElves(elves, directions);
                // DrawElves(elves);
            }

            Console.WriteLine("No elf moved on round " + (round-1));
        }

        public static void DrawElves(List<Elf> elves)
        {
            for (int y = elves.Min(e => e.Location.Item2); y <= elves.Max(e => e.Location.Item2); y++)
            {
                for (int x = elves.Min(e => e.Location.Item1); x <= elves.Max(e => e.Location.Item1); x++)
                {
                    if (elves.Any(e => e.Location == (x, y)))
                        Console.Write("#");
                    else
                    {
                        Console.Write(".");
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        public static int IterateElves(List<Elf> elves, List<string> directions)
        {
            int elvesThatMoved = 0;
            elves.ForEach(e =>
            {
                e.ProposedMove = null;
                e.AbleToMove = false;
            });
            var elfLocations = elves.Select(e => e.Location).ToHashSet();
            // phase 1
            foreach (var elf in elves)
            {
                if (NoNeighbouringElves(elf, elfLocations))
                    continue;

                foreach (var direction in directions)
                {
                    if (CanProposeMoveInDirection(elf, elfLocations, direction))
                    {
                        elf.ProposedMove = direction;
                        elf.ProposedLocation = Move(elf.Location, direction);
                        elf.AbleToMove = true;
                        break;
                    }
                }
            }

            // phase 2
            var groupedElves = elves.Where(e => e.AbleToMove).GroupBy(e => e.ProposedLocation);
            foreach (var elfGroup in groupedElves)
            {
                // Only move if it's the only elf to propose going here
                if (elfGroup.Count() == 1)
                    foreach (var elf in elfGroup)
                    {
                        if (elf.AbleToMove)
                        {
                            elf.Location = elf.ProposedLocation;
                            elvesThatMoved++;
                        }
                    }
            }

            // rotate directiosn
            var firstDirection = directions.First();
            directions.Remove(firstDirection);
            directions.Add(firstDirection);

            return elvesThatMoved;
        }

        public static (int, int) Move((int, int) start, string direction)
        {
            switch (direction)
            {
                case "N": return (start.Item1, start.Item2 - 1);
                case "S": return (start.Item1, start.Item2 + 1);
                case "E": return (start.Item1 + 1, start.Item2);
                case "W": return (start.Item1 - 1, start.Item2);
                default:
                    throw new ArgumentException();
            }
        }

        public static bool NoNeighbouringElves(Elf elf, HashSet<(int, int)> elfLocations)
        {
            List<(int, int)> placesToLook = new List<(int, int)>
            {
                (elf.Location.Item1 + 1, elf.Location.Item2 - 1),
                (elf.Location.Item1 + 1, elf.Location.Item2),
                (elf.Location.Item1 + 1, elf.Location.Item2 + 1),
                (elf.Location.Item1, elf.Location.Item2 + 1),
                (elf.Location.Item1 - 1, elf.Location.Item2 + 1),
                (elf.Location.Item1 - 1, elf.Location.Item2),
                (elf.Location.Item1 - 1, elf.Location.Item2 - 1),
                (elf.Location.Item1, elf.Location.Item2 - 1),
            };
            return !placesToLook.Any(p => elfLocations.Contains(p));
        }


        public static bool CanProposeMoveInDirection(Elf elf, HashSet<(int, int)> elfLocations, string direction)
        {
            List<(int, int)> placesToLook;
            switch (direction)
            {
                case "N":
                    placesToLook = new List<(int, int)>
                    {
                        (elf.Location.Item1 + 1, elf.Location.Item2 - 1),
                        (elf.Location.Item1, elf.Location.Item2 - 1),
                        (elf.Location.Item1 - 1, elf.Location.Item2 - 1)
                    };
                    return !placesToLook.Any(p => elfLocations.Contains(p));
                case "S":
                    placesToLook = new List<(int, int)>
                    {
                        (elf.Location.Item1 + 1, elf.Location.Item2 + 1),
                        (elf.Location.Item1, elf.Location.Item2 + 1),
                        (elf.Location.Item1 - 1, elf.Location.Item2 + 1)
                    };
                    return !placesToLook.Any(p => elfLocations.Contains(p));
                case "E":
                    placesToLook = new List<(int, int)>
                    {
                        (elf.Location.Item1 + 1, elf.Location.Item2 + 1),
                        (elf.Location.Item1 + 1, elf.Location.Item2),
                        (elf.Location.Item1 + 1, elf.Location.Item2 - 1)
                    };
                    return !placesToLook.Any(p => elfLocations.Contains(p));
                case "W":
                    placesToLook = new List<(int, int)>
                    {
                        (elf.Location.Item1 - 1, elf.Location.Item2 + 1),
                        (elf.Location.Item1 - 1, elf.Location.Item2),
                        (elf.Location.Item1 - 1, elf.Location.Item2 - 1)
                    };
                    return !placesToLook.Any(p => elfLocations.Contains(p));
                default:
                    throw new ArgumentException();
            }
        }
    }

    internal class Elf
    {
        public int Id;
        public (int, int) Location;
        public string ProposedMove;
        public (int, int) ProposedLocation;
        public bool AbleToMove;
    }
}
