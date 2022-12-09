using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode22
{
    internal class Day8
    {
        private static void DoDay8()
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(new FileStream("C:\\src\\juliahayward\\AdventOfCode22\\RawData\\8.txt",
                       FileMode.Open)))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }

            var height = lines.Count;
            var width = lines[0].Length;
            var treeHeights = new int[height, width];
            var visible = new bool[height, width];
            var totalVisible = 0;
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    // We could just do arithmetic on the chars!
                    treeHeights[i, j] = lines[i][j] - '0';
                }
            }
            // Sweep left
            for (var i = 0; i < height; i++)
            {
                var highestSoFar = -1;
                for (var j = 0; j < width; j++)
                {
                    // We could just do arithmetic on the chars!
                    if (treeHeights[i, j] > highestSoFar)
                    {
                        if (!visible[i, j]) totalVisible++;
                        visible[i, j] = true;
                        highestSoFar = treeHeights[i, j];
                    }
                }
            }
            // Sweep right
            for (var i = 0; i < height; i++)
            {
                var highestSoFar = -1;
                for (var j = width - 1; j >= 0; j--)
                {
                    if (treeHeights[i, j] > highestSoFar)
                    {
                        if (!visible[i, j]) totalVisible++;
                        visible[i, j] = true;
                        highestSoFar = treeHeights[i, j];
                    }
                }
            }
            // Sweep down
            for (var j = 0; j < width; j++)
            {
                var highestSoFar = -1;
                for (var i = 0; i < height; i++)
                {
                    if (treeHeights[i, j] > highestSoFar)
                    {
                        if (!visible[i, j]) totalVisible++;
                        visible[i, j] = true;
                        highestSoFar = treeHeights[i, j];
                    }
                }
            }
            // Sweep up
            for (var j = 0; j < width; j++)
            {
                var highestSoFar = -1;
                for (var i = height - 1; i >= 0; i--)
                {
                    if (treeHeights[i, j] > highestSoFar)
                    {
                        if (!visible[i, j]) totalVisible++;
                        visible[i, j] = true;
                        highestSoFar = treeHeights[i, j];
                    }
                }
            }

            Console.WriteLine(totalVisible);

            var scenicScores = new int[height, width];
            var highestScenicScore = 0;

            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    // We could just do arithmetic on the chars!
                    scenicScores[i, j] = ScenicScore(i, j, treeHeights);
                    if (scenicScores[i, j] > highestScenicScore)
                        highestScenicScore = scenicScores[i, j];
                }
            }

            Console.WriteLine(highestScenicScore);
        }

        private static int ScenicScore(int i, int j, int[,] treeHeights)
        {
            var eastScore = 0;
            if (j < treeHeights.GetLength(1) - 1) // if not, then it's on the eastern edge
            {
                // Look East
                for (var jj = j + 1; jj < treeHeights.GetLength(1); jj++)
                {
                    if (treeHeights[i, jj] >= treeHeights[i, j] || jj == treeHeights.GetLength(1) - 1)
                    {
                        eastScore = jj - j;
                        break;
                    }
                }
            }
            var westScore = 0;
            if (j > 0)
            {
                // Look East
                for (var jj = j - 1; jj >= 0; jj--)
                {
                    if (treeHeights[i, jj] >= treeHeights[i, j] || jj == 0)
                    {
                        westScore = j - jj;
                        break;
                    }
                }
            }
            var northScore = 0;
            if (i < treeHeights.GetLength(0) - 1)
            {
                // Look East
                for (var ii = i + 1; ii < treeHeights.GetLength(0); ii++)
                {
                    if (treeHeights[ii, j] >= treeHeights[i, j] || ii == treeHeights.GetLength(0) - 1)
                    {
                        northScore = ii - i;
                        break;
                    }
                }
            }
            var southScore = 0;
            if (i > 0)
            {
                // Look East
                for (var ii = i - 1; ii >= 0; ii--)
                {
                    if (treeHeights[ii, j] >= treeHeights[i, j] || ii == 0)
                    {
                        southScore = i - ii;
                        break;
                    }
                }
            }

            return southScore * northScore * eastScore * westScore;
        }
    }
}
