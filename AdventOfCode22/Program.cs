using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting;
using System.Text.Json;
using System.Text.Json.Nodes;
using static System.Net.Mime.MediaTypeNames;
using System.Text;

namespace AdventOfCode22
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DateTime start = DateTime.Now;
          //  Day17.DoDay17(2022);
            Console.WriteLine((DateTime.Now - start).TotalSeconds);

            // Part 2 - logging the initial part of the problem, the pattern repeats every 352 * 5 * 10091 = 17,760,160 blocks, 
            // after which the height has increased 27,619,067. So the value at 1E12 is (56303 * 27619067) + (value at 49,711,520 = 77,307,084)
            // where 1E12 = 56303 * 17,760,160 + 49,711,520
            Day17.DoDay17(49711520);
            Console.WriteLine((DateTime.Now - start).TotalSeconds);
            //Day17.DoDay17(1000000000000);
            Console.ReadLine();
        }

        
    }
}
