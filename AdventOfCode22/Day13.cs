using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace AdventOfCode22
{
    internal class Day13
    {
        private static void DoDay13()
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(new FileStream(
                       "C:\\src\\juliahayward\\AdventOfCode22\\RawData\\13.txt",
                       FileMode.Open)))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }

            var indexesInCorrectOrder = 0;
            var pairIndex = 1;
            for (int i = 0; i < lines.Count; i += 3)
            {
                var left = Parse(lines[i]);
                var right = Parse(lines[i + 1]);

                indexesInCorrectOrder += (PacketComparison(left, right) == -1) ? pairIndex : 0;
                pairIndex++;
            }

            Console.WriteLine(indexesInCorrectOrder);

            List<JsonNode> allPackets = new List<JsonNode>();
            // Find all of them and put them in a big list
            for (int i = 0; i < lines.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(lines[i])) allPackets.Add(Parse(lines[i]));
            }

            var divider1 = new JsonArray();
            var inner1 = new JsonArray
            {
                2
            };
            divider1.Add(inner1);

            var divider2 = new JsonArray();
            var inner2 = new JsonArray
            {
                6
            };
            divider2.Add(inner2);

            allPackets.Add(divider1);
            allPackets.Add(divider2);
            allPackets.Sort(PacketComparison);

            var first = allPackets.IndexOf(divider1) + 1;   // Yay for off-by-one
            var second = allPackets.IndexOf(divider2) + 1;

            Console.WriteLine(first * second);
        }

        private static JsonNode Parse(string input)
        {
            return JsonObject.Parse(input);
        }

        private static int PacketComparison(JsonNode left, JsonNode right)
        {
            if (left is JsonArray && right is JsonArray)
            {
                var leftArray = left as JsonArray;
                var rightArray = right as JsonArray;
                // One of the array items differs
                for (int i = 0; i < Math.Min(leftArray.Count, rightArray.Count); i++)
                {
                    var sublistCheck = PacketComparison(leftArray[i], rightArray[i]);
                    if (sublistCheck != 0) return sublistCheck;
                }

                // Array all same, but length different
                if (leftArray.Count > rightArray.Count) return 1;
                if (rightArray.Count > leftArray.Count) return -1;
                // Indeterminate; move on
                return 0;
            }
            else if (left is JsonArray)
            {
                var newRightArray = new JsonArray();
                newRightArray.Add(right.GetValue<int>());
                return PacketComparison(left, newRightArray);
            }
            else if (right is JsonArray)
            {
                var newLeftArray = new JsonArray();
                newLeftArray.Add(left.GetValue<int>());
                return PacketComparison(newLeftArray, right);
            }
            else
            {
                var leftInt = left.GetValue<int>();
                var rightInt = right.GetValue<int>();
                if (leftInt < rightInt) return -1;
                if (leftInt > rightInt) return 1;
                return 0;
            }
        }
    }
}
