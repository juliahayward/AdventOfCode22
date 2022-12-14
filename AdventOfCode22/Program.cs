using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting;
using System.Text.Json;
using System.Text.Json.Nodes;
using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode22
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DoDay14();
        }

        private static void DoDay14()
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(new FileStream(
                       "C:\\src\\juliahayward\\AdventOfCode22\\RawData\\14.txt", FileMode.Open)))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }
            // First, find the size of our array
            int maxy = 0, minx=int.MaxValue, maxx = 0; maxy = lines.Count;
            foreach (var line in lines)
            {
                var parts = line.Split(new string[] {"->"}, StringSplitOptions.None);
                foreach (var part in parts)
                {
                    int x = int.Parse(part.Split(',')[0].Trim());
                    int y = int.Parse(part.Split(',')[1].Trim());
                    maxy = Math.Max(y, maxy);
                    maxx = Math.Max(x, maxx);
                    minx = Math.Min(x, minx);
                }
            }
            // TODO: We could employ a shift here to minimise the size of the array
            var cave = new char[maxx+maxy, maxy+4];
            foreach (var line in lines)
            {
                var parts = line.Split(new string[] { "->" }, StringSplitOptions.None);
                for (int i = 0; i < parts.Length - 1; i++)
                {
                    int x = int.Parse(parts[i].Split(',')[0].Trim());
                    int y = int.Parse(parts[i].Split(',')[1].Trim());
                    int nextx = int.Parse(parts[i+1].Split(',')[0].Trim());
                    int nexty = int.Parse(parts[i+1].Split(',')[1].Trim());
                    FillInCaveWithRock(cave, x, y, nextx, nexty);
                }
            }

            // DrawCave(cave, minx, maxx, maxy);

            int numberOfSandThatSettle = 0;
            bool fallingOffSide = false;
            while (cave[500,0] != 'o' && !fallingOffSide)
            {
                (int, int) sandPosition = (500, 0);
                bool settled = false;
                while (!fallingOffSide && !settled)
                {
                    if (sandPosition.Item2 > maxy)
                    {
                        fallingOffSide = true;
                    }
                    else if (cave[sandPosition.Item1, sandPosition.Item2 + 1] == 0)
                        sandPosition.Item2 += 1;
                    else if (cave[sandPosition.Item1 - 1, sandPosition.Item2 + 1] == 0)
                    {
                        sandPosition.Item1 -= 1;
                        sandPosition.Item2 += 1;
                    }
                    else if (cave[sandPosition.Item1 + 1, sandPosition.Item2 + 1] == 0)
                    {
                        sandPosition.Item1 += 1;
                        sandPosition.Item2 += 1;
                    }
                    else
                    {
                        settled = true;
                        cave[sandPosition.Item1, sandPosition.Item2] = 'o';
                        numberOfSandThatSettle++;
                    }
                }
            }

            Console.WriteLine(numberOfSandThatSettle);

            // Now add the floor
            FillInCaveWithRock(cave, minx-5, maxy+2, maxx+5, maxy+2);

            // Similar to the above but nothing falls off the side now
            // and we're adding to the sand already there
            while (cave[500, 0] != 'o')
            {
                (int, int) sandPosition = (500, 0);
                bool settled = false;
                while (!settled)
                {
                    if (cave[sandPosition.Item1, sandPosition.Item2 + 1] == 0 && sandPosition.Item2 < maxy + 1)
                        sandPosition.Item2 += 1;
                    else if (cave[sandPosition.Item1 - 1, sandPosition.Item2 + 1] == 0 && sandPosition.Item2 < maxy + 1)
                    {
                        sandPosition.Item1 -= 1;
                        sandPosition.Item2 += 1;
                    }
                    else if (cave[sandPosition.Item1 + 1, sandPosition.Item2 + 1] == 0 && sandPosition.Item2 < maxy + 1)
                    {
                        sandPosition.Item1 += 1;
                        sandPosition.Item2 += 1;
                    }
                    else
                    {
                        settled = true;
                        cave[sandPosition.Item1, sandPosition.Item2] = 'o';
                        numberOfSandThatSettle++;
                    }
                }
            }
            Console.WriteLine(numberOfSandThatSettle);
        }

        private static void FillInCaveWithRock(char[,] cave, int x1, int y1, int x2, int y2)
        {
            if (x1 == x2)
            {
                for (int y = Math.Min(y1, y2); y <= Math.Max(y1, y2); y++)
                    cave[x1, y] = '#';
            }
            else
            {
                for (int x = Math.Min(x1, x2); x <= Math.Max(x1, x2); x++)
                    cave[x, y1] = '#';
            }
        }

        private static void DrawCave(char[,] cave, int minx, int maxx, int maxy)
        {
            for (int y = 0; y <= maxy; y++)
            {
                for (int x = 0; x <= maxx; x++)
                {
                    Console.Write(cave[x,y]);
                }
                Console.WriteLine("");
            }
        }

    }
}
