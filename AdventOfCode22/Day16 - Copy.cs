using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode22
{
    internal class Day16A
    {
        internal static void DoDay16()
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(new FileStream(
                       "C:\\src\\juliahayward\\AdventOfCode22\\RawData\\16.txt", FileMode.Open)))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }

            var valves = new Dictionary<string, Valve>();
            foreach (var line in lines)
            {
                var parts = line.Split(new[] {' ', '=', ';', ','}, StringSplitOptions.RemoveEmptyEntries);
                var valve = new Valve {Name = parts[1], Rate = int.Parse(parts[5])};
                for (int index = 10; index < parts.Length; index++)
                {
                    valve.Neighbours.Add(parts[index]);
                }

                valves.Add(valve.Name, valve);
            }

            var distances = new Dictionary<(string, string), int>();
            foreach (var valve in valves)
            {
                var currentValve = valve.Value;
                List<string> visited = new List<string>();
                distances[(currentValve.Name, currentValve.Name)] = 0;
                while (currentValve != null)
                {
                    foreach (var neighbour in currentValve.Neighbours)
                    {
                        if (!visited.Contains(neighbour))
                        {
                            if (distances.ContainsKey((valve.Value.Name, neighbour)))
                                distances[(valve.Value.Name, neighbour)] = Math.Min(
                                    distances[(valve.Value.Name, neighbour)],
                                    distances[(valve.Value.Name, currentValve.Name)] + 1);
                            else
                                distances[(valve.Value.Name, neighbour)] =
                                    distances[(valve.Value.Name, currentValve.Name)] + 1;
                        }
                    }

                    visited.Add(currentValve.Name);
                    currentValve = valves
                        .Where(x => !visited.Contains(x.Key) && distances.ContainsKey((valve.Value.Name, x.Key)))
                        .OrderBy(x => distances[(valve.Value.Name, x.Key)]).Select(x => x.Value).FirstOrDefault();
                }
            }

            int timeAvailable = 30;
            // Now calculate each sequence
            List<ValveSequence> sequences = new List<ValveSequence>();
            ValveSequence initialSequence = new ValveSequence();
            initialSequence.Valves.Add(valves["AA"]);
            sequences.Add(initialSequence);
            CalculateChildren(initialSequence, sequences, valves, distances, timeAvailable);

            /// For part one, find the greatest flow rate from a single sequence
            var best = sequences.OrderByDescending(s => s.FlowAchieved).FirstOrDefault();
            Console.WriteLine("Just you in 30 minutes: can get flow " + best.FlowAchieved);

            timeAvailable = 26;
            sequences = new List<ValveSequence>();
            initialSequence = new ValveSequence();
            initialSequence.Valves.Add(valves["AA"]);
            sequences.Add(initialSequence);
            CalculateChildren(initialSequence, sequences, valves, distances, timeAvailable);

            /// For part one, find the greatest flow rate from a single sequence
            best = sequences.OrderByDescending(s => s.FlowAchieved).FirstOrDefault();
            Console.WriteLine("Just you in 26 minutes: can get flow " + best.FlowAchieved);

            var bestTotalFlow = 0;
            foreach (var sequence in sequences.OrderByDescending(x => x.FlowAchieved))
            {
                // We will assume that the elephant does the lesser part of the total wlog (it's symmetric in the two)
                // Note - AA is always in common!
                var elephantSequence = sequences.Where(s => s.Valves.Intersect(sequence.Valves).Count() == 1 && s.FlowAchieved < sequence.FlowAchieved).OrderByDescending(s => s.FlowAchieved).First();
                var totalOverallFlow = sequence.FlowAchieved + elephantSequence.FlowAchieved;
                if (totalOverallFlow > bestTotalFlow)
                {
                    Console.WriteLine("New best for both: " + totalOverallFlow);
                    bestTotalFlow = totalOverallFlow;
                }
            }
        }

        private static void CalculateChildren(ValveSequence sequence, List<ValveSequence> sequences,
            Dictionary<string, Valve> valves, Dictionary<(string, string), int> distances, int timeAvailable)
        {
            foreach (var nextValve in valves.Where(v => !sequence.Valves.Contains(v.Value) && v.Value.Rate > 0))
            {
                var child = new ValveSequence();
                child.Valves.AddRange(sequence.Valves);
                child.Valves.Add(nextValve.Value);
                child.TimeElapsed = sequence.TimeElapsed + 1 + distances[(sequence.Valves.Last().Name, nextValve.Value.Name)];
                if (child.TimeElapsed > timeAvailable)
                    continue;

                child.FlowAchieved =
                    sequence.FlowAchieved + nextValve.Value.Rate * (timeAvailable - child.TimeElapsed);
                sequences.Add(child);
                CalculateChildren(child, sequences, valves, distances, timeAvailable);
            } 
        }

        private class Valve
        {
            public string Name;
            public int Rate;
            public List<string> Neighbours = new List<string>();
        }

        private class ValveSequence
        {
            public List<Valve> Valves = new List<Valve>();
            public int FlowAchieved = 0;
            public int TimeElapsed = 0;
        }
    }
}
