using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode22
{
    internal class Day15
    {
        internal static void DoDay15()
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(new FileStream(
                       "C:\\src\\juliahayward\\AdventOfCode22\\RawData\\15.txt", FileMode.Open)))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }

            DateTime start = DateTime.Now;

            var sensors = new List<SensorBeaconPair>();
            foreach (var line in lines)
            {
                var parts = line.Split(new[] { ' ', '=', ',', ':' }, StringSplitOptions.RemoveEmptyEntries);
                sensors.Add(new SensorBeaconPair()
                {
                    SensorX = int.Parse(parts[3]),
                    SensorY = int.Parse(parts[5]),
                    BeaconX = int.Parse(parts[11]),
                    BeaconY = int.Parse(parts[13]),
                });
            }

            // A sensor-beacon pair at (x1, y1)-(x2,y2) exclude all points with Manhattan distance from (x1,y1) equal or less than (|x2-x1|+|y2-y1|)
            int yOfInterest = 2000000; // 10 for the example

            var sorted = GetRanges(yOfInterest, sensors);

            // Number of excluded spaces, minus any beacons in that range
            Console.WriteLine(sorted.Sum(x => x.High - x.Low + 1 - sensors.Where(s => s.BeaconY == yOfInterest && s.BeaconX >= x.Low && s.BeaconX <= x.High).Select(s => s.BeaconX).Distinct().Count()));

            // Part 2 - brute force fails hugely; reuse part 1
            for (int y = 0; y < 4000000; y++)
            {
                sorted = GetRanges(y, sensors);
                if (!sorted.Any(s => s.Low <= 0 && s.High >= 4000000))
                {
                    var lower = sorted.Last(x => x.High <= 4000000).High;
                    var higher = sorted.First(x => x.Low > 0).Low;
                    long missingpointx = (higher + lower) / 2;
                    Console.WriteLine(4000000 * missingpointx + y);
                    // If you write more than one line, the puzzle is borked.
                    break;
                }
            }

            DateTime end = DateTime.Now;
            Console.WriteLine((end - start).TotalSeconds);
        }

        private static List<Range> GetRanges(int yOfInterest, List<SensorBeaconPair> sensors)
        {
            List<Range> excludedRangesOnLine = new List<Range>();
            foreach (var sensor in sensors)
            {
                var manhattanDistanceSensorToBeacon = sensor.Radius;
                if (Math.Abs(sensor.SensorY - yOfInterest) > manhattanDistanceSensorToBeacon)
                    continue; // too far away to be relevant

                var excludedRangeLower = sensor.SensorX - (manhattanDistanceSensorToBeacon - Math.Abs(sensor.SensorY - yOfInterest));
                var excludedRangeHigher = sensor.SensorX + (manhattanDistanceSensorToBeacon - Math.Abs(sensor.SensorY - yOfInterest));
                excludedRangesOnLine.Add(new Range(excludedRangeLower, excludedRangeHigher));
            }
            var sorted = excludedRangesOnLine.OrderBy(x => x.Low).ToList();
            Accumulate(sorted);
            return sorted;
        }

        private static void Accumulate(List<Range> input, int startIndex = 0)
        {
            int alteredIndex = -1;
            for (int i = startIndex; i < input.Count - 1; i++)
            {
                var first = input[i];
                var second = input[i + 1];
                if (first.High >= second.Low)
                {
                    // just changing first doesn't affect the list :(
                    first.High = Math.Max(first.High, second.High);
                    input.RemoveAt(i + 1);
                    alteredIndex = i;
                    break;
                }
            }
            // Carry on where you left off, don't start again
            if (alteredIndex >= 0) Accumulate(input, alteredIndex);
        }

        private class SensorBeaconPair
        {
            public int SensorX;
            public int SensorY;
            public int BeaconX;
            public int BeaconY;

            private int _radius = -1;
            public int Radius
            {
                get
                {
                    if (_radius == -1) _radius = Math.Abs(SensorX - BeaconX) + Math.Abs(SensorY - BeaconY);
                    return _radius;
                }
            }
        }

        internal class Range
        {
            public Range(int low, int high)
            {
                Low = low;
                High = high;
            }

            public int Low; 
            public int High;
        }
    }
}
