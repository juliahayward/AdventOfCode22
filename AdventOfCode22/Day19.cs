using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode22
{
    internal class Day19
    {
        internal static void DoDay19()
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(new FileStream(
                       "C:\\src\\juliahayward\\AdventOfCode22\\RawData\\19-example.txt", FileMode.Open)))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }

            List<Blueprint> blueprints = new List<Blueprint>();
            foreach (var line in lines)
            {
                var parts = line.Split(' ');
                blueprints.Add(new Blueprint()
                {
                    Number = int.Parse(parts[1].Replace(":", "")),
                    OreRobotNeedsOre = int.Parse(parts[6]),
                    ClayRobotNeedsOre = int.Parse(parts[12]),
                        ObsidianRobotNeedsOre = int.Parse(parts[18]),
                        ObsidianRobotNeedsClay = int.Parse(parts[21]),
                        GeodeRobotNeedsOre = int.Parse(parts[27]),
                            GeodeRobotNeedsObsidian = int.Parse(parts[30])
                });
            }

            foreach (var blueprint in blueprints)
            {
                List<State> initialStates = new List<State> {new State()};
                List<State> nextStates = new List<State>();
                for (int i = 1; i <= 24; i++)
                {
                    foreach (var state in initialStates)
                    {
                        var allRobotsPossible = true;
                        if (state.OreInStock >= blueprint.OreRobotNeedsOre)
                        {
                            var newState = new State()
                            {
                                OreInStock = state.OreInStock + state.OreRobots - blueprint.OreRobotNeedsOre,
                                ClayInStock = state.ClayInStock + state.ClayRobots,
                                ObsidianInStock = state.ObsidianInStock + state.ObsidianRobots,
                                GeodeInStock = state.GeodeInStock + state.GeodeRobots,
                                OreRobots = state.OreRobots + 1,
                                ClayRobots = state.ClayRobots,
                                ObsidianRobots = state.ObsidianRobots,
                                GeodeRobots = state.GeodeRobots
                            };
                            if (!nextStates.Any(x => x.Dominates(newState)) && !nextStates.Any(x => x.GeodeInStock > newState.AchievableGeodes(24-i)))
                                nextStates.Add(newState);
                        }
                        else allRobotsPossible = false;

                        if (state.OreInStock >= blueprint.ClayRobotNeedsOre)
                        {
                            var newState = new State()
                            {
                                OreInStock = state.OreInStock + state.OreRobots - blueprint.ClayRobotNeedsOre,
                                ClayInStock = state.ClayInStock + state.ClayRobots,
                                ObsidianInStock = state.ObsidianInStock + state.ObsidianRobots,
                                GeodeInStock = state.GeodeInStock + state.GeodeRobots,
                                OreRobots = state.OreRobots,
                                ClayRobots = state.ClayRobots + 1,
                                ObsidianRobots = state.ObsidianRobots,
                                GeodeRobots = state.GeodeRobots
                            };
                            if (!nextStates.Any(x => x.Dominates(newState)) && !nextStates.Any(x => x.GeodeInStock > newState.AchievableGeodes(24 - i)))
                                nextStates.Add(newState);
                        }
                        else allRobotsPossible = false;

                        if (state.OreInStock >= blueprint.ObsidianRobotNeedsOre && state.ClayInStock >= blueprint.ObsidianRobotNeedsClay)
                        {
                            var newState = new State()
                            {
                                OreInStock = state.OreInStock + state.OreRobots - blueprint.ObsidianRobotNeedsOre,
                                ClayInStock = state.ClayInStock + state.ClayRobots - blueprint.ObsidianRobotNeedsClay,
                                ObsidianInStock = state.ObsidianInStock + state.ObsidianRobots,
                                GeodeInStock = state.GeodeInStock + state.GeodeRobots,
                                OreRobots = state.OreRobots,
                                ClayRobots = state.ClayRobots,
                                ObsidianRobots = state.ObsidianRobots + 1,
                                GeodeRobots = state.GeodeRobots
                            };
                            if (!nextStates.Any(x => x.Dominates(newState)) && !nextStates.Any(x => x.GeodeInStock > newState.AchievableGeodes(24 - i)))
                                nextStates.Add(newState);
                        }
                        else allRobotsPossible = false;

                        if (state.OreInStock >= blueprint.GeodeRobotNeedsOre && state.ObsidianInStock >= blueprint.GeodeRobotNeedsObsidian)
                        {
                            var newState = new State()
                            {
                                OreInStock = state.OreInStock + state.OreRobots - blueprint.GeodeRobotNeedsOre,
                                ClayInStock = state.ClayInStock + state.ClayRobots,
                                ObsidianInStock = state.ObsidianInStock + state.ObsidianRobots - blueprint.GeodeRobotNeedsObsidian,
                                GeodeInStock = state.GeodeInStock + state.GeodeRobots,
                                OreRobots = state.OreRobots,
                                ClayRobots = state.ClayRobots,
                                ObsidianRobots = state.ObsidianRobots,
                                GeodeRobots = state.GeodeRobots + 1
                            };
                            if (!nextStates.Any(x => x.Dominates(newState)) && !nextStates.Any(x => x.GeodeInStock > newState.AchievableGeodes(24 - i)))
                                nextStates.Add(newState);
                        }
                        else allRobotsPossible = false;

                        // No point in saving up if we can already do anything
                        if (!allRobotsPossible)
                        {
                            var newState = new State()
                            {
                                OreInStock = state.OreInStock + state.OreRobots,
                                ClayInStock = state.ClayInStock + state.ClayRobots,
                                ObsidianInStock = state.ObsidianInStock + state.ObsidianRobots,
                                GeodeInStock = state.GeodeInStock + state.GeodeRobots,
                                OreRobots = state.OreRobots,
                                ClayRobots = state.ClayRobots,
                                ObsidianRobots = state.ObsidianRobots,
                                GeodeRobots = state.GeodeRobots
                            };
                            if (!nextStates.Any(x => x.Dominates(newState)) && !nextStates.Any(x => x.GeodeInStock > newState.AchievableGeodes(24 - i)))
                                nextStates.Add(newState);
                        }
                    }

                    Console.WriteLine(i +  "-->" + nextStates.Count());

                    initialStates = nextStates;
                    nextStates = new List<State>();
                }

                Console.WriteLine("Blueprint " + blueprint.Number + " makes " + initialStates.Max(x => x.GeodeInStock));
            }

        }

        internal class Blueprint
        {
            public int Number;
            public int OreRobotNeedsOre;
            public int ClayRobotNeedsOre;
            public int ObsidianRobotNeedsOre;
            public int ObsidianRobotNeedsClay;
            public int GeodeRobotNeedsOre;
            public int GeodeRobotNeedsObsidian;

            public int MaxGeodesAvailable;
        }

        internal class State
        {
            public int OreInStock;
            public int ClayInStock;
            public int ObsidianInStock;
            public int GeodeInStock;
            public int OreRobots = 1;
            public int ClayRobots;
            public int ObsidianRobots;
            public int GeodeRobots;

            public int AchievableGeodes(int minutesLeft)
            {
                // Ones we already have, ones we can produce with existing robots, and ones possible if we only
                // produce future robots
                return GeodeInStock + (GeodeRobots * minutesLeft) +
                       Math.Max(0, (minutesLeft) * (minutesLeft - 1) / 2);
            }

            public bool Dominates(State other)
            {
                return (OreInStock >= other.OreInStock && ClayInStock >= other.ClayInStock && ObsidianInStock >= other.ObsidianInStock && GeodeInStock >= other.GeodeInStock
                    && OreRobots >= other.OreRobots && ClayRobots >= other.ClayRobots && ObsidianRobots >= other.ObsidianRobots && GeodeRobots >= other.GeodeRobots);
            }
        }
    }
}
