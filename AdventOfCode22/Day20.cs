using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode22
{
    internal class Day20
    {
        internal static void DoDay20()
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(new FileStream(
                       "C:\\src\\juliahayward\\AdventOfCode22\\RawData\\20.txt", FileMode.Open)))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }

            int idx = 0;
            var integers = lines.Select(l => (long.Parse(l), idx++)).ToArray();
            // We need to mess with the above, so make a copy to iterate over! Note that in the main data, integers are *not* unique so we need to remember which is which
            idx = 0;
            var original = lines.Select(l => (long.Parse(l), idx++)).ToList();

            Mix(original, integers);

            var listLength = original.Count();
            var zero = integers.First(i => i.Item1 == 0);
            long i1 = integers.ElementAt((1000 + Array.IndexOf(integers, zero)) % listLength).Item1;
            long i2 = integers.ElementAt((2000 + Array.IndexOf(integers, zero)) % listLength).Item1;
            long i3 = integers.ElementAt((3000 + Array.IndexOf(integers, zero)) % listLength).Item1;
            Console.WriteLine(i1 + i2 + i3 + "="+i1+"+"+i2+"+"+i3);

            // Update the list
            idx = 0;
            integers = lines.Select(l => (811589153 * long.Parse(l), idx++)).ToArray();
            // We need to mess with the above, so make a copy to iterate over! Note that in the main data, integers are *not* unique so we need to remember which is which
            idx = 0;
            original = lines.Select(l => (811589153 * long.Parse(l), idx++)).ToList();

            for (int i = 0; i < 10; i++)
                Mix(original, integers);

            listLength = original.Count();
            zero = integers.First(i => i.Item1 == 0);
            i1 = integers.ElementAt((1000 + Array.IndexOf(integers, zero)) % listLength).Item1;
            i2 = integers.ElementAt((2000 + Array.IndexOf(integers, zero)) % listLength).Item1;
            i3 = integers.ElementAt((3000 + Array.IndexOf(integers, zero)) % listLength).Item1;
            Console.WriteLine(i1 + i2 + i3 + "=" + i1 + "+" + i2 + "+" + i3);
        }

        private static void Mix(List<(long, int)> original, (long, int)[] integers)
        {
            var listLength = original.Count();
            foreach (var i in original)
            {
                // Sidestep problems with wrapping, off-by-one and % of negative numbers
                for (int j = 0; j < Math.Abs(i.Item1) % (listLength - 1); j++)
                {
                    int index = Array.IndexOf(integers, i);
                    if (i.Item1 > 0)
                    {
                        var dest = index + 1;
                        if (dest == listLength) dest = 0;
                        var tmp = integers[index];
                        integers[index] = integers[dest];
                        integers[dest] = tmp;
                    }
                    else
                    {
                        bool rotate = false;
                        var dest = index - 1;
                        if (dest == -1)
                        {
                            dest = listLength - 1;
                            rotate = true;
                        }
                        var tmp = integers[index];
                        integers[index] = integers[dest];
                        integers[dest] = tmp;
                        if (rotate) // our overall indexing has shifted
                        {
                            tmp = integers[0];
                            for (int k = 0; k < listLength - 1; k++)
                                integers[k] = integers[k + 1];
                            integers[listLength - 1] = tmp;
                        }
                    }

                }

                //Console.WriteLine(string.Join(",", integers));
            }
        }
    }
}
