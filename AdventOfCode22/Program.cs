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
            DoDay10A();
            DoDay10B();
        }

        private static void DoDay10A()
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(new FileStream("C:\\src\\juliahayward\\AdventOfCode22\\RawData\\11.txt",
                       FileMode.Open)))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }

            var monkeys = new List<Monkey>();
            Monkey currentMonkey = null;
            foreach (var line in lines)
            {
                if (line.StartsWith("Monkey"))
                {
                    int index = int.Parse(line.Replace("Monkey", "").Replace(":", "").Trim());
                    currentMonkey = new Monkey() {Index = index};
                    monkeys.Add(currentMonkey);
                }
                else if (line.Contains("Starting items"))
                {
                    currentMonkey.Items =
                        line.Replace("Starting items:", "").Trim().Split(',')
                            .Select(x => long.Parse(x.Trim())).ToList();
                }
                else if (line.Contains("Operation"))
                {
                    if (line.Contains("old * old"))
                        currentMonkey.Operation = x => x * x;
                    else if (line.Contains("old * "))
                    {
                        int y = int.Parse(line.Replace("Operation: new = old * ", "").Trim());
                        currentMonkey.Operation = x => x * y;
                    }
                    else if (line.Contains("old + "))
                    {
                        int y = int.Parse(line.Replace("Operation: new = old + ", "").Trim());
                        currentMonkey.Operation = x => x + y;
                    }
                }
                else if (line.Contains("Test"))
                {
                    int y = int.Parse(line.Replace("Test: divisible by ", "").Trim());
                    currentMonkey.Test = x => x % y == 0;
                }
                else if (line.Contains("If true"))
                {
                    int y = int.Parse(line.Replace("If true: throw to monkey", "").Trim());
                    currentMonkey.IfTrueThrowToMonkeyIndex = y;
                }
                else if (line.Contains("If false"))
                {
                    int y = int.Parse(line.Replace("If false: throw to monkey ", "").Trim());
                    currentMonkey.IfFalseThrowToMonkeyIndex = y;
                }
            }

            for (int round = 1; round <= 20; round++)
            {
                foreach (var monkey in monkeys)
                {
                    while (monkey.Items.Any())
                    {
                        var item = monkey.Items.First();
                        // Worry goes up when monkey picks up item
                        var pickedUp = monkey.Operation(item);
                        // And back down when they grow bored of it.
                        var boredAgain = pickedUp / 3;
                        monkey.TotalActivity++;
                        monkey.Items.RemoveAt(0);
                        if (monkey.Test(boredAgain))
                            monkeys[monkey.IfTrueThrowToMonkeyIndex].Items.Add(boredAgain);
                        else
                            monkeys[monkey.IfFalseThrowToMonkeyIndex].Items.Add(boredAgain);

                    }
                }
            }

            var sortedMonkeys = monkeys.OrderByDescending(m => m.TotalActivity);
            long answer1 = sortedMonkeys.First().TotalActivity * sortedMonkeys.Skip(1).First().TotalActivity;
            Console.WriteLine(answer1);
        }

        private static void DoDay10B()
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(new FileStream("C:\\src\\juliahayward\\AdventOfCode22\\RawData\\11.txt",
                       FileMode.Open)))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }

            var monkeys = new List<Monkey>();
            Monkey currentMonkey = null;
            int monkeyDivisorLCM = 1;

            foreach (var line in lines)
            {
                if (line.StartsWith("Monkey"))
                {
                    int index = int.Parse(line.Replace("Monkey", "").Replace(":", "").Trim());
                    currentMonkey = new Monkey() { Index = index };
                    monkeys.Add(currentMonkey);
                }
                else if (line.Contains("Starting items"))
                {
                    currentMonkey.Items =
                        line.Replace("Starting items:", "").Trim().Split(',')
                            .Select(x => long.Parse(x.Trim())).ToList();
                }
                else if (line.Contains("Operation"))
                {
                    if (line.Contains("old * old"))
                        currentMonkey.Operation = x => x * x;
                    else if (line.Contains("old * "))
                    {
                        int y = int.Parse(line.Replace("Operation: new = old * ", "").Trim());
                        currentMonkey.Operation = x => x * y;
                    }
                    else if (line.Contains("old + "))
                    {
                        int y = int.Parse(line.Replace("Operation: new = old + ", "").Trim());
                        currentMonkey.Operation = x => x + y;
                    }
                }
                else if (line.Contains("Test"))
                {
                    int y = int.Parse(line.Replace("Test: divisible by ", "").Trim());
                    currentMonkey.Test = x => x % y == 0;
                    monkeyDivisorLCM = LCM(monkeyDivisorLCM, y);
                }
                else if (line.Contains("If true"))
                {
                    int y = int.Parse(line.Replace("If true: throw to monkey", "").Trim());
                    currentMonkey.IfTrueThrowToMonkeyIndex = y;
                }
                else if (line.Contains("If false"))
                {
                    int y = int.Parse(line.Replace("If false: throw to monkey ", "").Trim());
                    currentMonkey.IfFalseThrowToMonkeyIndex = y;
                }
            }

            for (int round = 1; round <= 10000; round++)
            {
                foreach (var monkey in monkeys)
                {
                    while (monkey.Items.Any())
                    {
                        var item = monkey.Items.First();
                        // Worry goes up when monkey picks up item
                        var pickedUp = monkey.Operation(item);
                        // NOTE - since all monkeys test by division mod N, the behaviour of an item is invariant up to mod LCM(divisors)
                        pickedUp = pickedUp % monkeyDivisorLCM;
                        monkey.TotalActivity++;
                        monkey.Items.RemoveAt(0);
                        if (monkey.Test(pickedUp))
                            monkeys[monkey.IfTrueThrowToMonkeyIndex].Items.Add(pickedUp);
                        else
                            monkeys[monkey.IfFalseThrowToMonkeyIndex].Items.Add(pickedUp);

                    }
                }
            }

            var sortedMonkeys = monkeys.OrderByDescending(m => m.TotalActivity);
            long answer1 = sortedMonkeys.First().TotalActivity * sortedMonkeys.Skip(1).First().TotalActivity;
            Console.WriteLine(answer1);
        }
        internal class Monkey
        {
            public int Index;

            public long TotalActivity;

            public List<long> Items = new List<long>();

            public Func<long, long> Operation;

            public Func<long, bool> Test;

            public int IfTrueThrowToMonkeyIndex;

            public int IfFalseThrowToMonkeyIndex;
        }

        static int GCF(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        static int LCM(int a, int b)
        {
            return (a / GCF(a, b)) * b;
        }

    }
}
