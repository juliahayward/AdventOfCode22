using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode22
{
    /*
     * DateTime start = DateTime.Now;
          //  Day17.DoDay17(2022);
            Console.WriteLine((DateTime.Now - start).TotalSeconds);

            // Part 2 - logging the initial part of the problem, the pattern repeats every 352 * 5 * 10091 = 17,760,160 blocks, 
            // after which the height has increased 27,619,067. So the value at 1E12 is (56303 * 27619067) + (value at 49,711,520 = 77,307,084)
            // where 1E12 = 56303 * 17,760,160 + 49,711,520
            Day17.DoDay17(49711520);
            Console.WriteLine((DateTime.Now - start).TotalSeconds);
            //Day17.DoDay17(1000000000000);
            Console.ReadLine();

    */
    
    internal class Day17
    {
        internal static void DoDay17(long totalRocks)
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(new FileStream(
                       "C:\\src\\juliahayward\\AdventOfCode22\\RawData\\17.txt", FileMode.Open)))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
                // Should only be 1
            }
            var jets = lines[0].ToCharArray();

            var shapes = new List<List<(int, int)>>();
            shapes.Add(new List<(int, int)>() { (0, 0), (1, 0), (2, 0), (3, 0) });
            shapes.Add(new List<(int, int)>() { (0, 1), (1, 1), (2, 1), (1, 0), (1, 2) });
            shapes.Add(new List<(int, int)>() { (0, 0), (1, 0), (2, 0), (2, 1), (2, 2) });
            shapes.Add(new List<(int, int)>() { (0, 0), (0, 1), (0, 2), (0, 3) });
            shapes.Add(new List<(int, int)>() { (0, 0), (1, 0), (0, 1), (1, 1) });
            var shapeMaxX = new List<int>() { 3, 2, 2, 0, 1 };

            var staticRocks = new List<(int, long)>();
            long highestStaticRockY = - 1;
            long previous = 0;
            int jetIndex = 0;
            int jetsCount = jets.Length;
            var possiblePeriodStarts = new List<long>();

            // Possible repeats after 10091 (length of list) * 5 (length of shape list) 

            for (long rock = 0; rock < totalRocks; rock++)
            {
                int shapeIndex = (int)(rock % 5);
                int rockPositionX = 2;
                long rockPositionY = highestStaticRockY + 4;
                while (true)
                {
                    // Make the horizontal move, if possible
                    var jetDirection = (jets[jetIndex] == '<') ? -1 : 1;
                    jetIndex++;
                    if (jetIndex == jetsCount) jetIndex = 0;  // seems much faster than mod!

                    bool canMoveSideways = true;
                    // hits left wall
                    if (rockPositionX + jetDirection < 0) canMoveSideways = false;
                    // hits right wall
                    if (rockPositionX + shapeMaxX[shapeIndex] + jetDirection > 6) canMoveSideways = false;
                    foreach (var rockPiece in shapes[shapeIndex])
                    {
                        // hits piece to the side
                        if (staticRocks.Contains((rockPositionX + rockPiece.Item1 + jetDirection, rockPositionY + rockPiece.Item2)))
                            canMoveSideways = false;
                    }
                    if (canMoveSideways)
                        rockPositionX += jetDirection;

                    // Make the downward move, if possible
                    bool canMoveDownwards = true;
                    // hits ground
                    if (rockPositionY <= 0) canMoveDownwards = false;
                    foreach (var rockPiece in shapes[shapeIndex])
                    {
                        // hits piece below
                        if (staticRocks.Contains((rockPositionX + rockPiece.Item1, rockPositionY + rockPiece.Item2 - 1)))
                            canMoveDownwards = false;         
                    }
                    if (canMoveDownwards)
                        rockPositionY -= 1;
                    else
                    {
                        var newStaticRocks = shapes[shapeIndex].Select(s => (rockPositionX + s.Item1, rockPositionY + s.Item2));
                        staticRocks.AddRange(newStaticRocks);
                        highestStaticRockY = Math.Max(highestStaticRockY, newStaticRocks.Max(s => s.Item2));
                        break;
                    }
                }

                if (rock % (jetsCount * 5) == 0)
                    possiblePeriodStarts.Add(rockPositionX);
                if (rock % (jetsCount * 5 * 352) == 0)
                {
                    Console.WriteLine(rock + " ---> " + (highestStaticRockY + 1) + "   " + (highestStaticRockY + 1 - previous));
                    previous = highestStaticRockY + 1;
                }

                // Draw the board
                /* for (int i=highestStaticRockY; i >= 0; i--)
                {
                    Console.Write("|");
                    for (int x=0; x<7; x++)
                    {
                        if (staticRocks.Contains((x, i))) Console.Write("#");
                        else
                            Console.Write('.');
                    }
                    Console.WriteLine("|");
                }
                Console.WriteLine("+-------+");
                Console.ReadLine();*/
                // Clear out stuff that (ought to be) unreachable
                staticRocks.RemoveAll(x => x.Item2 < highestStaticRockY - 50);
            }
            Console.WriteLine(highestStaticRockY + 1);
            //Console.WriteLine(possiblePeriodStarts.Count);
            // Periodicity doesn't necessarily start at 1
            /*for (int i = 1; i < 1000; i++)
            {
                bool possible = true;
                for (int j = i + 100; j < possiblePeriodStarts.Count; j++)
                {
                    if (possiblePeriodStarts.ElementAt(j) != possiblePeriodStarts.ElementAt(j - i))
                    {
                        possible = false;
                        break;
                    }
                }
                if (possible) Console.WriteLine("***" + i);
            }*/
        }
    }
}
