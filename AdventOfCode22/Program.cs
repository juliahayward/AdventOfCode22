﻿using System;
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
            var start = DateTime.UtcNow;
           Day25.DoDay25();
           Console.WriteLine((DateTime.UtcNow-start).TotalSeconds);

           Console.ReadLine();

        }

        
    }
}
