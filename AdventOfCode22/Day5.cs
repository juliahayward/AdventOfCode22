using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode22
{
    internal class Day5
    {
        private static void DoDay5(bool oneAtATime)
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(new FileStream("C:\\src\\juliahayward\\AdventOfCode22\\RawData\\5.txt",
                       FileMode.Open)))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }

            // First identify the line that defines the stacks
            var stackDefinitionLine = lines.First(x => x.StartsWith(" 1"));
            var stackDefinitionLineIndex = lines.IndexOf(stackDefinitionLine);

            var numberOfStacks = int.Parse(stackDefinitionLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Last());

            var stacks = new Stack<char>[numberOfStacks];
            for (int i = 0; i < numberOfStacks; i++) stacks[i] = new Stack<char>();

            for (int i = stackDefinitionLineIndex - 1; i >= 0; i--)
            {
                var crateLine = lines[i];
                for (int j = 0; j < numberOfStacks; j++)
                {
                    if (crateLine.Length > 4 * j + 1 && crateLine[4 * j + 1] != ' ')
                        stacks[j].Push(crateLine[4 * j + 1]);
                }
            }

            // Now do the moves
            for (int i = stackDefinitionLineIndex + 2; i < lines.Count; i++)
            {
                var parts = lines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                // Line is of the form "move N from I to J"
                var numToMove = int.Parse(parts[1]);
                var source = int.Parse(parts[3]) - 1;   // Yes, they are 1-based
                var target = int.Parse(parts[5]) - 1;
                if (oneAtATime)
                    MoveOneAtATime(stacks, numToMove, source, target);
                else
                    MoveAllInOneGo(stacks, numToMove, source, target);
            }

            // Write the top of each column
            Console.WriteLine(string.Join("", stacks.Select(x => x.First())));
        }

        private static void MoveOneAtATime(Stack<char>[] stacks, int numToMove, int source, int target)
        {
            for (int j = 0; j < numToMove; j++)
            {
                stacks[target].Push(stacks[source].Pop());
            }
        }

        private static void MoveAllInOneGo(Stack<char>[] stacks, int numToMove, int source, int target)
        {
            var cranesInternalStack = new Stack<char>();
            for (int j = 0; j < numToMove; j++)
            {
                cranesInternalStack.Push(stacks[source].Pop());
            }
            for (int j = 0; j < numToMove; j++)
            {
                stacks[target].Push(cranesInternalStack.Pop());
            }
        }
    }
}
