using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode22
{
    internal class Day16
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
                var parts = line.Split(new[] { ' ', '=', ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
                var valve = new Valve { Name = parts[1], Rate = int.Parse(parts[5]) };
                for (int index = 10; index < parts.Length; index++)
                {
                    valve.Neighbours.Add(parts[index]);
                }
                valves.Add(valve.Name, valve);
            }

            var initialState = new ValveState() { CurrentValve = "AA" };
            var numberOfWorkingValves = valves.Where(v => v.Value.Rate > 0).Count();

           // ComputeChildren(initialState, valves, numberOfWorkingValves, 30, false);

          //  Console.WriteLine(initialState.BestLeaf.PressureReleased);

            // Now do the same with an elephant
            initialState = new ValveState() { CurrentValve = "AA", CurrentElephantValve = "AA" };
            
            ComputeChildren(initialState, valves, numberOfWorkingValves, 26, true);
            Console.WriteLine(initialState.BestLeaf.PressureReleased);
        }

        public static int bestSoFar = 0;

        private static void ComputeChildren(ValveState currentState, Dictionary<string, Valve> valves, int numberOfWorkingValves, int totalMinutesAvailable, bool usingElephant)
        {
            var elephantsMove = (usingElephant && currentState.ActionsTaken.Count % 2 == 1);
            var currentValve = (elephantsMove ? currentState.CurrentElephantValve : currentState.CurrentValve);

            currentState.BestLeaf = currentState;
            // No child states if time has elapsed
            if (currentState.ElapsedTime >= totalMinutesAvailable) return; 
            // Can do no more if all valves are open
            if (currentState.OpenValves.Count == numberOfWorkingValves) return;
            // One possible child is to open this valve, if not already open, and is openable
            if (!currentState.OpenValves.Contains(currentValve) && valves[currentValve].Rate > 0)
            {
                var child = new ValveState()
                {
                    PressureReleased = currentState.PressureReleased + (totalMinutesAvailable - currentState.ElapsedTime - 1) * valves[currentValve].Rate
                };
                if (elephantsMove)
                {
                    child.CurrentElephantValve = currentValve;
                    child.CurrentValve = currentState.CurrentValve;
                    // Don't come back here if you haven't opened another valve since
                    child.ElephantNodesSinceLastValveOpened.Add(currentValve);
                }
                else
                {
                    child.CurrentValve = currentValve;
                    child.CurrentElephantValve = currentState.CurrentElephantValve;
                    // Don't come back here if you haven't opened another valve since
                    child.NodesSinceLastValveOpened.Add(currentValve);
                }
                child.OpenValves.AddRange(currentState.OpenValves);
                child.OpenValves.Add(currentValve);
                child.ActionsTaken.AddRange(currentState.ActionsTaken);
                child.ActionsTaken.Add(new OpenValveAction() { ValveName = currentValve });
                currentState.Children.Add(child);
                ComputeChildren(child, valves, numberOfWorkingValves, totalMinutesAvailable, usingElephant);
            }
            foreach (var neighbour in valves[currentValve].Neighbours)
            {
                var nodesToCheck = (elephantsMove) ? currentState.ElephantNodesSinceLastValveOpened : currentState.NodesSinceLastValveOpened;
                // I thought we should never need to traverse the same edge twice in the same direction. However, we may want to reach a big
                // valve early, then come back later to open a small valve that we passed en route. What we can say is that if there hasn't been
                // an OpenValveAction since we last traversed this edge in either direction, don't repeat it
                if (nodesToCheck.Contains(neighbour)) continue;

                var child = new ValveState()
                {
                    PressureReleased = currentState.PressureReleased
                };
                if (elephantsMove)
                {
                    child.CurrentElephantValve = neighbour;
                    child.CurrentValve = currentState.CurrentValve;
                    child.NodesSinceLastValveOpened.AddRange(currentState.NodesSinceLastValveOpened);
                    child.ElephantNodesSinceLastValveOpened.AddRange(currentState.ElephantNodesSinceLastValveOpened);
                    child.ElephantNodesSinceLastValveOpened.Add(neighbour);
                }
                else
                {
                    child.CurrentValve = neighbour;
                    child.CurrentElephantValve = currentState.CurrentElephantValve;
                    child.NodesSinceLastValveOpened.AddRange(currentState.NodesSinceLastValveOpened);
                    child.ElephantNodesSinceLastValveOpened.AddRange(currentState.ElephantNodesSinceLastValveOpened);
                    child.NodesSinceLastValveOpened.Add(neighbour);
                };
                child.OpenValves.AddRange(currentState.OpenValves);
                child.ActionsTaken.AddRange(currentState.ActionsTaken);
                child.ActionsTaken.Add(new MoveToValveAction() { FromValveName = currentValve, ToValveName = neighbour });            
                currentState.Children.Add(child);
                ComputeChildren(child, valves, numberOfWorkingValves, totalMinutesAvailable, usingElephant);
            }

            // We cut off when all valves are open, so "best" includes no wastleful pootling around
            if (currentState.Children.Any())
            {
                currentState.BestLeaf = currentState.Children.OrderByDescending(c => c.BestLeaf.PressureReleased).First().BestLeaf;
                // Once we're here, "Children" serve no further purpose, so recycle the memory
                currentState.Children = null;
            }
            // Purely to see progress
            if (currentState.PressureReleased > bestSoFar)
            {
                bestSoFar = currentState.PressureReleased;
                Console.WriteLine(bestSoFar);
            }
        }


        private class Valve
        {
            public string Name;
            public int Rate;
            public List<string> Neighbours = new List<string>();
        }

        private class ValveState
        {
            public string CurrentValve;
            public string CurrentElephantValve;
            public int ElapsedTime => ActionsTaken.Count() / ((CurrentElephantValve == null) ? 1 : 2);
            public List<string> OpenValves = new List<string>();
            public int PressureReleased = 0;
            public ValveState BestLeaf = null;
            public List<IAction> ActionsTaken = new List<IAction>();
            public List<ValveState> Children = new List<ValveState>();
            public List<string> NodesSinceLastValveOpened = new List<string>();
            public List<string> ElephantNodesSinceLastValveOpened = new List<string>();
        }

        private interface IAction { }

        private class OpenValveAction : IAction
        {
            public string ValveName;
            public override string ToString()
            {
                return "Open " + ValveName;
            }
        }
        private class MoveToValveAction : IAction
        {
            public string FromValveName; public string ToValveName;
            public override string ToString()
            {
                return FromValveName + "->" + ToValveName;
            }
        }
    }
}
