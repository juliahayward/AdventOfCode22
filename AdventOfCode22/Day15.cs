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
            Console.WriteLine(sorted.Sum(x => x.Item2 - x.Item1 + 1 - sensors.Where(s => s.BeaconY == yOfInterest && s.BeaconX >= x.Item1 && s.BeaconX <= x.Item2).Select(s => s.BeaconX).Distinct().Count()));

            // Part 2 - brute force fails hugely; reuse part 1
            for (int y = 0; y < 4000000; y++)
            {
                sorted = GetRanges(y, sensors);
                if (!sorted.Any(s => s.Item1 <= 0 && s.Item2 >= 4000000))
                {
                    var lower = sorted.Last(x => x.Item2 <= 4000000).Item2;
                    var higher = sorted.First(x => x.Item1 > 0).Item1;
                    long missingpointx = (higher + lower) / 2;
                    Console.WriteLine(4000000 * missingpointx + y);
                }
            }

        }

        private static List<(int, int)> GetRanges(int yOfInterest, List<SensorBeaconPair> sensors)
        {
            List<(int, int)> excludedRangesOnLine = new List<(int, int)>();
            foreach (var sensor in sensors)
            {
                var manhattanDistanceSensorToBeacon = Math.Abs(sensor.SensorX - sensor.BeaconX) + Math.Abs(sensor.SensorY - sensor.BeaconY);
                if (Math.Abs(sensor.SensorY - yOfInterest) > manhattanDistanceSensorToBeacon)
                    continue; // too far away to be relevant

                var excludedRangeLower = sensor.SensorX - (manhattanDistanceSensorToBeacon - Math.Abs(sensor.SensorY - yOfInterest));
                var excludedRangeHigher = sensor.SensorX + (manhattanDistanceSensorToBeacon - Math.Abs(sensor.SensorY - yOfInterest));
                excludedRangesOnLine.Add((excludedRangeLower, excludedRangeHigher));
            }
            var sorted = excludedRangesOnLine.OrderBy(x => x.Item1).ToList();
            Accumulate(sorted);
            return sorted;
        }

        private static void Accumulate(List<(int, int)> input)
        {
            bool workDone = false;
            for (int i = 0; i < input.Count - 1; i++)
            {
                var first = input[i];
                var second = input[i + 1];
                if (first.Item2 >= second.Item1)
                {
                    // just changing first doesn't affect the list :(
                    input.RemoveAt(i);
                    input.Insert(i, (first.Item1, Math.Max(first.Item2, second.Item2)));
                    input.RemoveAt(i + 1);
                    workDone = true;
                    break;
                }
            }
            if (workDone) Accumulate(input);
        }

        private class SensorBeaconPair
        {
            public int SensorX;
            public int SensorY;
            public int BeaconX;
            public int BeaconY;
        }
    }
}
