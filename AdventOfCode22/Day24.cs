using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AdventOfCode22
{
    internal class Day24
    {
        internal static int _boardWidth, _boardHeight;

        internal static void DoDay24()
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(new FileStream(
                       "C:\\src\\juliahayward\\AdventOfCode22\\RawData\\24.txt", FileMode.Open)))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }

            var blizzards = new List<Blizzard>();
            var blizzardChars = new char[] {'>', '<', 'v', '^'};

            _boardHeight = lines.Count - 2;
            _boardWidth = lines.Max(x => x.Length) - 2;
            // increasing y coordinate is SOUTH
            for (int i = 1; i <= _boardHeight; i++)
            {
                for (int j = 1; j < lines[i].Length - 1; j++)
                {
                    if (blizzardChars.Contains(lines[i][j]))
                        blizzards.Add(new Blizzard() {initialX = j - 1, initialY = i - 1, Direction = lines[i][j]});
                }
            }

            var yourLocation = (0, -1);
            // This is the last location on the board which is available - we'll assume one more step to get off the board
            var targetLocation = (_boardWidth - 1, _boardHeight - 1);
            var outwardTime = TimeAtTargetLocation(yourLocation, targetLocation, blizzards, 0);

            Console.WriteLine("First goal achieved at time " + (outwardTime + 1));

            // Now go back...
            var returnTime = TimeAtTargetLocation((_boardWidth - 1, _boardHeight), (0, 0), blizzards, outwardTime + 1);

            Console.WriteLine("Returned to start at " + (returnTime + 1));

            var secondOutwardTime = TimeAtTargetLocation(yourLocation, targetLocation, blizzards, returnTime + 1);

            Console.WriteLine("Goal achieved again at time " + (secondOutwardTime + 1));

        }

        public static int TimeAtTargetLocation((int, int) yourLocation, (int, int) targetLocation, List<Blizzard> blizzards, int startTime)
        {
            var validLocations = new Dictionary<int, List<(int, int)>>();
            validLocations[startTime] = new List<(int, int)>() {yourLocation};
            var time = startTime + 1;
            while (true)
            {
                validLocations[time] = new List<(int, int)>();

                foreach (var location in validLocations[time - 1])
                {
                    if (NoBlizzardAt(location, time, blizzards))
                        validLocations[time].Add(location);
                    if (location.Item1 > 0 && location.Item2 < _boardHeight && NoBlizzardAt((location.Item1 - 1, location.Item2), time, blizzards))
                        validLocations[time].Add((location.Item1 - 1, location.Item2));
                    if (location.Item2 > 0 && NoBlizzardAt((location.Item1, location.Item2 - 1), time, blizzards))
                        validLocations[time].Add((location.Item1, location.Item2 - 1));
                    if (location.Item1 < _boardWidth - 1 && location.Item2 >= 0 &&
                        NoBlizzardAt((location.Item1 + 1, location.Item2), time,
                            blizzards)) // Force yourself off the initial position southwards!
                        validLocations[time].Add((location.Item1 + 1, location.Item2));
                    if (location.Item2 < _boardHeight - 1 &&
                        NoBlizzardAt((location.Item1, location.Item2 + 1), time, blizzards))
                        validLocations[time].Add((location.Item1, location.Item2 + 1));
                }

                validLocations[time] = validLocations[time].Distinct().ToList();

                for (int i = -1; i <= _boardHeight; i++)
                {
                    for (int j = -1; j <= _boardWidth; j++)
                    {
                        if (validLocations[time].Contains((j, i)))
                            Console.Write("*");
                        else if (NoBlizzardAt((j, i), time, blizzards))
                            Console.Write(".");
                        else Console.Write("v");
                    }

                    Console.WriteLine();
                }

                Console.WriteLine("there are " + validLocations[time].Count() + " locations at time " + time);

                if (validLocations[time].Contains(targetLocation))
                    return time;

                time++;
            }
        }


        internal static bool NoBlizzardAt((int, int) location, int time, List<Blizzard> blizzards)
        {
            return (!blizzards.Any(b => b.X(time) == location.Item1 && b.Y(time) == location.Item2));
        }

        internal class Blizzard
        {
            public int initialX, initialY;
            public char Direction;

            public int X(int time)
            {
                int modtime = time % _boardWidth;
                return (initialX 
                       + ((Direction == '>') ? time : 0)
                       + ((Direction == '<') ? _boardWidth - modtime : 0)) % _boardWidth;  // watch out for % with negative numbers!!
            }

            public int Y(int time)
            {
                int modtime = time % _boardHeight;
                return (initialY
                    + ((Direction == 'v') ? time : 0)
                    + ((Direction == '^') ? _boardHeight - modtime : 0)) % _boardHeight;
            }
        }
    }
}
