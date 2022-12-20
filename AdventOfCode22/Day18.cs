using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace AdventOfCode22
{
    internal class Day18
    {
        internal static void DoDay18()
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(new FileStream(
                       "C:\\src\\juliahayward\\AdventOfCode22\\RawData\\18.txt", FileMode.Open)))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }
            var cubes = new List<Cube>();
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                cubes.Add(new Cube { X = int.Parse(parts[0]), Y = int.Parse(parts[1]), Z = int.Parse(parts[2]) });
            }

            foreach (var c in cubes)
            {
                c.ExposedFaces = 6 - cubes.Count(oc => c.IsNeighbourOf(oc));
            };

            Console.WriteLine(cubes.Sum(c => c.ExposedFaces));

            int xmin = cubes.Min(c => c.X);
            int xmax = cubes.Max(c => c.X);
            int ymin = cubes.Min(x => x.Y);
            int ymax = cubes.Max(x => x.Y);
            int zmin = cubes.Min(x => x.Z);
            int zmax = cubes.Max(x => x.Z);
            // It appears the mins are all 0, so 
            int[,,] exterior = new int[xmax+1, ymax+1, zmax+1];
            // 0 = unknown, 1 = lava, 2 = exterior
            foreach (var cell in cubes)
                exterior[cell.X, cell.Y, cell.Z] = 1;
            for (int i = 0; i <= xmax; i++)
                for (int j = 0; j <= ymax; j++)
                    for (int k = 0; k <= xmax; k++)
                    {
                        if ((i == 0 || j == 0 || k == 0 || i == xmax || j == ymax || k == zmax) && exterior[i, j, k] != 1)
                        {
                            exterior[i, j, k] = 2;
                        }
                    }
            bool progressMade = false;
            do
            {
                progressMade = false;
                for (int i = 1; i < xmax; i++)
                    for (int j = 1; j < ymax; j++)
                        for (int k = 1; k < xmax; k++)
                        {
                            if (exterior[i, j, k] != 0) 
                                continue;
                            // If you're next to exterior, then you're exterior too
                            if (exterior[i-1,j,k]==2 || exterior[i + 1, j, k] == 2 || exterior[i, j - 1, k] == 2 || exterior[i, j + 1, k] == 2
                                || exterior[i, j, k - 1] == 2 || exterior[i, j, k + 1] == 2)
                            {
                                exterior[i, j, k] = 2;
                                progressMade = true;
                            }
                        }
                Console.Write("*");
            }
            while (progressMade);

            for (int i = 1; i < xmax; i++)
                for (int j = 1; j < ymax; j++)
                    for (int k = 1; k < xmax; k++)
                    {
                        if (exterior[i,j,k]== 0)
                            cubes.Add(new Cube() { X = i, Y = j, Z = k, IsEnclosedInternalGap= true });
                    }

            foreach (var c in cubes)
            {
                c.ExternalExposedFaces = 6 - cubes.Count(oc => c.IsNeighbourOf(oc));
            };

            Console.WriteLine(cubes.Sum(c => c.ExternalExposedFaces));

        }

        internal class Cube
        {
            public int X, Y, Z;

            public bool IsNeighbourOf(Cube other)
            {
                var displacements = new List<int>() { Math.Abs(X - other.X), Math.Abs(Y - other.Y), Math.Abs(Z - other.Z) };
                displacements.Sort();
                return displacements.ElementAt(0) == 0 && displacements.ElementAt(1) == 0 && displacements.ElementAt(2) == 1;
            }

            public int ExposedFaces;

            public bool IsEnclosedInternalGap;

            public int ExternalExposedFaces;
        }
    }
}
