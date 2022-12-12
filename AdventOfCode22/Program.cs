using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.CompilerServices;

namespace AdventOfCode22
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DoDay12();
        }

        private static void DoDay12()
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(new FileStream("C:\\src\\juliahayward\\AdventOfCode22\\RawData\\12.txt",
                       FileMode.Open)))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }
            int width = lines[0].Length;
            int height = lines.Count;
            var cells = new List<Cell>();
            for (int row = 0; row < height; row++)
                for (int col = 0; col < width; col++)
            {
                cells.Add(new Cell() { Height = lines[row][col], Visited = false, DistanceFromStart = int.MaxValue,
                X = col, Y = row});
            }

            // To assist in part 2, we'll consider routes in reverse, starting at E and ending at S

            var currentCell = cells.Single(x => x.Height == 'E');
            currentCell.DistanceFromStart = 0;
            currentCell.Height = 'z';   // this is its actual height, not the end marker

            var targetCell = cells.Single(x => x.Height == 'S');
            targetCell.Height = 'a';    // this also 

            while (currentCell != null)
            {
                var north = cells.FirstOrDefault(x => x.X == currentCell.X + 1 && x.Y == currentCell.Y);
                var east = cells.FirstOrDefault(x => x.X == currentCell.X && x.Y == currentCell.Y + 1);
                var south = cells.FirstOrDefault(x => x.X == currentCell.X - 1 && x.Y == currentCell.Y);
                var west = cells.FirstOrDefault(x => x.X == currentCell.X && x.Y == currentCell.Y - 1);
                foreach (var neighbour in new[] {north, east, south, west})
                {
                    if (neighbour != null && !neighbour.Visited && currentCell.CanMoveTo(neighbour))
                    {
                        neighbour.DistanceFromStart = Math.Min(neighbour.DistanceFromStart, currentCell.DistanceFromStart + 1);
                    }
                }
                currentCell.Visited = true;
                // Careful - if the only remaining ones are max distance away, then they are unreachable
                currentCell = cells.Where(x => !x.Visited && x.DistanceFromStart < int.MaxValue)
                    .OrderBy(x => x.DistanceFromStart).FirstOrDefault();
            }

            Console.WriteLine(targetCell.DistanceFromStart);

            // Find distances to all potential starts
            Console.WriteLine(cells.Where(x => x.Height == 'a').Min(x => x.DistanceFromStart));
        }

        public class Cell
        {
            public int X, Y;
            public char Height;
            public int DistanceFromStart;
            public bool Visited;

            public bool CanMoveTo(Cell otherCell)
            {
                
                return (Height - otherCell.Height <= 1);
            }
        }

    }
}
