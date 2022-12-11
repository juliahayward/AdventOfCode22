using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode22
{
    internal class Day10
    {
        private static void DoDay10()
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(new FileStream("C:\\src\\juliahayward\\AdventOfCode22\\RawData\\10.txt",
                       FileMode.Open)))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }

            var signals = new List<Signal>();
            signals.Add(new Signal() { StartCycle = 0, Value = 1 });
            int currentCycle = 1;

            foreach (var line in lines)
            {
                if (line.StartsWith("noop"))
                {
                    currentCycle++;
                }
                else
                {
                    var parts = line.Split(' ');
                    // parts[0] will be addx
                    var increment = int.Parse(parts[1]);
                    currentCycle += 2;
                    var currentSignal = signals.Last().Value;
                    signals.Add(new Signal() { StartCycle = currentCycle, Value = currentSignal + increment });
                }
            }

            var cyclesOfInterest = new[] { 20, 60, 100, 140, 180, 220 };

            var totalStrength = cyclesOfInterest.Sum(cycle => cycle * signals.Where(x => x.StartCycle <= cycle).Last().Value);

            Console.WriteLine(totalStrength);

            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 40; col++)
                {
                    int cycleOnWhichThisPixelIsDrawn = row * 40 + col + 1;
                    int xx = signals.Where(x => x.StartCycle <= cycleOnWhichThisPixelIsDrawn).Last().Value;
                    if (xx >= col - 1 && xx <= col + 1)
                        Console.Write("#");
                    else
                        Console.Write(".");
                }

                Console.WriteLine("");
            }
        }

        private class Signal
        {
            public int Value;
            public int StartCycle;
        }
    }
}
