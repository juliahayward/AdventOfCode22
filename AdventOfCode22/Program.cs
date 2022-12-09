using System;
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
            DoDay9(2);
            DoDay9(10);
            Console.ReadLine();
        }

        private static void DoDay9(int numberOfKnots)
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(new FileStream("C:\\src\\juliahayward\\AdventOfCode22\\RawData\\9.txt",
                       FileMode.Open)))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }

            // Now do the same with 10 points
            var points = new Point[numberOfKnots];
            for (int i = 0; i <= numberOfKnots-1; i++) points[i] = new Point();
            Point head = points[0];
            Point tail = points[numberOfKnots-1];
            var tailPositions = new List<string>(); ;
            tailPositions.Add("0,0");

            foreach (var line in lines)
            {
                var parts = line.Split(' ');
                var direction = parts[0];
                var distance = int.Parse(parts[1]);
                for (var i = 0; i < distance; i++)
                {
                    switch (direction)
                    {
                        case "D":
                            head.Y--;
                            break;
                        case "L":
                            head.X--;
                            break;
                        case "U":
                            head.Y++;
                            break;
                        case "R":
                            head.X++;
                            break;
                    }

                    for (int j=0; j<=numberOfKnots-2; j++)
                        MoveSuccessor(points[j], points[j+1]);

                    tailPositions.Add(tail.X + "," + tail.Y);
                }
            }

            Console.WriteLine(tailPositions.Distinct().Count());
        }

        private static void MoveSuccessor(Point head, Point tail)
        {
            // Move the tail to catch up. In the first part neither coordinate can be more than 2 apart, and only one can be 2 apart as we
            // can't move the head diagonally. However, in the second, a middle segment can move diagonally so we need to check those extra cases.
            var displacement = (head.X - tail.X) + "," + (head.Y - tail.Y);
            switch (displacement)
            {
                case "2,0":
                    tail.X++;
                    break;
                case "2,1":
                case "2,2":
                case "1,2":
                    tail.Y++;
                    tail.X++;
                    break;
                case "0,2":
                    tail.Y++;
                    break;
                case "-1,2":
                case "-2,2":
                case "-2,1":
                    tail.Y++;
                    tail.X--;
                    break;
                case "-2,0":
                    tail.X--;
                    break;
                case "-2,-1":
                case "-2,-2":
                case "-1,-2":
                    tail.Y--;
                    tail.X--;
                    break;
                case "0,-2":
                    tail.Y--;
                    break;
                case "1,-2":
                case "2,-2":
                case "2,-1":
                    tail.Y--;
                    tail.X++;
                    break;
                // If the two are adjacent or coincident, don't move
            }
        }

        private class Point
        {
            public int X;
            public int Y;
        }
    }
}
