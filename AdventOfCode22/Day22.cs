using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace AdventOfCode22
{
    internal class Day22
    {
        // 1: 60362
        // 2: 74288

        // These contain special-case moves for when we fold the grid up into a cube.
        private static Dictionary<(int, int), ((int, int), string)> _norths =
            new Dictionary<(int, int), ((int, int), string)>();
        private static Dictionary<(int, int), ((int, int), string)> _easts =
            new Dictionary<(int, int), ((int, int), string)>();
        private static Dictionary<(int, int), ((int, int), string)> _souths =
            new Dictionary<(int, int), ((int, int), string)>();
        private static Dictionary<(int, int), ((int, int), string)> _wests =
            new Dictionary<(int, int), ((int, int), string)>();

        internal static void DoDay22()
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(new FileStream(
                       "C:\\src\\juliahayward\\AdventOfCode22\\RawData\\22.txt", FileMode.Open)))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }

            int height = lines.Count - 2;
            int width = lines.Take(height).Max(x => x.Length);
            var grid = new char[width, height];
            // increasing y coordinate is SOUTH
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    grid[j, i] = lines[i][j];
                }
            }

            var directions = lines.Last();


            var location = (lines[0].IndexOf("."), 0);
            var direction = "E";

            while (!string.IsNullOrEmpty(directions))
            {
                if (directions.StartsWith("L") || directions.StartsWith("R"))
                {
                    direction = Rotate(direction, directions[0]);
                    directions = directions.Substring(1);
                }
                else
                {
                    int index = directions.IndexOfAny(new[] {'L', 'R'});
                    int distance = (index >= 0) ? int.Parse(directions.Substring(0, index)) : int.Parse(directions);
                    var next = Move(location, distance, direction, grid);
                    location = next.Item1;
                    direction = next.Item2;
                    if (index >= 0)
                        directions = directions.Substring(index);
                    else directions = "";
                }
            }

            Console.WriteLine(1000 * (location.Item2+1) + 4 * (location.Item1+1) + values[direction]);

            AddCubeWrapRounds();

            directions = lines.Last();
            location = (lines[0].IndexOf("."), 0);
            direction = "E";

            while (!string.IsNullOrEmpty(directions))
            {
                if (directions.StartsWith("L") || directions.StartsWith("R"))
                {
                    direction = Rotate(direction, directions[0]);
                    directions = directions.Substring(1);
                }
                else
                {
                    int index = directions.IndexOfAny(new[] { 'L', 'R' });
                    int distance = (index >= 0) ? int.Parse(directions.Substring(0, index)) : int.Parse(directions);
                    var next = Move(location, distance, direction, grid);
                    location = next.Item1;
                    direction = next.Item2;
                    if (index >= 0)
                        directions = directions.Substring(index);
                    else directions = "";
                }
            }

            Console.WriteLine(1000 * (location.Item2 + 1) + 4 * (location.Item1 + 1) + values[direction]);

        }


        private static Dictionary<string, string> lefts = new Dictionary<string, string>()
            {{"N", "W"}, {"W", "S"}, {"S", "E"}, {"E", "N"}};
        private static Dictionary<string, string> rights = new Dictionary<string, string>()
            {{"N", "E"}, {"E", "S"}, {"S", "W"}, {"W", "N"}};
        private static Dictionary<string, int> values = new Dictionary<string, int>()
            {{"N",3}, {"E", 0}, {"S", 1}, {"W", 2}};

        private static string Rotate(string direction, char leftOrRight)
        {
            if (leftOrRight == 'L')
                return lefts[direction];

            return rights[direction]; 
        }

        private static ((int, int), string) Move((int, int) location, int distance, string direction, char[,] grid)
        {
            for (int i=0; i < distance; i++)
            {
                var nextLocation = Next(location, direction, grid);
                // Don't walk into a wall
                if (grid[nextLocation.Item1.Item1, nextLocation.Item1.Item2] == '#')
                    return (location, direction);
                location = nextLocation.Item1;
                direction = nextLocation.Item2;
            }

            return (location, direction);
        }

        private static ((int, int), string) Next((int, int) location, string direction, char[,] grid)
        {
            switch (direction)
            {
                case "N":
                    if (_norths.ContainsKey(location))
                        return _norths[location];
                    // Simple move north
                    if (IsValidCell((location.Item1, location.Item2 - 1), grid))
                         return ((location.Item1, location.Item2 - 1), direction);
                    // otherwise move South 
                    int targetY1 = location.Item2;
                    for (int y = location.Item2 + 1; y < grid.GetLength(1); y++)
                    {
                        if (IsValidCell((location.Item1, y), grid))
                            targetY1 = y;
                        else
                            break;
                    }

                    return ((location.Item1, targetY1), direction);

                case "S":
                    if (_souths.ContainsKey(location))
                        return _souths[location];
                    if (IsValidCell((location.Item1, location.Item2 + 1), grid))
                        return ((location.Item1, location.Item2 + 1), direction);
                    int targetY2 = location.Item2;
                    for (int y = location.Item2 -1; y >= 0; y--)
                    {
                        if (IsValidCell((location.Item1, y), grid))
                            targetY2 = y;
                        else
                            break;
                    }

                    return ((location.Item1, targetY2), direction);

                case "W":
                    if (_wests.ContainsKey(location))
                        return _wests[location];
                    if (IsValidCell((location.Item1 - 1, location.Item2), grid))
                        return ((location.Item1 - 1, location.Item2), direction);
                    int targetX1 = location.Item2;
                    for (int x = location.Item1 + 1; x < grid.GetLength(0); x++)
                    {
                        if (IsValidCell((x, location.Item2), grid))
                            targetX1 = x;
                        else
                            break;
                    }

                    return ((targetX1, location.Item2), direction);

                case "E":
                    if (_easts.ContainsKey(location))
                        return _easts[location];
                    if (IsValidCell((location.Item1 + 1, location.Item2), grid))
                        return ((location.Item1 + 1, location.Item2), direction);
                    int targetX2 = location.Item2;
                    for (int x = location.Item1 - 1; x >= 0; x--)
                    {
                        if (IsValidCell((x, location.Item2), grid))
                            targetX2 = x;
                        else
                            break;
                    }

                    return ((targetX2, location.Item2), direction);

                default:
                    throw new ArgumentException();
            }
        }

        private static bool IsValidCell((int, int) location, char[,] grid)
        {
            return location.Item1 >= 0 && location.Item1 < grid.GetLength(0) &&
                    location.Item2 >= 0 && location.Item2 < grid.GetLength(1) && 
                    (grid[location.Item1, location.Item2] == '.' || grid[location.Item1, location.Item2] == '#');
        }


        private static void AddCubeWrapRounds()
        {
            for (int i = 0; i < 50; i++)
            {
                // 16 edge
                _norths.Add((50+i, 0), ((0, 150+i), "E"));
                _wests.Add((0, 150+i), ((50+i, 0), "S"));
                // 26 edge
                _norths.Add((100 + i, 0), ((i, 199), "N"));
                _souths.Add((i, 199), ((100+i, 0), "S"));
                // 24 edge
                _easts.Add((149, i), ((99, 149 - i), "W"));
                _easts.Add((99, 149 - i), ((149, i), "W"));
                // 23 edge
                _souths.Add((100 + i, 49), ((99, 50+i), "W"));
                _easts.Add((99, 50+i), ((100 + i, 49), "N"));
                // 46 edge
                _souths.Add((50 + i, 149), ((49, 150 + i), "W"));
                _easts.Add((49, 150 + i), ((50 + i, 149), "N"));
                // 15 edge
                _wests.Add((50, i), ((0, 149 - i), "E"));
                _wests.Add((0, 149 - i), ((50, i), "E"));
                // 35 edge
                _norths.Add((i, 100), ((50, 50 + i), "E"));
                _wests.Add((50, 50 + i), ((i, 100), "S"));
            }
            // For example
            /*for (int i = 0; i < 4; i++)
            {
                // 12 edge
                _norths.Add((8 + i, 0), ((i, 4), "S"));
                _norths.Add((i, 4), ((8 + i, 0), "S"));
                // 13
                _wests.Add((8, i), ((4, 4+i), "S"));
                _norths.Add((4, 4+i), ((8,i), "S"));
                // 16
                _easts.Add((11, i), ((15, 11-i), "W"));
                _easts.Add((15, 11-i), ((11, i), "W"));
                // 46
                _easts.Add((11, 4+i), ((15-i, 8), "S"));
                _norths.Add((15-i, 8), ((11, 4 + i), "W"));
                // 62
                _souths.Add((12+i, 11), ((0, 7-i), "E"));
                _wests.Add((0, 7-i), ((12+i, 11), "N"));
                // 52
                _souths.Add((8+i, 11), ((3-i, 7), "N"));
                _souths.Add((3-i, 7), ((8+i, 11), "N"));
                // 35
                _souths.Add((7, 4 + i), ((8, 11 - i), "E"));
                _wests.Add((8, 11-i), ((7, 4+i), "N"));
            }*/
        }
    }
}
