using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Schema;

namespace AdventOfCode22
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DoDay4();
            Console.ReadLine();
        }

        private static void DoDay4()
        {
            int totalRangesFullyContained = 0;
            int totalRangesOverlapping = 0;
            var reader = new StreamReader(new FileStream("C:\\src\\juliahayward\\AdventOfCode22\\RawData\\4.txt",
                FileMode.Open));
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var elfAllocations = line.Split(',');
                var firstElf = (lower: int.Parse(elfAllocations[0].Split('-')[0]),
                    higher: int.Parse(elfAllocations[0].Split('-')[1]));
                var secondElf = (lower: int.Parse(elfAllocations[1].Split('-')[0]),
                    higher: int.Parse(elfAllocations[1].Split('-')[1]));

                if ((firstElf.lower <= secondElf.lower && firstElf.higher >= secondElf.higher)
                    || (secondElf.lower <= firstElf.lower && secondElf.higher >= firstElf.higher))
                    totalRangesFullyContained++;

                if (!(firstElf.higher < secondElf.lower || secondElf.higher < firstElf.lower))
                    totalRangesOverlapping++;
            }

            Console.WriteLine(totalRangesFullyContained);
            Console.WriteLine(totalRangesOverlapping);
            reader.Close();
        }

        
    }
}
