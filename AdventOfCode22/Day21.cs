using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode22
{
    internal class Day21
    {
        internal static void DoDay21()
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(new FileStream(
                       "C:\\src\\juliahayward\\AdventOfCode22\\RawData\\21.txt", FileMode.Open)))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }

            Dictionary<string, Monkey> monkeys = ReadMonkeysFromInput(lines);
            // First part - take all as input, see what happens
            ResolveAsManyMonkeysAsPossible(monkeys);
            Console.WriteLine("Root says " + monkeys["root"].Value);

            long lowerBoundForHuman = -1;
            long upperBoundForHuman = 1;
            bool rootBounded = false;
            var calculatedRootValues = new Dictionary<long, double>();
            while (!rootBounded)
            {
                var lowerValue = ValueForHumanInput(lowerBoundForHuman, calculatedRootValues, lines);
                var upperValue = ValueForHumanInput(upperBoundForHuman, calculatedRootValues, lines);
                Console.WriteLine("Bound-searching " + lowerBoundForHuman + " to " + upperBoundForHuman);
                if (lowerValue * upperValue < 0)
                    rootBounded = true;
                else
                {
                    lowerBoundForHuman *= 2;
                    upperBoundForHuman *= 2;
                }
            }

            bool rootFound = false;
            while (!rootFound)
            {
                Console.WriteLine("Trying " + lowerBoundForHuman + " to " + upperBoundForHuman);
                var midpoint = (lowerBoundForHuman + upperBoundForHuman) / 2;
                var lowerValue = ValueForHumanInput(lowerBoundForHuman, calculatedRootValues, lines);
                var upperValue = ValueForHumanInput(upperBoundForHuman, calculatedRootValues, lines);
                var midValue = ValueForHumanInput(midpoint, calculatedRootValues, lines);
                if (midValue == 0)
                {
                    rootFound = true;
                    Console.WriteLine("Root sees equality when human inputs " + lowerBoundForHuman + " to " + upperBoundForHuman);
                }
                else if (lowerValue * midValue < 0)
                {
                    upperBoundForHuman = midpoint;
                }
                else if (midValue * upperValue < 0)
                {
                    lowerBoundForHuman = midpoint;
                }
                else
                {
                    // oops
                    throw new Exception("Oops, don't have a bound any more");
                }
            }
        }

        private static double ValueForHumanInput(long human, Dictionary<long, double> cached, IEnumerable<string> lines)
        {
            if (!cached.ContainsKey(human))
                cached.Add(human, ValueForHumanInput(human, lines));

            return cached[human];
        }

        private static double ValueForHumanInput(long human, IEnumerable<string> lines)
        {
            // Reset
            var monkeys = ReadMonkeysFromInput(lines);
            // Alter "root" so it's trying to get to 0
            monkeys["root"].Expression = monkeys["root"].Expression.Replace("+", "-");
            monkeys["humn"].Value = human;
            monkeys["humn"].Resolved = true;
            ResolveAsManyMonkeysAsPossible(monkeys);
            return monkeys["root"].Value;
        }

        private static Dictionary<string, Monkey> ReadMonkeysFromInput(IEnumerable<string> lines)
        {
            int result;
            Dictionary<string, Monkey> monkeys = new Dictionary<string, Monkey>();
            foreach (var line in lines)
            {
                var parts = line.Split(':');
                var monkey = new Monkey() { Name = parts[0].Trim(), Expression = parts[1].Trim() };
                if (int.TryParse(monkey.Expression, out result))
                {
                    monkey.Value = result;
                    monkey.Resolved = true;
                }
                monkeys.Add(parts[0].Trim(), monkey);
            }

            return monkeys;
        }

        private static void ResolveAsManyMonkeysAsPossible(Dictionary<string, Monkey> monkeys)
        {
            bool progress = true;
            while (progress)
            {
                progress = false;
                foreach (var monkey in monkeys.Values.Where(m => !m.Resolved))
                {
                    if (monkey.Expression.Contains("+") && monkeys[monkey.Expression.Substring(0, 4)].Resolved
                                                        && monkeys[monkey.Expression.Substring(7, 4)].Resolved)
                    {
                        monkey.Value = monkeys[monkey.Expression.Substring(0, 4)].Value +
                                       monkeys[monkey.Expression.Substring(7, 4)].Value;
                        monkey.Resolved = true;
                        progress = true;
                    }
                    if (monkey.Expression.Contains("-") && monkeys[monkey.Expression.Substring(0, 4)].Resolved
                                                        && monkeys[monkey.Expression.Substring(7, 4)].Resolved)
                    {
                        monkey.Value = monkeys[monkey.Expression.Substring(0, 4)].Value -
                                       monkeys[monkey.Expression.Substring(7, 4)].Value;
                        monkey.Resolved = true;
                        progress = true;
                    }
                    if (monkey.Expression.Contains("*") && monkeys[monkey.Expression.Substring(0, 4)].Resolved
                                                        && monkeys[monkey.Expression.Substring(7, 4)].Resolved)
                    {
                        monkey.Value = monkeys[monkey.Expression.Substring(0, 4)].Value *
                                       monkeys[monkey.Expression.Substring(7, 4)].Value;
                        monkey.Resolved = true;
                        progress = true;
                    }
                    if (monkey.Expression.Contains("/") && monkeys[monkey.Expression.Substring(0, 4)].Resolved
                                                        && monkeys[monkey.Expression.Substring(7, 4)].Resolved)
                    {
                        monkey.Value = monkeys[monkey.Expression.Substring(0, 4)].Value /
                                       monkeys[monkey.Expression.Substring(7, 4)].Value;
                        monkey.Resolved = true;
                        progress = true;
                    }
                }
            }
        }

        public class Monkey
        {
            public string Name;
            public string Expression;
            public double Value;
            public bool Resolved;
        }
    }
}
